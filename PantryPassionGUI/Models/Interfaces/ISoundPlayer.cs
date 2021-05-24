namespace PantryPassionGUI.Models.Interfaces
{
    public interface ISoundPlayer
    {
        void Play();
        bool Mute { get; set; }
    }
}
