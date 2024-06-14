using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ReFuel.Stb
{
    public unsafe delegate int StbiReadProc(void *userdata, byte* buffer, int count);
    public unsafe delegate void StbiSkipProc(void *userdata, int count);
    public unsafe delegate int StbiEofProc(void *userdata);

    public unsafe class StbiStreamWrapper : IDisposable
    {
        private Stream _stream;
        private bool _keepOpen;
        private bool _isDisposed;

        private StbiReadProc _readCb;
        private StbiSkipProc _skipCb;
        private StbiEofProc _eofCb;

        public StbiStreamWrapper(Stream stream, bool keepOpen = false)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            _stream = stream;
            _keepOpen = keepOpen;

            _readCb = ReadCb;
            _skipCb = SkipCb;
            _eofCb = EofCb;
        }

        public void CreateCallbacks(out stbi_io_callbacks cb)
        {
            cb = default;
            cb.read = Marshal.GetFunctionPointerForDelegate<StbiReadProc>(_readCb);
            cb.skip = Marshal.GetFunctionPointerForDelegate<StbiSkipProc>(_skipCb);
            cb.eof = Marshal.GetFunctionPointerForDelegate<StbiEofProc>(_eofCb);
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