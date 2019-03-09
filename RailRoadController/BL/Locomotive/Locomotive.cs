using System;
using RailRoadController.Controllers;
using RailRoadController.Entities;

namespace RailRoadController.BL.Locomotive
{
    public interface ILocomotive
    {
        string Name { get; set; }
        string Address { get; set; }
        int ProjectedPower { get; set; }
        int CurrentPower { get; set; }
        int Direction { get; set; }
        int Inertia { get; set; }
        string LastCommandSent { get; set; }
        FunctionSet Functions { get; set; }
        void ToggleDirection();
        void SetPower(int newPower);
        void SetInertia(int newInertia);
        void ToggleFunction(int funcNumber);

        event EventHandler MovementChanged;
        event EventHandler FunctionChanged;
    }

    public class Locomotive : ILocomotive
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int ProjectedPower { get; set; }
        public int CurrentPower { get; set; }
        public int Direction { get; set; }
        public int Inertia { get; set; }
        public string LastCommandSent { get; set; }
        public FunctionSet Functions { get; set; }

        public event EventHandler MovementChanged;
        public event EventHandler FunctionChanged;

        public Locomotive()
        {
            Functions = new FunctionSet();
        }

        protected virtual void RaiseMovementChanged()
        {
            Console.WriteLine("Locomotive.RaiseMovementChanged " + Address);
            EventArgs e = new EventArgs();
            MovementChanged?.Invoke(this, e);
        }

        protected virtual void RaiseFunctionChanged()
        {
            EventArgs e = new EventArgs();
            FunctionChanged?.Invoke(this, e);
        }

        public void ToggleDirection()
        {
            Direction = Direction == 1 ? 0 : 1;

            RaiseMovementChanged();
        }

        public void SetPower(int newPower)
        {
            ProjectedPower = newPower;

            RaiseMovementChanged();
        }

        public void SetInertia(int newInertia)
        {
            Inertia = newInertia;

            RaiseMovementChanged();
        }

        public void ToggleFunction(int funcNumber)
        {
            if (funcNumber == 0)
                Functions.F0 = !Functions.F0;
            else if (funcNumber == 1)
                Functions.F1 = !Functions.F1;
            else if (funcNumber == 2)
                Functions.F2 = !Functions.F2;
            else if (funcNumber == 3)
                Functions.F3 = !Functions.F3;
            else if (funcNumber == 4)
                Functions.F4 = !Functions.F4;
            else if (funcNumber == 5)
                Functions.F5 = !Functions.F5;
            else if (funcNumber == 6)
                Functions.F6 = !Functions.F6;
            else if (funcNumber == 7)
                Functions.F7 = !Functions.F7;
            else if (funcNumber == 8)
                Functions.F8 = !Functions.F8;

            RaiseFunctionChanged();
        }

        public LocomotiveStatus MapToLocomotiveStatus()
        {
            var output = new LocomotiveStatus
            {
                DccAddress = Address,
                Name = Name,
                Power = ProjectedPower,
                Direction = Direction,
                Functions = new FunctionSet
                {
                    F0 = Functions.F0,
                    F1 = Functions.F1,
                    F2 = Functions.F2,
                    F3 = Functions.F3,
                    F4 = Functions.F4,
                    F5 = Functions.F5,
                    F6 = Functions.F6,
                    F7 = Functions.F7,
                    F8 = Functions.F8
                },
                Inertia = Inertia
            };
            return output;
        }
    }
}
