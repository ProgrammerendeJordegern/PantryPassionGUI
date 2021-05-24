namespace PantryPassionGUI.Models.Interfaces
{
    public interface ITimer<T>
    {
        void Enable();
        void Disable();
        T GetTimer();
    }
}
