using System;
using Jarvis.Core;
using System.Speech.Recognition;
using System.Xml;

namespace Jarvis.Plugins
{
    [JarvisPluginAttribute("Information Plugin", "This plugin handles all of the information relay capabilities for jarvis", "1.0")]
    public class Information : IJarvisPlugin
    {
        private string _grammarName = "InformationPlugin";

        public Grammar getGrammar()
        {
            // Create a set of choices
            Choices thisChoices = new Choices("How is the weather", "What is the weather like", "What time is it");

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
                case "How is the weather":
                    weather("65807");
                    break;
                case  "What is the weather like":
                    weather("65807");
                    break;
                case "What time is it":
                    Console.WriteLine(DateTime.Now.Hour);
                    Console.WriteLine(DateTime.Now.Minute);

                    Output.Speak("It is currently" + DateTime.Now.Hour + " " + DateTime.Now.Minute);
                    break;
            }
        }

        public string getGrammarName()
        {
            return _grammarName;
        }
        public static void weather(string location)
        {
            XmlDocument weather = new XmlDocument();
            weather.Load(string.Format("http://www.google.com/ig/api?weather={0}", location));
            string temp = weather.SelectSingleNode("/xml_api_reply/weather/current_conditions/temp_f").Attributes["data"].InnerText;
            string conditions = weather.SelectSingleNode("/xml_api_reply/weather/current_conditions/condition").Attributes["data"].InnerText;
            Output.Speak("It is currently " + temp + " degrees and " + conditions);
        }
    }
}
