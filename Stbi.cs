using System;
using System.Runtime.InteropServices;

namespace ReFuel.Stb.Native
{
    public enum StbiEnum : uint
    {
        STBI_default = 0,
        STBI_grey = 1,
        STBI_grey_alpha = 2,
        STBI_rgb = 3,
        STBI_rgb_alpha = 4,
    }

    public partial struct stbi_io_callbacks
    {
        public IntPtr read;

        public IntPtr skip;

        public IntPtr eof;
    }

    public static unsafe partial class Stbi
    {
        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_load_from_memory", ExactSpelling = true)]

        public static extern byte* load_from_memory(byte* buffer, int len, int* x, int* y, int* channels_in_file, int desired_channels);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_load_from_callbacks", ExactSpelling = true)]

        public static extern byte* load_from_callbacks(stbi_io_callbacks* clbk, void* user, int* x, int* y, int* channels_in_file, int desired_channels);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_load", ExactSpelling = true)]

        public static extern byte* load(sbyte* filename, int* x, int* y, int* channels_in_file, int desired_channels);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_load_from_file", ExactSpelling = true)]

        public static extern byte* load_from_file(void* f, int* x, int* y, int* channels_in_file, int desired_channels);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_load_gif_from_memory", ExactSpelling = true)]

        public static extern byte* load_gif_from_memory(byte* buffer, int len, int** delays, int* x, int* y, int* z, int* comp, int req_comp);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_load_16_from_memory", ExactSpelling = true)]

        public static extern ushort* load_16_from_memory(byte* buffer, int len, int* x, int* y, int* channels_in_file, int desired_channels);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_load_16_from_callbacks", ExactSpelling = true)]

        public static extern ushort* load_16_from_callbacks(stbi_io_callbacks* clbk, void* user, int* x, int* y, int* channels_in_file, int desired_channels);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_load_16", ExactSpelling = true)]

        public static extern ushort* load_16(sbyte* filename, int* x, int* y, int* channels_in_file, int desired_channels);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_load_from_file_16", ExactSpelling = true)]

        public static extern ushort* load_from_file_16(void* f, int* x, int* y, int* channels_in_file, int desired_channels);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_loadf_from_memory", ExactSpelling = true)]
        public static extern float* loadf_from_memory(byte* buffer, int len, int* x, int* y, int* channels_in_file, int desired_channels);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_loadf_from_callbacks", ExactSpelling = true)]
        public static extern float* loadf_from_callbacks(stbi_io_callbacks* clbk, void* user, int* x, int* y, int* channels_in_file, int desired_channels);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_loadf", ExactSpelling = true)]
        public static extern float* loadf(sbyte* filename, int* x, int* y, int* channels_in_file, int desired_channels);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_loadf_from_file", ExactSpelling = true)]
        public static extern float* loadf_from_file(void* f, int* x, int* y, int* channels_in_file, int desired_channels);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_hdr_to_ldr_gamma", ExactSpelling = true)]
        public static extern void hdr_to_ldr_gamma(float gamma);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_hdr_to_ldr_scale", ExactSpelling = true)]
        public static extern void hdr_to_ldr_scale(float scale);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_ldr_to_hdr_gamma", ExactSpelling = true)]
        public static extern void ldr_to_hdr_gamma(float gamma);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_ldr_to_hdr_scale", ExactSpelling = true)]
        public static extern void ldr_to_hdr_scale(float scale);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_is_hdr_from_callbacks", ExactSpelling = true)]
        public static extern int is_hdr_from_callbacks(stbi_io_callbacks* clbk, void* user);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_is_hdr_from_memory", ExactSpelling = true)]
        public static extern int is_hdr_from_memory(byte* buffer, int len);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_is_hdr", ExactSpelling = true)]
        public static extern int is_hdr(sbyte* filename);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_is_hdr_from_file", ExactSpelling = true)]
        public static extern int is_hdr_from_file(void* f);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_failure_reason", ExactSpelling = true)]

        public static extern sbyte* failure_reason();

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_image_free", ExactSpelling = true)]
        public static extern void image_free(void* retval_from_stbi_load);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_info_from_memory", ExactSpelling = true)]
        public static extern int info_from_memory(byte* buffer, int len, int* x, int* y, int* comp);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_info_from_callbacks", ExactSpelling = true)]
        public static extern int info_from_callbacks(stbi_io_callbacks* clbk, void* user, int* x, int* y, int* comp);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_is_16_bit_from_memory", ExactSpelling = true)]
        public static extern int is_16_bit_from_memory(byte* buffer, int len);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_is_16_bit_from_callbacks", ExactSpelling = true)]
        public static extern int is_16_bit_from_callbacks(stbi_io_callbacks* clbk, void* user);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_info", ExactSpelling = true)]
        public static extern int info(sbyte* filename, int* x, int* y, int* comp);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_info_from_file", ExactSpelling = true)]
        public static extern int info_from_file(void* f, int* x, int* y, int* comp);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_is_16_bit", ExactSpelling = true)]
        public static extern int is_16_bit(sbyte* filename);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_is_16_bit_from_file", ExactSpelling = true)]
        public static extern int is_16_bit_from_file(void* f);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_set_unpremultiply_on_load", ExactSpelling = true)]
        public static extern void set_unpremultiply_on_load(int flag_true_if_should_unpremultiply);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_convert_iphone_png_to_rgb", ExactSpelling = true)]
        public static extern void convert_iphone_png_to_rgb(int flag_true_if_should_convert);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_set_flip_vertically_on_load", ExactSpelling = true)]
        public static extern void set_flip_vertically_on_load(int flag_true_if_should_flip);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_set_unpremultiply_on_load_thread", ExactSpelling = true)]
        public static extern void set_unpremultiply_on_load_thread(int flag_true_if_should_unpremultiply);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_convert_iphone_png_to_rgb_thread", ExactSpelling = true)]
        public static extern void convert_iphone_png_to_rgb_thread(int flag_true_if_should_convert);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_set_flip_vertically_on_load_thread", ExactSpelling = true)]
        public static extern void set_flip_vertically_on_load_thread(int flag_true_if_should_flip);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_zlib_decode_malloc_guesssize", ExactSpelling = true)]

        public static extern sbyte* zlib_decode_malloc_guesssize(sbyte* buffer, int len, int initial_size, int* outlen);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_zlib_decode_malloc_guesssize_headerflag", ExactSpelling = true)]

        public static extern sbyte* zlib_decode_malloc_guesssize_headerflag(sbyte* buffer, int len, int initial_size, int* outlen, int parse_header);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_zlib_decode_malloc", ExactSpelling = true)]

        public static extern sbyte* zlib_decode_malloc(sbyte* buffer, int len, int* outlen);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_zlib_decode_buffer", ExactSpelling = true)]
        public static extern int zlib_decode_buffer(sbyte* ibuffer, int ilen);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_zlib_decode_noheader_malloc", ExactSpelling = true)]

        public static extern sbyte* zlib_decode_noheader_malloc(sbyte* buffer, int len, int* outlen);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_zlib_decode_noheader_buffer", ExactSpelling = true)]
        public static extern int zlib_decode_noheader_buffer(sbyte* ibuffer, int ilen);
    }
}
