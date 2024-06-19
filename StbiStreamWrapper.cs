using ReFuel.Stb.Native;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ReFuel.Stb
{
    /// <summary>
    /// Pointer to STBI stream read function.
    /// </summary>
    /// <param name="userdata">User provided userdata pointer.</param>
    /// <param name="buffer">C array to read into.</param>
    /// <param name="count">Size of the C array in bytes.</param>
    /// <returns>The number of bytes read from the stream.</returns>
    public unsafe delegate int StbiReadProc(void *userdata, byte* buffer, int count);
    /// <summary>
    /// Pointer to STBI stream skip function.
    /// </summary>
    /// <param name="userdata">User provided userdata pointer.</param>
    /// <param name="count">Number of bytes to skip.</param>
    public unsafe delegate void StbiSkipProc(void *userdata, int count);
    /// <summary>
    /// Pointer to STBI stream end of file function.
    /// </summary>
    /// <param name="userdata">User provided userdata pointer.</param>
    /// <returns>Non-zero value if the end of the stream has been reached.</returns>
    public unsafe delegate int StbiEofProc(void *userdata);

    /// <summary>
    /// An easy to use stream wrapper for use with STBI image load functions.
    /// </summary>
    public unsafe class StbiStreamWrapper : IDisposable
    {
        private readonly stbi_io_callbacks _callbacks;
        private readonly Stream _stream;
        private readonly bool _keepOpen;
        private bool _isDisposed;

        private StbiReadProc _readCb;
        private StbiSkipProc _skipCb;
        private StbiEofProc _eofCb;

        public ref readonly stbi_io_callbacks Callbacks => ref _callbacks;

        public StbiStreamWrapper(Stream stream, bool keepOpen = false)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            _stream = stream;
            _keepOpen = keepOpen;

            _readCb = ReadCb;
            _skipCb = SkipCb;
            _eofCb = EofCb;

            _callbacks = default;
            _callbacks.read = Marshal.GetFunctionPointerForDelegate<StbiReadProc>(_readCb);
            _callbacks.skip = Marshal.GetFunctionPointerForDelegate<StbiSkipProc>(_skipCb);
            _callbacks.eof = Marshal.GetFunctionPointerForDelegate<StbiEofProc>(_eofCb);
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

    /// <summary>
    /// An easy to use stream wrapper for STBI image write functions.
    /// </summary>
    /// <remarks>Keep struct alive for the duration of the write operation.</remarks>
    public struct StbiWriteStreamWrapper
    {
        private readonly Stream _stream;
        private readonly StbiWriteProc _cb;

        public IntPtr Callback => Marshal.GetFunctionPointerForDelegate(_cb);

        public StbiWriteStreamWrapper(Stream stream)
        {
            _stream = stream;
            unsafe
            {
                _cb = WriteCb;
            }
        }

        private unsafe void WriteCb(void *context, void *data, int size)
        {
            _stream.Write(new ReadOnlySpan<byte>((byte*)data, size));
        }

        public static implicit operator IntPtr(in StbiWriteStreamWrapper wrapper) => wrapper.Callback;
    }
}