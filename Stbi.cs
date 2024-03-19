using System;
using System.Runtime.InteropServices;
using Quik.Stb.Image;

namespace Quik.Stb
{
    [NativeTypeName("unsigned int")]
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
        [NativeTypeName("int (*)(void *, char *, int)")]
        public IntPtr read;

        [NativeTypeName("void (*)(void *, int)")]
        public IntPtr skip;

        [NativeTypeName("int (*)(void *)")]
        public IntPtr eof;
    }

    public static unsafe partial class Stbi
    {
        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_load_from_memory", ExactSpelling = true)]
        [return: NativeTypeName("stbi_uc *")]
        public static extern byte* load_from_memory([NativeTypeName("const stbi_uc *")] byte* buffer, int len, int* x, int* y, int* channels_in_file, int desired_channels);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_load_from_callbacks", ExactSpelling = true)]
        [return: NativeTypeName("stbi_uc *")]
        public static extern byte* load_from_callbacks([NativeTypeName("const stbi_io_callbacks *")] stbi_io_callbacks* clbk, void* user, int* x, int* y, int* channels_in_file, int desired_channels);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_load", ExactSpelling = true)]
        [return: NativeTypeName("stbi_uc *")]
        public static extern byte* load([NativeTypeName("const char *")] sbyte* filename, int* x, int* y, int* channels_in_file, int desired_channels);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_load_from_file", ExactSpelling = true)]
        [return: NativeTypeName("stbi_uc *")]
        public static extern byte* load_from_file([NativeTypeName("FILE *")] void* f, int* x, int* y, int* channels_in_file, int desired_channels);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_load_gif_from_memory", ExactSpelling = true)]
        [return: NativeTypeName("stbi_uc *")]
        public static extern byte* load_gif_from_memory([NativeTypeName("const stbi_uc *")] byte* buffer, int len, int** delays, int* x, int* y, int* z, int* comp, int req_comp);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_load_16_from_memory", ExactSpelling = true)]
        [return: NativeTypeName("stbi_us *")]
        public static extern ushort* load_16_from_memory([NativeTypeName("const stbi_uc *")] byte* buffer, int len, int* x, int* y, int* channels_in_file, int desired_channels);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_load_16_from_callbacks", ExactSpelling = true)]
        [return: NativeTypeName("stbi_us *")]
        public static extern ushort* load_16_from_callbacks([NativeTypeName("const stbi_io_callbacks *")] stbi_io_callbacks* clbk, void* user, int* x, int* y, int* channels_in_file, int desired_channels);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_load_16", ExactSpelling = true)]
        [return: NativeTypeName("stbi_us *")]
        public static extern ushort* load_16([NativeTypeName("const char *")] sbyte* filename, int* x, int* y, int* channels_in_file, int desired_channels);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_load_from_file_16", ExactSpelling = true)]
        [return: NativeTypeName("stbi_us *")]
        public static extern ushort* load_from_file_16([NativeTypeName("FILE *")] void* f, int* x, int* y, int* channels_in_file, int desired_channels);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_loadf_from_memory", ExactSpelling = true)]
        public static extern float* loadf_from_memory([NativeTypeName("const stbi_uc *")] byte* buffer, int len, int* x, int* y, int* channels_in_file, int desired_channels);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_loadf_from_callbacks", ExactSpelling = true)]
        public static extern float* loadf_from_callbacks([NativeTypeName("const stbi_io_callbacks *")] stbi_io_callbacks* clbk, void* user, int* x, int* y, int* channels_in_file, int desired_channels);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_loadf", ExactSpelling = true)]
        public static extern float* loadf([NativeTypeName("const char *")] sbyte* filename, int* x, int* y, int* channels_in_file, int desired_channels);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_loadf_from_file", ExactSpelling = true)]
        public static extern float* loadf_from_file([NativeTypeName("FILE *")] void* f, int* x, int* y, int* channels_in_file, int desired_channels);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_hdr_to_ldr_gamma", ExactSpelling = true)]
        public static extern void hdr_to_ldr_gamma(float gamma);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_hdr_to_ldr_scale", ExactSpelling = true)]
        public static extern void hdr_to_ldr_scale(float scale);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_ldr_to_hdr_gamma", ExactSpelling = true)]
        public static extern void ldr_to_hdr_gamma(float gamma);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_ldr_to_hdr_scale", ExactSpelling = true)]
        public static extern void ldr_to_hdr_scale(float scale);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_is_hdr_from_callbacks", ExactSpelling = true)]
        public static extern int is_hdr_from_callbacks([NativeTypeName("const stbi_io_callbacks *")] stbi_io_callbacks* clbk, void* user);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_is_hdr_from_memory", ExactSpelling = true)]
        public static extern int is_hdr_from_memory([NativeTypeName("const stbi_uc *")] byte* buffer, int len);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_is_hdr", ExactSpelling = true)]
        public static extern int is_hdr([NativeTypeName("const char *")] sbyte* filename);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_is_hdr_from_file", ExactSpelling = true)]
        public static extern int is_hdr_from_file([NativeTypeName("FILE *")] void* f);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_failure_reason", ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* failure_reason();

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_image_free", ExactSpelling = true)]
        public static extern void image_free(void* retval_from_stbi_load);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_info_from_memory", ExactSpelling = true)]
        public static extern int info_from_memory([NativeTypeName("const stbi_uc *")] byte* buffer, int len, int* x, int* y, int* comp);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_info_from_callbacks", ExactSpelling = true)]
        public static extern int info_from_callbacks([NativeTypeName("const stbi_io_callbacks *")] stbi_io_callbacks* clbk, void* user, int* x, int* y, int* comp);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_is_16_bit_from_memory", ExactSpelling = true)]
        public static extern int is_16_bit_from_memory([NativeTypeName("const stbi_uc *")] byte* buffer, int len);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_is_16_bit_from_callbacks", ExactSpelling = true)]
        public static extern int is_16_bit_from_callbacks([NativeTypeName("const stbi_io_callbacks *")] stbi_io_callbacks* clbk, void* user);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_info", ExactSpelling = true)]
        public static extern int info([NativeTypeName("const char *")] sbyte* filename, int* x, int* y, int* comp);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_info_from_file", ExactSpelling = true)]
        public static extern int info_from_file([NativeTypeName("FILE *")] void* f, int* x, int* y, int* comp);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_is_16_bit", ExactSpelling = true)]
        public static extern int is_16_bit([NativeTypeName("const char *")] sbyte* filename);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_is_16_bit_from_file", ExactSpelling = true)]
        public static extern int is_16_bit_from_file([NativeTypeName("FILE *")] void* f);

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
        [return: NativeTypeName("char *")]
        public static extern sbyte* zlib_decode_malloc_guesssize([NativeTypeName("const char *")] sbyte* buffer, int len, int initial_size, int* outlen);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_zlib_decode_malloc_guesssize_headerflag", ExactSpelling = true)]
        [return: NativeTypeName("char *")]
        public static extern sbyte* zlib_decode_malloc_guesssize_headerflag([NativeTypeName("const char *")] sbyte* buffer, int len, int initial_size, int* outlen, int parse_header);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_zlib_decode_malloc", ExactSpelling = true)]
        [return: NativeTypeName("char *")]
        public static extern sbyte* zlib_decode_malloc([NativeTypeName("const char *")] sbyte* buffer, int len, int* outlen);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_zlib_decode_buffer", ExactSpelling = true)]
        public static extern int zlib_decode_buffer([NativeTypeName("char *")] sbyte* obuffer, int olen, [NativeTypeName("const char *")] sbyte* ibuffer, int ilen);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_zlib_decode_noheader_malloc", ExactSpelling = true)]
        [return: NativeTypeName("char *")]
        public static extern sbyte* zlib_decode_noheader_malloc([NativeTypeName("const char *")] sbyte* buffer, int len, int* outlen);

        [DllImport("stbi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "stbi_zlib_decode_noheader_buffer", ExactSpelling = true)]
        public static extern int zlib_decode_noheader_buffer([NativeTypeName("char *")] sbyte* obuffer, int olen, [NativeTypeName("const char *")] sbyte* ibuffer, int ilen);
    }
}
