namespace PantryPassionGUI.Models
{
    public interface ISoundPlayer
    {
        void Play();
        bool Mute { get; set; }
    }
}
