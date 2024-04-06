using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Quik.Stb
{
    public unsafe static partial class Stbi
    {
        private delegate void FailedAssertProc(byte *expression, byte *file, int line, byte *function);

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

            // quik_stbi_failed_assert_store(Marshal.GetFunctionPointerForDelegate<FailedAssertProc>(FailedAssert));
        }

        private static IntPtr Resolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (libraryName != "stbi")
                return IntPtr.Zero;

            foreach (string name in LibraryNames)
            {
                if (NativeLibrary.TryLoad(name, assembly, searchPath, out IntPtr handle))
                {
                    return handle;
                }
            }

            return NativeLibrary.Load(libraryName);
        }

        private static void FailedAssert(byte *expression, byte *file, int line, byte *function)
        {
            string expr = expression == null ? string.Empty : Marshal.PtrToStringUTF8((IntPtr)expression);
            string f    = file       == null ? string.Empty : Marshal.PtrToStringUTF8((IntPtr)file);
            string func = function   == null ? string.Empty : Marshal.PtrToStringUTF8((IntPtr)function);

            Exception ex = new Exception("Assert failed in native stbi code.");
                ex.Data.Add("Expression", expr);
                ex.Data.Add("File", f);
                ex.Data.Add("Line", line);
                ex.Data.Add("Function", func);
            throw ex;
        }
    }
}