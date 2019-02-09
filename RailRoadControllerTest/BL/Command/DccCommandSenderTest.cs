using System.IO.Ports;
using System.Text;
using NUnit.Framework;
using RailRoadController.BL.Command;
using NSubstitute;

namespace RailRoadControllerTest.BL.Command
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
        public void SendCommandShouldSendTheCommandToTheSerialPort()
        {
            var command = "command";
            _sut.SendCommand(command);
            _serialPort.Received().Write(Arg.Is<byte[]>(x => Encoding.UTF8.GetString(x).Equals(command)));
        }
    }
}
