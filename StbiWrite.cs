using System;
using System.Runtime.InteropServices;

namespace ReFuel.Stb
{
    public unsafe delegate void StbiWriteProc(void* context, void* data, int size);

    public unsafe partial class Stbi
    {
        private static readonly int* _tga_with_rle_ptr;
        private static readonly int* _png_compression_level_ptr;
        private static readonly int* _forced_png_filter_ptr;

        public static int write_tga_with_rle
        {
            get => *_tga_with_rle_ptr;
            set => *_tga_with_rle_ptr = value;
        }

        public static int write_png_compression_level
        { 
            get => *_png_compression_level_ptr;
            set => *_png_compression_level_ptr = value;
        }

        public static int write_force_png_filter
        {
            get => *_forced_png_filter_ptr;
            set => *_forced_png_filter_ptr = value;
        }

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_write_png_to_func")]
        public static extern int write_png_to_func(IntPtr func, void* context, int w, int h, int comp, void* data, int stride_in_bytes);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_write_bmp_to_func")]
        public static extern int write_bmp_to_func(IntPtr func, void* context, int w, int h, int comp, void* data);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_write_tga_to_func")]
        public static extern int write_tga_to_func(IntPtr func, void* context, int w, int h, int comp, void* data);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_write_hdr_to_func")]
        public static extern int write_hdr_to_func(IntPtr func, void* context, int w, int h, int comp, void* data);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_write_jpg_to_func")]
        public static extern int write_jpg_to_func(IntPtr func, void* context, int w, int h, int comp, void* data, int quality);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_flip_vertically_on_write")]
        public static extern int flip_vertically_on_write(int value);
    }
}
