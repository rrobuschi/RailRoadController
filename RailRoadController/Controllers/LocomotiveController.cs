using System;
using Microsoft.AspNetCore.Mvc;
using RailRoadController.BL.Locomotive;
using RailRoadController.BL.Track;
using RailRoadController.Entities;

namespace RailRoadController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocomotiveController : ControllerBase
    {
        private readonly ILocomotiveManager _locomotiveManager;
        private ILocomotiveUpdateManager _locomotiveUpdateManager;
        private ITrackManager _trackManager;

        public LocomotiveController(ILocomotiveManager locomotiveManager, ILocomotiveUpdateManager locomotiveUpdateManager, ITrackManager trackManager)
        {
            _locomotiveManager = locomotiveManager;
            _locomotiveUpdateManager = locomotiveUpdateManager;
            _trackManager = trackManager;
        }

        [Route("[action]")]
        public void SetLocomotiveMovement([FromBody] MovementCommand command)
        {
            Console.WriteLine("SetLocomotiveMovement received");
            _locomotiveManager.SetInertia(command.DccAddress, command.Inertia);
            _locomotiveManager.SetPower(command.DccAddress, command.Power);
            _locomotiveManager.SetDirection(command.DccAddress, command.Direction);
        }

        [Route("[action]")]
        public void SetLocomotiveFunctions([FromBody] FunctionCommand command)
        {
            _locomotiveManager.ToggleFunction(command.DccAddress, command.FunctionId);
        }

        [Route("[action]")]
        public LocomotiveStatus GetLocomotiveStatus(string dccAddress)
        {
            var locomotive = _locomotiveManager.GetLocomotive(dccAddress);

            var output = locomotive.MapToLocomotiveStatus();

            return output;
        }

        [Route("[action]")]
        public void EnableTrack()
        {
            Console.WriteLine("LocomotiveController received EnableTrack command");
            _trackManager.EnableTrack();
        }

        [Route("[action]")]
        public void DisableTrack()
        {
            _trackManager.DisableTrack();
        }
    }

    public class LocomotiveStatus
    {
        public string DccAddress { get; set; }
        public string Name { get; set; }
        public int Power { get; set; }
        public int Direction { get; set; }
        public int Inertia { get; set; }
        public FunctionSet Functions { get; set; }
    }
}
