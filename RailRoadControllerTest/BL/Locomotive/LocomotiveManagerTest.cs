using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using RailRoadController.BL.Locomotive;

namespace RailRoadControllerTest.BL.Locomotive
{
    public class LocomotiveManagerTest
    {
        private ILocomotiveManager _sut;
        private List<RailRoadController.BL.Locomotive.Locomotive> _fleet;

        [SetUp]
        public void Setup()
        {
            var fleet = 
                new List<RailRoadController.BL.Locomotive.Locomotive>
                {
                    new RailRoadController.BL.Locomotive.Locomotive
                        {Address = "01", Name = "Locomotive01", Direction = 0, ProjectedPower = 0, Inertia = 0},
                    new RailRoadController.BL.Locomotive.Locomotive
                        {Address = "02", Name = "Locomotive02", Direction = 0, ProjectedPower = 0, Inertia = 0}
                };
            _fleet = fleet;
            _sut = new LocomotiveManager(fleet);
        }

        [Test]
        public void SetDirection_should_toggle_the_direction_of_the_specified_locomotive()
        {
            var dccAddress = "01";
            _fleet.Single(x => x.Address == dccAddress).Direction = 0;

            _sut.SetDirection(dccAddress, 1);

            _fleet.Single(x => x.Address == dccAddress).Direction.Should().Be(1);
        }

        [Test]
        public void SetPower_should_set_the_projected_power_of_the_specified_locomotive()
        {
            var dccAddress = "02";
            _fleet.Single(x => x.Address == dccAddress).ProjectedPower = 0;

            _sut.SetPower(dccAddress, 100);

            _fleet.Single(x => x.Address == dccAddress).ProjectedPower.Should().Be(100);
        }

        [Test]
        public void SetInertia_should_set_the_inertia_of_the_specified_locomotive()
        {
            var dccAddress = "01";
            _fleet.Single(x => x.Address == dccAddress).Inertia = 0;

            _sut.SetInertia(dccAddress, 100);

            _fleet.Single(x => x.Address == dccAddress).Inertia.Should().Be(100);
        }

        [Test]
        public void ToggleFunction_should_toggle_the_specified_function_of_the_specified_locomotive()
        {
            var dccAddress = "02";
            _fleet.Single(x => x.Address == dccAddress).Functions.F0 = false;

            _sut.ToggleFunction(dccAddress, 0);

            _fleet.Single(x => x.Address == dccAddress).Functions.F0.Should().BeTrue();
        }
    }
}
