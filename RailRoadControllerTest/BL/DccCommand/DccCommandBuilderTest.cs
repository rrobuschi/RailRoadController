using FluentAssertions;
using NUnit.Framework;
using RailRoadController.BL.DccCommand;
using RailRoadController.Entities;

namespace RailRoadControllerTest.BL.DccCommand
{
    public class DccCommandBuilderTest
    {
        private IDccCommandBuilder _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new DccCommandBuilder();
        }

        [Test]
        public void BuildCommandShouldBuildCorrectLocomotiveCommand()
        {
            var actual = _sut.BuildCommand("01", "50", "1");
            actual.Should().Be("<t 1 01 50 1>");
        }

        [TestCase(true, "<1>")]
        [TestCase(false, "<0>")]
        public void BuildCommandShouldBuildCorrectOnOffCommand(bool tracks, string expected)
        {
            var actual = _sut.BuildCommand(tracks);
            actual.Should().Be(expected);
        }

        [TestCase("01", true, false, false, false, false, false, false, false, false, "<f 01 144>;<f 01 176>")]
        public void BuildCommandShouldBuildCorrectFunctionsCommand(string dccAddress, bool locomotiveOn, bool f1,
            bool f2, bool f3, bool f4, bool f5, bool f6, bool f7, bool f8, string expected)
        {
            var functionSet = new FunctionSet
            {
                F1 = f1,
                F2 = f2,
                F3 = f3,
                F4 = f4,
                F5 = f5,
                F6 = f6,
                F7 = f7,
                F8 = f8
            };
            var actual = _sut.BuildCommand(dccAddress, functionSet, locomotiveOn);
            actual.Should().Be(expected);
        }
    }
}
