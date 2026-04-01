using System;
using System.Runtime.InteropServices;
using System.Reflection;

namespace ReFuel.Stb.Native
{
    /// <summary>
    /// Direct access to the native STBI function calls.
    /// </summary>
    public unsafe static partial class Stbi
    {
        private delegate void FailedAssertProc(byte *expression, byte *file, int line, byte *function);
        private static IntPtr stbiHandle;

        private static readonly string[] LibraryNames = new string[] 
        {
            //FIXME: This is wrong on so many levels, but, i need to do this
            // in order to get a change of this running.
            "runtimes/win-x64/native/libstbi.dll",
            "runtimes/win-x86/native/libstbi.dll",
            "runtimes/linux-arm/native/libstbi.so",
            "runtimes/linux-arm64/native/libstbi.so",
            "runtimes/linux-x64/native/libstbi.so",
            "runtimes/osx-arm64/libstbi.dylib",
            "runtimes/osx-x64/libstbi.dylib",
            "libstbi.dll",
            "libstbi.so",
            "libstbi.dylib",
        };

        static Stbi()
        {
            NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), Resolver);

            // Dummy call to fail_reason so we have a handle to STBI.
            failure_reason();

            // Load global address pointers.

            _tga_with_rle_ptr = (int*)NativeLibrary.GetExport(stbiHandle, "stbi_write_tga_with_rle");
            _png_compression_level_ptr = (int*)NativeLibrary.GetExport(stbiHandle, "stbi_write_png_compression_level");
            _force_png_filter_ptr = (int*)NativeLibrary.GetExport(stbiHandle, "stbi_write_force_png_filter");
        }

        private static IntPtr Resolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (libraryName != "stbi")
                return IntPtr.Zero;
            else if (stbiHandle != IntPtr.Zero)
                return stbiHandle;

            foreach (string name in LibraryNames)
            {
                if (NativeLibrary.TryLoad(name, assembly, searchPath, out stbiHandle))
                {
                    return stbiHandle;
                }
            }

            return stbiHandle = NativeLibrary.Load(libraryName);
        }
    }
}