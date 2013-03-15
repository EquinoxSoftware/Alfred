using System;
using System.Speech.Synthesis;

namespace Jarvis.Core
{
   public class Output
    {
        public static void Speak(string input)
        {
            var synth = new SpeechSynthesizer();
            var sayThis = new Prompt(input);
            synth.Speak(sayThis);
        }
    }
}
