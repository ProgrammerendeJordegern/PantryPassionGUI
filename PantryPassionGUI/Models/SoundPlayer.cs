using System;
using System.IO;
using System.Media;
using System.Reflection;
using System.Threading;
using PantryPassionGUI.Utilities;

namespace PantryPassionGUI.Models
{
    public class SoundPlayer : ISoundPlayer
    {
        public System.Media.SoundPlayer _soundPlayer { get; private set; }
        string _path = Environment.CurrentDirectory + @"\barcodeSoundBeep.wav";
        private IOutput _output;

        public SoundPlayer()
        {
            _soundPlayer = new System.Media.SoundPlayer(_path);
            Mute = false;
            _output = new Output();
        }

        public SoundPlayer(IOutput output)
        {
            _soundPlayer = new System.Media.SoundPlayer(_path);
            Mute = false;
            _output = output;
        }

        public bool Mute { get; set; }

        public void Play()
        {
            if (Mute == false)
            {
                _soundPlayer.Play();
                _output.OutputLine("Sound played");
            }
        }
    }
}