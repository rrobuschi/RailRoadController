using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Timers;
using RailRoadController.BL.DccCommand;

namespace RailRoadController.BL.Locomotive
{
    public class LocomotiveUpdateManager
    {
        private readonly ConcurrentQueue<string> _commandQueue;
        private readonly IDccCommandBuilder _dccCommandBuilder;
        private readonly IDccCommandSender _dccCommandSender;
        private readonly Timer _timer;

        public LocomotiveUpdateManager(List<Locomotive> fleet, IDccCommandBuilder dccCommandBuilder, IDccCommandSender dccCommandSender)
        {
            _dccCommandBuilder = dccCommandBuilder;
            _dccCommandSender = dccCommandSender;
            _commandQueue = new ConcurrentQueue<string>();
            _timer = new Timer(100);
            _timer.Elapsed += TimerElapsed;

            foreach (var locomotive in fleet)
            {
                locomotive.FunctionChanged += LocomotiveFunctionChanged;
                locomotive.MovementChanged += LocomotiveMovementChanged;
            }

            _timer.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            while (_commandQueue.TryDequeue(out var dccCommand))
            {
                _dccCommandSender.SendCommand(dccCommand);
            }
            _timer.Start();
        }

        private void LocomotiveMovementChanged(object sender, EventArgs e)
        {
            var locomotive = (Locomotive)sender;

            var dccCommand = _dccCommandBuilder.BuildCommand(locomotive.Address, locomotive.ProjectedPower.ToString(), locomotive.Direction.ToString());

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