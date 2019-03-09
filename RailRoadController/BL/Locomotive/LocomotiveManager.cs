using System.Collections.Generic;
using System.Linq;

namespace RailRoadController.BL.Locomotive
{
    public interface ILocomotiveManager
    {
        void SetPower(string dccAddress, int newPower);

        void SetInertia(string dccAddress, int newInertia);

        void ToggleFunction(string dccAddress, int funcNumber);

        void SetDirection(string dccAddress, int direction);

        Locomotive GetLocomotive(string dccAddress);
    }

    public class LocomotiveManager : ILocomotiveManager
    {
        private readonly List<Locomotive> _fleet;
        private readonly ILocomotivePersister _locomotivePersister;

        public LocomotiveManager(ILocomotivePersister locomotivePersister)
        {
            _locomotivePersister = locomotivePersister;
            _fleet = _locomotivePersister.LoadFleet();
        }

        public void SetPower(string dccAddress, int newPower)
        {
            var locomotive = _fleet.SingleOrDefault(x => x.Address == dccAddress);

            if (locomotive != null && locomotive.ProjectedPower != newPower)
            {
                locomotive.SetPower(newPower);
            }
        }

        public void SetInertia(string dccAddress, int newInertia)
        {
            var locomotive = _fleet.SingleOrDefault(x => x.Address == dccAddress);

            if (locomotive != null && locomotive.Inertia != newInertia)
            {
                locomotive.SetInertia(newInertia);
            }
        }

        public void ToggleFunction(string dccAddress, int funcNumber)
        {
            var locomotive = _fleet.SingleOrDefault(x => x.Address == dccAddress);

            locomotive?.ToggleFunction(funcNumber);
        }

        public void SetDirection(string dccAddress, int newDirection)
        {
            var locomotive = _fleet.SingleOrDefault(x => x.Address == dccAddress);

            if (locomotive != null && locomotive.Direction != newDirection)
            {
                locomotive.ToggleDirection();
            }
        }

        public Locomotive GetLocomotive(string dccAddress)
        {
            return _fleet.Single(x => x.Address == dccAddress);
        }
    }
}
