using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using RailRoadController.BL.Track;

namespace RailRoadControllerTest.BL.Track
{
    public class TrackManagerTest
    {
        private ITrackManager _sut;
        private bool _eventTriggered;

        [SetUp]
        public void Setup()
        {
            _sut = new TrackManager();
            _eventTriggered = false;
            _sut.TrackStatusChanged += SutOnTrackStatusChanged;
        }

        private void SutOnTrackStatusChanged(object sender, EventArgs e)
        {
            _eventTriggered = true;
        }

        [Test]
        public void EnableTrack_should_store_status_and_trigger_event()
        {
            _sut.EnableTrack();

            _sut.GetTrackStatus().Should().BeTrue();
            _eventTriggered.Should().BeTrue();
        }

        [Test]
        public void DisableTrack_should_store_status_and_trigger_event()
        {
            _sut.DisableTrack();

            _sut.GetTrackStatus().Should().BeFalse();
            _eventTriggered.Should().BeTrue();
        }


    }
}
