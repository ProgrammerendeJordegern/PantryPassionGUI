using System;
using System.IO;
using System.Media;
using System.Reflection;
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

                //System.Media.SystemSounds.Beep.Play();

                string path = Environment.CurrentDirectory + @"\barcodeSoundBeep.wav";

                System.Media.SoundPlayer soundPlayer =
                    new System.Media.SoundPlayer(path);
                soundPlayer.Play();
            }
        }
    }
}