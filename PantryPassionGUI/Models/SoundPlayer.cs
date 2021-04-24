using System;
using System.Media;
using System.Threading;

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

                //System.Media.SoundPlayer soundPlayer =
                //    new System.Media.SoundPlayer(@"C:\Users\Simon Kjær\Desktop\barcodeSoundBeep.wav");
                //soundPlayer.Play();
            }
        }
    }
}