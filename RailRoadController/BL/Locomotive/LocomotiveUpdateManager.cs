using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Timers;
using RailRoadController.BL.DccCommand;
using RailRoadController.BL.Track;

namespace RailRoadController.BL.Locomotive
{
    public interface ILocomotiveUpdateManager
    {
    }

    public class LocomotiveUpdateManager : ILocomotiveUpdateManager
    {
        private readonly ConcurrentQueue<string> _commandQueue;
        private readonly IDccCommandBuilder _dccCommandBuilder;
        private readonly IDccCommandSender _dccCommandSender;
        private ILocomotivePersister _locomotivePersister;
        private readonly Timer _timer;
        private ITrackManager _trackManager;

        public LocomotiveUpdateManager(ILocomotivePersister locomotivePersister, IDccCommandBuilder dccCommandBuilder, IDccCommandSender dccCommandSender, ITrackManager trackManager)
        {
            _dccCommandBuilder = dccCommandBuilder;
            _dccCommandSender = dccCommandSender;
            _trackManager = trackManager;
            _locomotivePersister = locomotivePersister;
            var fleet = _locomotivePersister.LoadFleet();
            _commandQueue = new ConcurrentQueue<string>();
            _timer = new Timer(100);
            _timer.Elapsed += TimerElapsed;

            foreach (var locomotive in fleet)
            {
                locomotive.FunctionChanged += LocomotiveFunctionChanged;
                locomotive.MovementChanged += LocomotiveMovementChanged;
            }

            _trackManager.TrackStatusChanged += TrackStatusChanged;

            _timer.Start();
        }

        private void TrackStatusChanged(object sender, EventArgs e)
        {
            Console.WriteLine("LocomotiveUpdateController event handler TrackStatusChanged");
            var trackManager = (TrackManager) sender;
            var dccCommand = _dccCommandBuilder.BuildCommand(trackManager.GetTrackStatus());

            Console.WriteLine("Prepared DCC command " + dccCommand);
            _commandQueue.Enqueue(dccCommand);
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            while (_commandQueue.TryDequeue(out var dccCommand))
            {
                Console.WriteLine("Sending command " + dccCommand);
                _dccCommandSender.SendCommand(dccCommand);
            }
            _timer.Start();
        }

        private void LocomotiveMovementChanged(object sender, EventArgs e)
        {
            Console.WriteLine("LocomotiveUpdateController event handler LocomotiveMovementChanged");
            var locomotive = (Locomotive)sender;

            var dccCommand = _dccCommandBuilder.BuildCommand(locomotive.Address, locomotive.ProjectedPower.ToString(), locomotive.Direction.ToString());
            Console.WriteLine("Prepared DCC command " + dccCommand);
            _commandQueue.Enqueue(dccCommand);
        }

        private void LocomotiveFunctionChanged(object sender, EventArgs e)
        {
            var locomotive = (Locomotive)sender;

            var dccCommand = _dccCommandBuilder.BuildCommand(locomotive.Address, locomotive.Functions);

            _commandQueue.Enqueue(dccCommand);
        }
    }
}