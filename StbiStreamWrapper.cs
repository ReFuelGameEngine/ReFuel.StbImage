using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Quik.Stb
{
    public unsafe class StbiStreamWrapper : IDisposable
    {
        private Stream _stream;
        private bool _keepOpen;
        private bool _isDisposed;

        private delegate int ReadProc(void *userdata, byte* buffer, int count);
        private delegate void SkipProc(void *userdata, int count);
        private delegate int Eof(void *userdata);

        public StbiStreamWrapper(Stream stream, bool keepOpen = false)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            _stream = stream;
            _keepOpen = keepOpen;
        }

        public void CreateCallbacks(out stbi_io_callbacks cb)
        {
            cb = default;
            cb.read = Marshal.GetFunctionPointerForDelegate<ReadProc>(ReadCb);
            cb.skip = Marshal.GetFunctionPointerForDelegate<SkipProc>(SkipCb);
            cb.eof = Marshal.GetFunctionPointerForDelegate<Eof>(EofCb);
        }

        private int ReadCb(void *userdata, byte* buffer, int count)
        {
            Span<byte> bytes = new Span<byte>(buffer, count);
            return _stream.Read(bytes);
        }

        private void SkipCb(void *userdata, int count)
        {
            _stream.Seek(count, SeekOrigin.Current);
        }

        private int EofCb(void *userdata)
        {
            if (!_stream.CanRead || _stream.Position == _stream.Length)
                return 1;
            return 0;
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            if (!_keepOpen) _stream.Dispose();

            _isDisposed = true;
        }
    }
}