using System;
using System.Media;

namespace PantryPassionGUI.Models
{
    public class SoundPlayer : ISoundPlayer
    {
        public SoundPlayer()
        {
            Mute = false;
        }

        public bool Mute { get; set; }

        public void Play()
        {
            if (Mute == false)
            {
                //Console.Beep();
                System.Media.SystemSounds.Beep.Play();
                
            }
        }
    }
}