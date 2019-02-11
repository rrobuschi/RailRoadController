namespace RailRoadController.Entities
{
    public class Locomotive
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public byte ProjectedPower { get; set; }
        public byte CurrentPower { get; set; }
        public int Direction { get; set; }
        public byte Inertia { get; set; }
        public bool On { get; set; }
        public string LastCommandSent { get; set; }
        public FunctionSet Functions { get; set; }

        public Locomotive()
        {
            Functions = new FunctionSet();
        }
    }
}
