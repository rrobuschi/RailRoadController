namespace RailRoadController.Entities
{
    public class MovementCommand
    {
        public string DccAddress { get; set; }
        public int Direction { get; set; }
        public int Power { get; set; }
        public int Inertia { get; set; }
    }
}