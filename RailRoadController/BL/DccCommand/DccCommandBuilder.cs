using RailRoadController.Entities;

namespace RailRoadController.BL.DccCommand
{
    public interface IDccCommandBuilder
    {
        string BuildCommand(string dccAddress, string power, string direction);

        string BuildCommand(bool tracksOn);

        string BuildCommand(string dccAddress, FunctionSet dccFunctions, bool locomotiveOn);
    }
    
    public class DccCommandBuilder : IDccCommandBuilder
    {
        public string BuildCommand(string dccAddress, string power, string direction)
        {
            return "<t 1 " + dccAddress + " " + power + " " + direction + ">";
        }

        public string BuildCommand(bool tracksOn)
        {
            return tracksOn ? "<1>" : "<0>";
        }

        public string BuildCommand(string dccAddress, FunctionSet dccFunctions, bool locomotiveOn)
        {
            var output = "";

            var value = 128;
            if (dccFunctions.F1)
                value += 1;
            if (dccFunctions.F2)
                value += 2;
            if (dccFunctions.F3)
                value += 4;
            if (dccFunctions.F4)
                value += 8;
            if (locomotiveOn)
                value += 16;

            output = AddCommand(output, "<f " + dccAddress + " " + value + ">");

            value = 176;
            if (dccFunctions.F5)
                value += 1;
            if (dccFunctions.F6)
                value += 2;
            if (dccFunctions.F7)
                value += 4;
            if (dccFunctions.F8)
                value += 8;

            output = AddCommand(output, "<f " + dccAddress + " " + value + ">");
            return output;
        }

        private string AddCommand(string startingCommandString, string newCommand)
        {
            var output = startingCommandString;
            if (!string.IsNullOrEmpty(output))
            {
                output += ";";
            }
            output += newCommand;
            return output;
        }
    }
}
