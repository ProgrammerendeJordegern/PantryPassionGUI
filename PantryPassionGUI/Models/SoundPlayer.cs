using System;
using System.IO;
using System.Media;
using System.Reflection;
using System.Threading;

namespace PantryPassionGUI.Models
{
    public class SoundPlayer : ISoundPlayer
    {
        public System.Media.SoundPlayer _soundPlayer { get; private set; }
        string _path = Environment.CurrentDirectory + @"\barcodeSoundBeep.wav";
        public SoundPlayer()
        {
            _soundPlayer = new System.Media.SoundPlayer(_path);
            Mute = false;
        }

        public bool Mute { get; set; }

        public void Play()
        {
            if (Mute == false)
            {
                _soundPlayer.Play();
            }
        }
    }
}