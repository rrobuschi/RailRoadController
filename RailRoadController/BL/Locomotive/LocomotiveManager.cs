using System.Collections.Generic;
using System.Linq;

namespace RailRoadController.BL.Locomotive
{
    public interface ILocomotiveManager
    {
        void ToggleDirection(string dccAddress);

        void SetPower(string dccAddress, int newPower);

        void SetInertia(string dccAddress, int newInertia);

        void ToggleFunction(string dccAddress, int funcNumber);
    }

    public class LocomotiveManager : ILocomotiveManager
    {
        private readonly List<Locomotive> _fleet;

        public LocomotiveManager(List<Locomotive> fleet)
        {
            _fleet = fleet;
        }

        public void ToggleDirection(string dccAddress)
        {
            var locomotive = _fleet.SingleOrDefault(x => x.Address == dccAddress);

            locomotive?.ToggleDirection();
        }

        public void SetPower(string dccAddress, int newPower)
        {
            var locomotive = _fleet.SingleOrDefault(x => x.Address == dccAddress);

            locomotive?.SetPower(newPower);
        }

        public void SetInertia(string dccAddress, int newInertia)
        {
            var locomotive = _fleet.SingleOrDefault(x => x.Address == dccAddress);

            locomotive?.SetInertia(newInertia);
        }

        public void ToggleFunction(string dccAddress, int funcNumber)
        {
            var locomotive = _fleet.SingleOrDefault(x => x.Address == dccAddress);

            locomotive?.ToggleFunction(funcNumber);
        }
    }
}
