using System;
using PantryPassionGUI.Utilities;

namespace PantryPassionGUI.Models
{
    public class SoundPlayer : ISoundPlayer
    {
        //Uses SoundPlayer in System.Media
        public System.Media.SoundPlayer _soundPlayer { get; private set; }

        //Gets the path the sound file is located
        string _path = Environment.CurrentDirectory + @"\barcodeSoundBeep.wav";
        private IOutput _output;

        public SoundPlayer()
        {
            _soundPlayer = new System.Media.SoundPlayer(_path);
            Mute = false;
            _output = new Output();
        }

        //Used to do dependency injection in testing
        public SoundPlayer(IOutput output)
        {
            _soundPlayer = new System.Media.SoundPlayer(_path);
            Mute = false;
            _output = output;
        }

        public bool Mute { get; set; }

        //Plays the sound
        public void Play()
        {
            if (Mute == false)
            {
                _soundPlayer.Play();

                //write line to output, so play can be tested in unit test
                _output.OutputLine("Sound played");
            }
        }
    }
}