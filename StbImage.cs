using ReFuel.Stb.Native;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;

namespace ReFuel.Stb
{
    /// <summary>
    /// A class that encompasses all features of stb_image.h in a safe way.
    /// </summary>
    public unsafe class StbImage : IDisposable
    {
        private bool isDisposed = false;

        /// <summary>
        /// Pointer to the image.
        /// </summary>
        public IntPtr ImagePointer { get; }

        /// <summary>
        /// Width of the image.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Height of the image.
        /// </summary>
        /// <value></value>
        public int Height { get; }

        /// <summary>
        /// Internal image format.
        /// </summary>
        public StbiImageFormat Format { get; }
        /// <summary>
        /// True if the image is a floating point image.
        /// </summary>
        public bool IsFloat { get; }

        private StbImage(IntPtr image, int x, int y, StbiImageFormat format, bool isFloat)
        {
            ImagePointer = image;
            Width = x;
            Height = y;
            Format = format;
            IsFloat = isFloat;
        }

        ~StbImage()
        {
            Dispose(false);
        }

        /// <summary>
        /// Get a safe span to the image pointer.
        /// </summary>
        /// <typeparam name="T">The pixel type.</typeparam>
        /// <returns>A span to the image data.</returns>
        /// <exception cref="Exception">The image uses an unexpected image format.</exception>
        public ReadOnlySpan<T> AsSpan<T>() where T : unmanaged
        {
            int sz = Format switch
            {
                StbiImageFormat.Grey => 1,
                StbiImageFormat.GreyAlpha => 2,
                StbiImageFormat.Rgb => 3,
                StbiImageFormat.Rgba => 4,
                _ => throw new Exception("unkown image format")
            } * (IsFloat ? sizeof(float) : sizeof(byte));

            return new ReadOnlySpan<T>((T*)ImagePointer, Width * Height * sz / sizeof(T));
        }

        /// <summary>
        /// Write image to a PNG file.
        /// </summary>
        /// <param name="dest">The destination stream.</param>
        /// <remarks>
        /// Incurs a conversion cost if the image format is not a byte format. Affected by non-thread safe global options.
        /// </remarks>
        public void WritePng(Stream dest) => WritePng(AsSpan<byte>(), Width, Height, Format, dest, isFloat: IsFloat);
        
        /// <summary>
        /// Write image to a BMP file.
        /// </summary>
        /// <param name="dest">The destination stream.</param>
        /// <remarks>
        /// Incurs a conversion cost if the image format is not a byte format. Affected by non-thread safe global options.
        /// </remarks>
        public void WriteBmp(Stream dest) => WriteBmp(AsSpan<byte>(), Width, Height, Format, dest, isFloat: IsFloat);
        
        /// <summary>
        /// Write image to a TGA file.
        /// </summary>
        /// <param name="dest">The destination stream.</param>
        /// <remarks>
        /// Incurs a conversion cost if the image format is not a byte format. Ignores alpha channel. Affected by non-thread safe global options.
        /// </remarks>
        public void WriteTga(Stream dest) => WriteTga(AsSpan<byte>(), Width, Height, Format, dest, isFloat: IsFloat);
        
        /// <summary>
        /// Write image to a PNG file.
        /// </summary>
        /// <param name="dest">The destination stream.</param>
        /// <remarks>
        /// Incurs a conversion cost if the image format is not a float format. Affected by non-thread safe global options.
        /// </remarks>
        public void WriteHdr(Stream dest) => WriteHdr(AsSpan<byte>(), Width, Height, Format, dest, isFloat: IsFloat);
        
        /// <summary>
        /// Write image to a PNG file.
        /// </summary>
        /// <param name="dest">The destination stream.</param>
        /// <remarks>
        /// Incurs a conversion cost if the image format is not a byte format. Ignores alpha channel. Affected by non-thread safe global options.
        /// </remarks>
        public void WriteJpg(Stream dest, int quality = 90) => WriteJpg(AsSpan<byte>(), Width, Height, Format, dest, quality: quality, isFloat: IsFloat);

        public void Dispose() => Dispose(true);

