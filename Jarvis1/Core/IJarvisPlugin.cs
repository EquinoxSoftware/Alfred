using System;
using System.Speech.Recognition;

namespace Jarvis.Core
{
    public interface IJarvisPlugin
    {
        Grammar getGrammar();

        void handleSpeechInput(SpeechRecognizedEventArgs e);

        string getGrammarName();
    }
}
