using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using FluentAssertions;
using NSubstitute;
using RailRoadController.BL.DccCommand;
using RailRoadController.BL.Locomotive;
using RailRoadController.BL.Track;
using RailRoadController.Entities;

namespace RailRoadControllerTest.BL.Locomotive
{
    public class LocomotiveUpdateManagerTest
    {
        private LocomotiveUpdateManager _sut;
        private List<RailRoadController.BL.Locomotive.Locomotive> _fleet;
        private IDccCommandBuilder _dccCommandBuilder;
        private IDccCommandSender _dccCommandSender;
        private ITrackManager _trackManager;

        [SetUp]
        public void Setup()
        {
            _fleet = new List<RailRoadController.BL.Locomotive.Locomotive>
            {
                new RailRoadController.BL.Locomotive.Locomotive
                {
                    Address = "01", Name = "locomotive 1", Direction = 0, ProjectedPower = 0
                }
            };
            _dccCommandBuilder = Substitute.For<IDccCommandBuilder>();
            _dccCommandSender = Substitute.For<IDccCommandSender>();
            _sut = new LocomotiveUpdateManager(_fleet, _dccCommandBuilder, _dccCommandSender, _trackManager);
        }

        [Test]
        public void LocomotiveUpdateManager_object_exists()
        {
            _sut.Should().NotBeNull();
        }

        [Test]
        public void an_update_to_locomotive_power_causes_a_dcc_command_to_be_sent()
        {
            _dccCommandBuilder.BuildCommand("01", "50", "0").Returns("dccCommand50");

            var locomotive = _fleet[0];
            locomotive.SetPower(50);

            Thread.Sleep(150);

            _dccCommandSender.Received().SendCommand("dccCommand50");
        }

        [Test]
        public void an_update_to_locomotive_direction_causes_a_dcc_command_to_be_sent()
        {
            _dccCommandBuilder.BuildCommand("01", "0", "1").Returns("dccCommanddir1");

            var locomotive = _fleet[0];
            locomotive.ToggleDirection();

            Thread.Sleep(150);

            _dccCommandSender.Received().SendCommand("dccCommanddir1");
        }

        [Test]
        public void an_update_to_locomotive_function_causes_a_dcc_command_to_be_sent()
        {
            FunctionSet functions ;
            _dccCommandBuilder.BuildCommand("01", Arg.Is<FunctionSet>(x => x.F1)).Returns("dccCommandfun1");

            var locomotive = _fleet[0];
            locomotive.ToggleFunction(1);

            Thread.Sleep(150);

            _dccCommandSender.Received().SendCommand("dccCommandfun1");
        }

    }
}