        private void Dispose(bool disposing)
        {
            if (isDisposed) return;

            if (disposing)
            {
                GC.SuppressFinalize(this);
            }

            Stbi.image_free(ImagePointer.ToPointer());
            isDisposed = true;
        }

        /// <summary>
        /// Set to flip the y-axis of loaded images on load.
        /// </summary>
        public static bool FlipVerticallyOnLoad { set => Stbi.set_flip_vertically_on_load(value ? 1 : 0); }

        /// <summary>
        /// Set to flip the y-axis of saved images.
        /// </summary>
        public static bool FlipVerticallyOnSave { set => Stbi.flip_vertically_on_write(value ? 1 : 0); }

        /// <summary>
        /// Set to unpremultiply images on load.
        /// </summary>
        /// <remarks>
        /// According to the stb_image documentation, only iPhone PNG images
        /// can come with premultiplied alpha.
        /// </remarks>
        public static bool UnpremultiplyOnLoad { set => Stbi.set_unpremultiply_on_load(1); }

        /// <summary>
        /// Force a filter on PNG filter when saving.
        /// </summary>
        /// <remarks>
        /// -1 for auto, 0 through 5 to pick a filter. Higher is more. Not thread safe.
        /// </remarks>
        public int WriteForcePngFilter
        {
            get => Stbi.write_force_png_filter;
            set
            {
                if (value < -1 || value > 5)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "The PNG filter must be in the range 0 to 5, or -1 for auto.");
                }

