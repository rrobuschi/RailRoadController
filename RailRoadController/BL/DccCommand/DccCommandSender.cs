using System.IO.Ports;
using System.Text;

namespace RailRoadController.BL.DccCommand
{
    public interface IDccCommandSender
    {
        void SendCommand(string command);
    }

    public class DccCommandSender : IDccCommandSender
    {
        private ISerialDevice _serialPort;

        public DccCommandSender(ISerialDevice serialPort)
        {
            _serialPort = serialPort;
        }


        public void SendCommand(string command)
        {
            _serialPort.Write(Encoding.UTF8.GetBytes(command));
        }
    }
}
