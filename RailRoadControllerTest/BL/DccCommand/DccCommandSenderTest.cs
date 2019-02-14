using System.IO.Ports;
using System.Text;
using NSubstitute;
using NUnit.Framework;
using RailRoadController.BL.DccCommand;

namespace RailRoadControllerTest.BL.DccCommand
{
    public class DccCommandSenderTest
    {
        private IDccCommandSender _sut;
        private ISerialDevice _serialPort;

        [SetUp]
        public void Setup()
        {
            _serialPort = Substitute.For<ISerialDevice>();
            _sut = new DccCommandSender(_serialPort);
        }

        [Test]
        public void SendCommand_should_send_the_command_to_the_serial_port()
        {
            var command = "command";
            _sut.SendCommand(command);
            _serialPort.Received().Write(Arg.Is<byte[]>(x => Encoding.UTF8.GetString(x).Equals(command)));
        }
    }
}
