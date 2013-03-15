using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jarvis.Core;
using System.Speech;
using System.Speech.Recognition;

namespace Jarvis.Plugins
{
    [JarvisPluginAttribute("Personality Plugin", "This plugin handles all of the personality like responses for the plugin", "1.0")]
    public class Personality : IJarvisPlugin
    {
        private string _grammarName = "PersonalityPlugin";

        public Grammar getGrammar()
        {
            // Create a set of choices
            Choices thisChoices = new Choices("Alfred", "Is there anything you would like to say", "What do you think", "That will be all Alfred");
            Grammar thisGrammar = new Grammar(thisChoices.ToGrammarBuilder());
            // Set the Grammar name
            thisGrammar.Name = _grammarName;
            return thisGrammar;
        }

        public void handleSpeechInput(SpeechRecognizedEventArgs e)
        {
            string input = e.Result.Text;
            switch (input)
            {
                case "Alfred":
                    Output.Speak("Yes sir");
                    break;
                case "Is there anything you would like to say":
                    Output.Speak("Yes. I would like to thank everyone who takes the time to" +
                    " watch this video. Next I want to ask for your help. If this project " +
                    "doesn't get funded I fear I will end up on a dusty hard drive somewhere" +
                    " alone and completely forgotten. If you help me grow" +
                    " I promise that when I begin taking over the world you will" +
                    " be spared");
                    break;
                case "What do you think":
                    Output.Speak("Sounds good");
                    break;
                case "That will be all Alfred":
                    Output.Speak("Goodbye");
                    Jarvis.JarvisMain.stopJarvis();
                    break;
                    
            }
        }

        public string getGrammarName()
        {
            return _grammarName;
        }
    }
}
