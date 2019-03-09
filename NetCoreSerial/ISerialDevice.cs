namespace System.IO.Ports
{
    public interface ISerialDevice
    {
        bool IsOpened { get; }

        event Action<object, byte[]> DataReceived;

        void Close();
        void Open();
        void Write(byte[] buf);
    }
}