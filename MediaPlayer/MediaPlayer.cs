using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jarvis.Core;
using System.Speech.Recognition;
using WMPLib;
using SongDatabaseBuilder;
namespace MediaPlayer
{
    [JarvisPluginAttribute("Media Player Plugin", 
        "This plugin handles the Media Player functions of jarvis", "1.0")]
    public class MediaPlayer : IJarvisPlugin
    {
        WindowsMediaPlayer mPlayer = new WindowsMediaPlayer();
        private string _grammarName = "MediaPlugin";
        public Grammar getGrammar()
        {
            throw new NotImplementedException();
        }

        public void handleSpeechInput(SpeechRecognizedEventArgs e)
        {
            switch (e.Result.Text)
            {
                case "Stop":
                    mPlayer.controls.stop();
                    break;
                case "Pause":
                    mPlayer.controls.pause();
                    break;
            }

            if (e.Result.Text != null)
            {
                mPlayer.URL = e.Result.Text;
                Console.WriteLine(e.Result.Text);
                mPlayer.controls.play();
            }


        }

        public string getGrammarName()
        {
            return _grammarName;
        }

        public void BuildGrammar(SongLocations songs)
        {
            Grammar songsGrammar = null;
            int count = songs.Count;
            string[] phrases = new string[count];
            for (int i = 0; i < count; i++)
            {
                string name = songs[i].Name.Replace('_', ' ');

                phrases.SetValue(name, i);
            }

            Choices choices = new Choices();
            choices.Add(phrases);

            choices.Add("stop, pause");

            GrammarBuilder thisGB = new GrammarBuilder(choices);

            songsGrammar = new Grammar(thisGB);

             
        }
    }
}
