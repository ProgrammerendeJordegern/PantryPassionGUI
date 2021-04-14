namespace PantryPassionGUI.Models
{
    public interface ITimer<T>
    {
        void Enable();
        void Disable();
        T GetTimer();
    }
}
