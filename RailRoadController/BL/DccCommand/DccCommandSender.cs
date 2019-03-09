using System;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace RailRoadController.BL.DccCommand
{
    public interface IDccCommandSender
    {
        void SendCommand(string command);
    }

    public class DccCommandSender : IDccCommandSender
    {
        private readonly ISerialDevice _serialPort;

        public DccCommandSender(ISerialDevice serialPort)
        {
            _serialPort = serialPort;
        }


        public void SendCommand(string command)
        {
            if (!_serialPort.IsOpened)
            {
                _serialPort.Open();
                Thread.Sleep(1000);
            }
            Console.WriteLine("DccCommandSender is sending command " + command);
            _serialPort.Write(Encoding.UTF8.GetBytes(command));
        }
    }

    public class DccCommandSenderMock : IDccCommandSender
    {
        public DccCommandSenderMock()
        {
        }


        public void SendCommand(string command)
        {
            Console.WriteLine(command);
        }
    }

}
