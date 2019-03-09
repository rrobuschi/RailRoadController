using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Ports
{
    public class SerialDevice : IDisposable, ISerialDevice
    {
        public const int READING_BUFFER_SIZE = 1024;

        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        private CancellationToken CancellationToken => cts.Token;

        private int? _fd;
        private readonly IntPtr readingBuffer = Marshal.AllocHGlobal(READING_BUFFER_SIZE);

        protected readonly string portName;

        protected readonly BaudRate baudRate;

        public event Action<object, byte[]> DataReceived;
        public SerialDevice(string portName, BaudRate baudRate)
        {
            this.portName = portName;
            this.baudRate = baudRate;
        }

        public void Open()
        {
            Console.WriteLine("Opening serial port " + portName);
            // open serial port
            int fd = Libc.open(portName, Libc.OpenFlags.O_RDWR | Libc.OpenFlags.O_NONBLOCK);

            if (fd == -1)
            {
                throw new ApplicationException($"failed to open port ({portName})");
            }

            // set baud rate
            byte[] termiosData = new byte[256];

            Libc.tcgetattr(fd, termiosData);
            Libc.cfsetspeed(termiosData, baudRate);
            Libc.tcsetattr(fd, 0, termiosData);
            // start reading
            Task.Run((Action)StartReading, CancellationToken);
            this._fd = fd;
        }

        private void StartReading()
        {
            if (!_fd.HasValue)
            {
                throw new Exception();
            }

            while (true)
            {
                CancellationToken.ThrowIfCancellationRequested();

                int res = Libc.read(_fd.Value, readingBuffer, READING_BUFFER_SIZE);

                if (res != -1)
                {
                    byte[] buf = new byte[res];
                    Marshal.Copy(readingBuffer, buf, 0, res);

                    OnDataReceived(buf);
                }

                Thread.Sleep(50);
            }
        }

        protected virtual void OnDataReceived(byte[] data)
        {
            DataReceived?.Invoke(this, data);
        }

        public bool IsOpened => _fd.HasValue;

        public void Close()
        {
            Console.WriteLine("Closing serial port");
            if (!_fd.HasValue)
            {
                throw new ApplicationException();
            }

            cts.Cancel();
            Libc.close(_fd.Value);
            Marshal.FreeHGlobal(readingBuffer);
        }

        public void Write(byte[] buf)
        {
            if (!_fd.HasValue)
            {
                throw new ApplicationException();
            }

            Console.WriteLine("Sending serial data " + Encoding.UTF8.GetString(buf));

            IntPtr ptr = Marshal.AllocHGlobal(buf.Length);
            Marshal.Copy(buf, 0, ptr, buf.Length);
            Libc.write(_fd.Value, ptr, buf.Length);
            Marshal.FreeHGlobal(ptr);
        }

        public static string[] GetPortNames()
        {
            int p = (int)Environment.OSVersion.Platform;
            List<string> serial_ports = new List<string>();

            // Are we on Unix?
            if (p == 4 || p == 128 || p == 6)
            {
                string[] ttys = System.IO.Directory.GetFiles("/dev/", "tty*");
                foreach (string dev in ttys)
                {
                    //Arduino MEGAs show up as ttyACM due to their different USB<->RS232 chips
                    if (dev.StartsWith("/dev/ttyS") 
                        || dev.StartsWith("/dev/ttyUSB") 
                        || dev.StartsWith("/dev/ttyACM") 
                        || dev.StartsWith("/dev/ttyAMA") 
                        || dev.StartsWith("/dev/serial"))
                    {
                        serial_ports.Add(dev);
                    }
                }
                //newer Pi with bluetooth map serial
                ttys = System.IO.Directory.GetFiles("/dev/", "serial*");
                foreach (string dev in ttys)
                {
                    serial_ports.Add(dev);
                }
            }
            
            return serial_ports.ToArray();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            if (IsOpened)
            {
                Close();
            }
        }
    }
}