                Stbi.write_force_png_filter = value;
            }
        }

        /// <summary>
        /// Change the PNG compression level on save.
        /// </summary>
        /// <remarks>
        /// Higher is more. Defaults to 8. Not thread safe.
        /// </remarks>
        public int WritePngCompressionLevel
        {
            get => Stbi.write_png_compression_level;
            set => Stbi.write_png_compression_level = value;
        }

        /// <summary>
        /// Enable run length encoding on TGA images on save.
        /// </summary>
        /// <remarks>
        /// Not thread safe.
        /// </remarks>
        public bool WriteTgaEnableRLE
        {
            get => Stbi.write_tga_with_rle != 0;
            set => Stbi.write_tga_with_rle = value ? 1 : 0;
        }

        /// <summary>
        /// Try loading an image, without raising exceptions.
        /// </summary>
        /// <param name="image">The resulting image.</param>
        /// <param name="stream">Source stream.</param>
        /// <param name="format">The desired image format.</param>
        /// <returns>True on success.</returns>
        public static bool TryLoad([NotNullWhen(true)] out StbImage? image, Stream stream, StbiImageFormat format = StbiImageFormat.Default, bool asFloat = false)
        {
            int x, y, iFormat;
            StbiStreamWrapper wrapper = new StbiStreamWrapper(stream, true);
            wrapper.CreateCallbacks(out stbi_io_callbacks cb);

            stream.Position = 0;
            IntPtr imagePtr;
            if (asFloat)
            {
                imagePtr = (IntPtr)Stbi.loadf_from_callbacks(&cb, null, &x, &y, &iFormat, (int)format);
            }
            else
            {
                imagePtr = (IntPtr)Stbi.load_from_callbacks(&cb, null, &x, &y, &iFormat, (int)format);
            }

            if (imagePtr != IntPtr.Zero)
            {
                image = new StbImage(imagePtr, x, y, (StbiImageFormat)iFormat, asFloat);
                return true;
            }
            else
            {
                image = null;
                return false;
            }
        }

        /// <summary>
        /// Try loading an image, without raising exceptions.
        /// </summary>
        /// <param name="image">The resulting image.</param>
        /// <param name="span">Source memory span.</param>
        /// <param name="format">The desired image format.</param>
        /// <returns>True on success.</returns>
        public static bool TryLoad([NotNullWhen(true)] out StbImage? image, ReadOnlySpan<byte> span, StbiImageFormat format = StbiImageFormat.Default, bool asFloat = false)
        {
            IntPtr imagePtr = IntPtr.Zero;
            int x, y, iFormat;
            fixed (byte *ptr = MemoryMarshal.AsBytes(span))
            {
                if (asFloat)
                {
                    imagePtr = (IntPtr)Stbi.loadf_from_memory(ptr, span.Length, &x, &y, &iFormat, (int)format);
                }
                else
                {
                    imagePtr = (IntPtr)Stbi.load_from_memory(ptr, span.Length, &x, &y, &iFormat, (int)format);
                }

                if (imagePtr != IntPtr.Zero)
                {
                    image = new StbImage(imagePtr, x, y, (StbiImageFormat)iFormat, asFloat);
                    return true;
                }
                else
                {
                    image = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// Load an image.
        /// </summary>
        /// <param name="stream">The stream to load from.</param>
        /// <param name="format">The desired image format.</param>
        /// <returns>The image object.</returns>
        public static StbImage Load(Stream stream, StbiImageFormat format = StbiImageFormat.Default, bool asFloat = false)
        {
            if (TryLoad(out StbImage? image, stream, format, asFloat))
            {
                return image;
            }

            string reason = Marshal.PtrToStringUTF8((IntPtr)Stbi.failure_reason())!;
            throw new Exception($"Failed to load image: {reason}");
        }

        /// <summary>
        /// Load an image.
        /// </summary>
        /// <param name="span">The span of memory to load from.</param>
        /// <param name="format">The desired image format.</param>
        /// <returns>The image object.</returns>
        public static StbImage Load(ReadOnlySpan<byte> span, StbiImageFormat format = StbiImageFormat.Default, bool asFloat = false)
        {
            if (TryLoad(out StbImage? image, span, format, asFloat))
            {
                return image;
            }

            string reason = Marshal.PtrToStringUTF8((IntPtr)Stbi.failure_reason())!;
            throw new Exception($"Failed to load image: {reason}");
        }

        /// <summary>
        /// Peek image info from a stream.
        /// </summary>
        /// <param name="stream">The stream to peek into.</param>
        /// <param name="width">Width of the image.</param>
        /// <param name="height">Height of the image.</param>
        /// <param name="format">The image format.</param>
        /// <returns>True if the stream contained an image.</returns>
        public static bool PeekInfo(Stream stream, out int width, out int height, out StbiImageFormat format)
        {
            int x, y, iFormat;
            StbiStreamWrapper wrapper = new StbiStreamWrapper(stream, true);
            wrapper.CreateCallbacks(out stbi_io_callbacks cb);

            stream.Position = 0;
            int result = Stbi.info_from_callbacks(&cb, null, &x, &y, &iFormat);

            width = x;
            height = y;
            format = (StbiImageFormat)iFormat;

            return result != 0;
        }

        /// <summary>
        /// Peek image info from a span.
        /// </summary>
        /// <param name="span">The span to peek into.</param>
        /// <param name="width">Width of the image.</param>
        /// <param name="height">Height of the image.</param>
        /// <param name="format">The image format.</param>
        /// <returns>True if the stream contained an image.</returns>
        public static bool PeekInfo<T>(ReadOnlySpan<T> span, out int width, out int height, out StbiImageFormat format) where T : unmanaged
        {
            fixed (byte* ptr = MemoryMarshal.AsBytes(span))
            {
                int x, y, iFormat;
                int result = Stbi.info_from_memory(ptr, span.Length * sizeof(T), &x, &y, &iFormat);
                width = x;
                height = y;
                format = (StbiImageFormat)iFormat;
                return result != 0;
            }
        }

        private static int Components(StbiImageFormat format) => format switch
        {
            StbiImageFormat.Grey => 1,
            StbiImageFormat.GreyAlpha => 2,
            StbiImageFormat.Rgb => 3,
            StbiImageFormat.Rgba => 4,
            _ => throw new ArgumentException("Expected a fully qualified format.")
        };

        private static byte[] ConvertFloatToByte<T>(ReadOnlySpan<T> source, int width, int height, int components)
            where T : unmanaged
        {
            byte[] conversion = new byte[width * height * components];
            ReadOnlySpan<float> dataAsFloat = MemoryMarshal.Cast<T, float>(source);

            for (int i = 0; i<conversion.Length; i++)
            {
                conversion[i] = (byte) Math.Clamp(MathF.Round(dataAsFloat[i]* 255), 0, 255);
            }

            return conversion;
        }

        private static float[] ConvertByteToFloat<T>(ReadOnlySpan<T> source, int width, int height, int components)
            where T : unmanaged
        {
            float[] conversion = new float[width * height * components];
            ReadOnlySpan<byte> dataAsByte = MemoryMarshal.Cast<T, byte>(source);

            for (int i = 0; i < conversion.Length; i++)
            {
                conversion[i] = Math.Clamp(dataAsByte[i]/255f, 0f, 1f);
            }

            return conversion;
        }

        /// <summary>
        /// Write any image to a PNG file.
        /// </summary>
        /// <typeparam name="T">Any packed byte or float array structure.</typeparam>
        /// <param name="data">Span of pixel data.</param>
        /// <param name="width">Width of the pixel data in pixels.</param>
        /// <param name="height">Height of the pixel data in pixels.</param>
        /// <param name="format">Color format of the pixel data. Must not be <see cref="StbiImageFormat.StbiImageFormat"/>.</param>
        /// <param name="destination">The destination stream.</param>
        /// <param name="isFloat">True if the pixel format in data is a floating point format.</param>
        /// <remarks>
        /// This will incur a conversion cost if the pixel format is not a byte format. Affected by global non-thread safe options.
        /// </remarks>
        public static void WritePng<T>(ReadOnlySpan<T> data, int width, int height, StbiImageFormat format, Stream destination, bool isFloat = false)
            where T : unmanaged
        {
            int components = Components(format);
            ReadOnlySpan<byte> source;
            byte[]? conversion;

            if (isFloat)
            {
                conversion = ConvertFloatToByte(data, width, height, components);
                source = conversion;
            }
            else
            {
                source = MemoryMarshal.AsBytes(data);
            }

            StbiWriteStreamWrapper wrapper = new StbiWriteStreamWrapper(destination);

            fixed (byte *ptr = source)
                Stbi.write_png_to_func(wrapper, null, width, height, components, ptr, width * components);
        }

        /// <summary>
        /// Write any image to a BMP file.
        /// </summary>
        /// <typeparam name="T">Any packed byte or float array structure.</typeparam>
        /// <param name="data">Span of pixel data.</param>
        /// <param name="width">Width of the pixel data in pixels.</param>
        /// <param name="height">Height of the pixel data in pixels.</param>
        /// <param name="format">Color format of the pixel data. Must not be <see cref="StbiImageFormat.StbiImageFormat"/>.</param>
        /// <param name="destination">The destination stream.</param>
        /// <param name="isFloat">True if the pixel format in data is a floating point format.</param>
        /// <remarks>
        /// This will incur a conversion cost if the pixel format is not a byte format. Ignores the alpha channel. Affected by global non-thread safe options.
        /// </remarks>
        public static void WriteBmp<T>(ReadOnlySpan<T> data, int width, int height, StbiImageFormat format, Stream destination, bool isFloat = false)
            where T : unmanaged
        {
            int components = Components(format);
            ReadOnlySpan<byte> source;
            byte[]? conversion;

            if (isFloat)
            {
                conversion = ConvertFloatToByte(data, width, height, components);
                source = conversion;
            }
            else
            {
                source = MemoryMarshal.AsBytes(data);
            }

            StbiWriteStreamWrapper wrapper = new StbiWriteStreamWrapper(destination);

            fixed (byte* ptr = source)
                Stbi.write_bmp_to_func(wrapper, null, width, height, components, ptr);
        }

        /// <summary>
        /// Write any image to a TGA file.
        /// </summary>
        /// <typeparam name="T">Any packed byte or float array structure.</typeparam>
        /// <param name="data">Span of pixel data.</param>
        /// <param name="width">Width of the pixel data in pixels.</param>
        /// <param name="height">Height of the pixel data in pixels.</param>
        /// <param name="format">Color format of the pixel data. Must not be <see cref="StbiImageFormat.StbiImageFormat"/>.</param>
        /// <param name="destination">The destination stream.</param>
        /// <param name="isFloat">True if the pixel format in data is a floating point format.</param>
        /// <remarks>
        /// This will incur a conversion cost if the pixel format is not a byte format. Affected by global non-thread safe options.
        /// </remarks>
        public static void WriteTga<T>(ReadOnlySpan<T> data, int width, int height, StbiImageFormat format, Stream destination, bool isFloat = false)
            where T : unmanaged
        {
            int components = Components(format);
            ReadOnlySpan<byte> source;
            byte[]? conversion;

            if (isFloat)
            {
                conversion = ConvertFloatToByte(data, width, height, components);
                source = conversion;
            }
            else
            {
                source = MemoryMarshal.AsBytes(data);
            }

            StbiWriteStreamWrapper wrapper = new StbiWriteStreamWrapper(destination);

            fixed (byte* ptr = source)
                Stbi.write_tga_to_func(wrapper, null, width, height, components, ptr);
        }

        /// <summary>
        /// Write any image to a PNG file.
        /// </summary>
        /// <typeparam name="T">Any packed byte or float array structure.</typeparam>
        /// <param name="data">Span of pixel data.</param>
        /// <param name="width">Width of the pixel data in pixels.</param>
        /// <param name="height">Height of the pixel data in pixels.</param>
        /// <param name="format">Color format of the pixel data. Must not be <see cref="StbiImageFormat.StbiImageFormat"/>.</param>
        /// <param name="destination">The destination stream.</param>
        /// <param name="isFloat">True if the pixel format in data is a floating point format.</param>
        /// <remarks>
        /// This will incur a conversion cost if the pixel format is not a float format. Affected by global non-thread safe options.
        /// </remarks>
        public static void WriteHdr<T>(ReadOnlySpan<T> data, int width, int height, StbiImageFormat format, Stream destination, bool isFloat = false)
            where T : unmanaged
        {
            int components = Components(format);
            ReadOnlySpan<float> source;
            float[]? conversion;

            if (!isFloat)
            {
                conversion = ConvertByteToFloat(data, width, height, components);
                source = conversion;
            }
            else
            {
                source = MemoryMarshal.Cast<T, float>(data);
            }

            StbiWriteStreamWrapper wrapper = new StbiWriteStreamWrapper(destination);

            fixed (float* ptr = source)
                Stbi.write_hdr_to_func(wrapper, null, width, height, components, ptr);
        }

        /// <summary>
        /// Write any image to a PNG file.
        /// </summary>
        /// <typeparam name="T">Any packed byte or float array structure.</typeparam>
        /// <param name="data">Span of pixel data.</param>
        /// <param name="width">Width of the pixel data in pixels.</param>
        /// <param name="height">Height of the pixel data in pixels.</param>
        /// <param name="format">Color format of the pixel data. Must not be <see cref="StbiImageFormat.StbiImageFormat"/>.</param>
        /// <param name="destination">The destination stream.</param>
        /// <param name="isFloat">True if the pixel format in data is a floating point format.</param>
        /// <remarks>
        /// This will incur a conversion cost if the pixel format is not a byte format. Ignores the alpha channel. Affected by global non-thread safe options.
        /// </remarks>
        public static void WriteJpg<T>(ReadOnlySpan<T> data, int width, int height, StbiImageFormat format, Stream destination, int quality = 90, bool isFloat = false)
            where T : unmanaged
        {
            int components = Components(format);
            ReadOnlySpan<byte> source;
            byte[]? conversion;

            if (isFloat)
            {
                conversion = ConvertFloatToByte(data, width, height, components);
                source = conversion;
            }
            else
            {
                source = MemoryMarshal.AsBytes(data);
            }

            StbiWriteStreamWrapper wrapper = new StbiWriteStreamWrapper(destination);

            fixed (byte* ptr = source)
                Stbi.write_jpg_to_func(wrapper, null, width, height, components, ptr, quality);
        }
    }
}