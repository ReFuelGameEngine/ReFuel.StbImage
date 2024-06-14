using System;
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
        public static bool FlipVerticallyOnLoad { set => Stbi.set_flip_vertically_on_load(1); }

        /// <summary>
        /// Set to unpremultiply images on load.
        /// </summary>
        /// <remarks>
        /// According to the stb_image documentation, only iPhone PNG images
        /// can come with premultiplied alpha.
        /// </remarks>
        public static bool UnpremultiplyOnLoad { set => Stbi.set_unpremultiply_on_load(1); }

        /// <summary>
        /// Try loading an image, without raising exceptions.
        /// </summary>
        /// <param name="image">The resulting image.</param>
        /// <param name="stream">Source stream.</param>
        /// <param name="format">The desired image format.</param>
        /// <returns>True on success.</returns>
        public static bool TryLoad(out StbImage image, Stream stream, StbiImageFormat format = StbiImageFormat.Default, bool isFloat = false)
        {
            int x, y, iFormat;
            StbiStreamWrapper wrapper = new StbiStreamWrapper(stream, true);
            wrapper.CreateCallbacks(out stbi_io_callbacks cb);

            stream.Position = 0;
            IntPtr imagePtr;
            if (isFloat)
            {
                imagePtr = (IntPtr)Stbi.loadf_from_callbacks(&cb, null, &x, &y, &iFormat, (int)format);
            }
            else
            {
                imagePtr = (IntPtr)Stbi.load_from_callbacks(&cb, null, &x, &y, &iFormat, (int)format);
            }

            if (imagePtr != IntPtr.Zero)
            {
                image = new StbImage(imagePtr, x, y, (StbiImageFormat)iFormat, isFloat);
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
        public static bool TryLoad<T>(out StbImage image, ReadOnlySpan<T> span, StbiImageFormat format = StbiImageFormat.Default, bool isFloat = false)
            where T : unmanaged
        {
            IntPtr imagePtr = IntPtr.Zero;
            int x, y, iFormat;
            fixed (byte *ptr = MemoryMarshal.AsBytes(span))
            {
                if (isFloat)
                {
                    imagePtr = (IntPtr)Stbi.loadf_from_memory(ptr, span.Length * sizeof(T), &x, &y, &iFormat, (int)format);
                }
                else
                {
                    imagePtr = (IntPtr)Stbi.load_from_memory(ptr, span.Length * sizeof(T), &x, &y, &iFormat, (int)format);
                }

                if (imagePtr != IntPtr.Zero)
                {
                    image = new StbImage(imagePtr, x, y, (StbiImageFormat)iFormat, isFloat);
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
        public static StbImage Load(Stream stream, StbiImageFormat format = StbiImageFormat.Default, bool isFloat = false)
        {
            if (TryLoad(out StbImage image, stream, format, isFloat))
            {
                return image;
            }

            string reason = Marshal.PtrToStringUTF8((IntPtr)Stbi.failure_reason());
            throw new Exception($"Failed to load image: {reason}");
        }

        /// <summary>
        /// Load an image.
        /// </summary>
        /// <param name="span">The span of memory to load from.</param>
        /// <param name="format">The desired image format.</param>
        /// <returns>The image object.</returns>
        public static StbImage Load<T>(ReadOnlySpan<T> span, StbiImageFormat format = StbiImageFormat.Default, bool isFloat = false)
            where T : unmanaged
        {
            if (TryLoad(out StbImage image, span, format, isFloat))
            {
                return image;
            }

            string reason = Marshal.PtrToStringUTF8((IntPtr)Stbi.failure_reason());
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
    }
}