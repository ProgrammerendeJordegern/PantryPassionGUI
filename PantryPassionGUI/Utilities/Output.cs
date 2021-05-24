namespace PantryPassionGUI.Utilities
{
    public class Output : IOutput
    {
        //Used to assert in unit testing
        public void OutputLine(string line)
        {
            System.Console.WriteLine(line);
        }
        
    }
}