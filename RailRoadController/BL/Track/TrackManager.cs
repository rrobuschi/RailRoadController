using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RailRoadController.BL.Track
{
    public interface ITrackManager
    {
        void EnableTrack();
        void DisableTrack();
        bool GetTrackStatus();

        event EventHandler TrackStatusChanged;
    }

    public class TrackManager : ITrackManager
    {
        private bool _trackOn;

        public bool GetTrackStatus()
        {
            return _trackOn;
        }

        public event EventHandler TrackStatusChanged;

        public void EnableTrack()
        {
            Console.WriteLine("TrackManager received command EnableTrack");
            _trackOn = true;

            EventArgs e = new EventArgs();
            TrackStatusChanged?.Invoke(this, e);
        }

        public void DisableTrack()
        {
            _trackOn = false;

            EventArgs e = new EventArgs();
            TrackStatusChanged?.Invoke(this, e);
        }
    }
}
