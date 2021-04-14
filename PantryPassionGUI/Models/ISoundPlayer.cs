namespace PantryPassionGUI.Models
{
    interface ISoundPlayer
    {
        void Play();
        bool Mute { get; set; }
    }
}
