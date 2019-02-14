using System;
using FluentAssertions;
using NUnit.Framework;

namespace RailRoadControllerTest.BL.Locomotive
{
    public class LocomotiveTest
    {
        private RailRoadController.BL.Locomotive.ILocomotive _sut;
        private bool _movementEventReceived;
        private bool _functionEventReceived;
        private RailRoadController.BL.Locomotive.Locomotive _eventReceivedLocomotive;

        [SetUp]
        public void Setup()
        {
            _sut = new RailRoadController.BL.Locomotive.Locomotive();
            _sut.MovementChanged += MovementChangedHandler;
            _sut.FunctionChanged += FunctionChangedHandler;
        }

        private void FunctionChangedHandler(object sender, EventArgs e)
        {
            _eventReceivedLocomotive = (RailRoadController.BL.Locomotive.Locomotive) sender;
            _functionEventReceived = true;
        }

        private void MovementChangedHandler(object sender, EventArgs e)
        {
            _eventReceivedLocomotive = (RailRoadController.BL.Locomotive.Locomotive)sender;
            _movementEventReceived = true;
        }

        [Test]
        public void Locomotive_class_exists()
        {
            var loc = new RailRoadController.BL.Locomotive.Locomotive();

            loc.Should().NotBeNull();
        }

        [Test]
        public void ToggleDirection_changes_the_direction_and_raises_event()
        {
            _sut.Direction = 0;
            _movementEventReceived = false;

            _sut.ToggleDirection();

            _sut.Direction.Should().Be(1);
            _movementEventReceived.Should().BeTrue();
            _eventReceivedLocomotive.Direction.Should().Be(1);
        }

        [Test]
        public void baubau()
        {
            _sut.Functions.F1 = false;
            _functionEventReceived = false;

            _sut.ToggleFunction(1);

            _sut.Functions.F1.Should().BeTrue();
            _functionEventReceived.Should().BeTrue();
            _eventReceivedLocomotive.Functions.F1.Should().BeTrue();
        }
    }
}
