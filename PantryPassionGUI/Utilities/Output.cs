namespace PantryPassionGUI.Utilities
{
    public class Output : IOutput
    {
        public void OutputLine(string line)
        {
            System.Console.WriteLine(line);
        }
        
    }
}