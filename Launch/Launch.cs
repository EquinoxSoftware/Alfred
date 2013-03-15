using System;
using Jarvis.Core;
using System.Speech.Recognition;
using System.Diagnostics;


namespace Jarvis.Plugins
{
   [JarvisPluginAttribute("Launcher Plugin", "This plugin handles all of the launching capabilities for jarvis", "1.0")]
    public class Launch : IJarvisPlugin
    {
        private string _grammarName = "LauncherPlugin";

        public Grammar getGrammar()
        {
            // Create a set of choices
            Choices thisChoices = new Choices("Email", "Internet", "Notepad", "Calculator");

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
                case "Email":
                    Process.Start("outlook.exe");
                    break;
                case "Internet":
                    Process.Start("firefox.exe");
                    break;
                case "Notepad":
                    Process.Start("notepad.exe");
                    break;
                case "Calculator":
                    Process.Start("calc.exe");
                    break;
            }
        }

        public string getGrammarName()
        {
            return _grammarName;
        }
    }
}
