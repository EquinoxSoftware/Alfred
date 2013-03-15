using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jarvis.Core;
using System.Speech.Recognition;
using System.Timers;
using System.Threading;
using System.Xml;
using WMPLib;

namespace Jarvis.Plugins
{
    [JarvisPluginAttribute("Alarm Clock Plugin", "This plugin handles the alarm clock functionality of jarvis", "1.0")]
    public class AlarmClock : IJarvisPlugin
    {
        private string _grammarName = "AlarmPlugin";
        private const int MinSleep = 250;
        private static DateTime alarmTime;
        public Grammar getGrammar()
        {
            Choices setChoices = new Choices("I'd like to set an alarm for", "Set alarm for", "Set Alarm Clock", "Set Alarm");
            GrammarBuilder morningChoices = new Choices("am", "in the morning");
            morningChoices.Append(new SemanticResultValue(true));
            GrammarBuilder eveningChoices = new Choices("pm", "tonight", "at night");
            eveningChoices.Append(new SemanticResultValue(false));
            Choices amOrPm = new Choices(morningChoices, eveningChoices);
            SemanticResultKey amOrPmKey = new SemanticResultKey("AmPm", amOrPm);
            Choices hours = new Choices("1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12");
            Choices minutes = new Choices("10", "15", "20", "25", "30", "35", "40", "45", "50", "55", "60", "o'clock", " ");
            GrammarBuilder thisGB = new GrammarBuilder();
            thisGB.Append(setChoices);
            thisGB.Append(new SemanticResultKey("Hours", hours));
            thisGB.Append(new SemanticResultKey("Minutes", minutes));
            thisGB.Append(amOrPmKey);
            Grammar thisGrammar = new Grammar(thisGB);
            // Set the Grammar name
            thisGrammar.Name = _grammarName;
            return thisGrammar;
        }

        public void handleSpeechInput(SpeechRecognizedEventArgs e)
        {
            Console.WriteLine(e.Result.Text);
           setAlarm(e.Result.Semantics["Hours"].Value.ToString(),e.Result.Semantics["Minutes"].Value.ToString(), (bool)e.Result.Semantics["AmPm"].Value);
        }

        public string getGrammarName()
        {
            return _grammarName;
        }

        private static bool setAlarm(string hours, string minutes, bool AmOrPm)
        {

            if (minutes.Equals("o'clock") || minutes.Equals(""))
                alarmTime = getAlarmTime(int.Parse(hours), 0, AmOrPm);
            else
                alarmTime = getAlarmTime(int.Parse(hours), int.Parse(minutes), AmOrPm);

            new Thread(new ThreadStart(alarmRunning)).Start();
            if (!alarmTime.Equals(DateTime.Now))
                return true;
            else
                return false;
        }

        private static DateTime getAlarmTime(int hours, int minutes, bool amPm)
        {
            alarmTime = new DateTime();
            if(amPm && hours == 12)
                hours = 0;

            int offSetHourAM = hours - DateTime.Now.Hour;
            int offSetHourPM = (hours + 12) - DateTime.Now.Hour;
            int offSetMin = minutes - DateTime.Now.Minute;
            Console.WriteLine(offSetHourAM);

            if (amPm)
            {
                if (offSetHourAM > 0)
                    alarmTime = DateTime.Now.AddHours(offSetHourAM).AddMinutes(offSetMin).AddSeconds(0 - DateTime.Now.Second);
                else if (offSetHourAM == 0 && offSetMin == 0)
                    alarmTime = DateTime.Now.AddHours(24).AddMinutes(offSetMin).AddSeconds(0 - DateTime.Now.Second);
                else if (offSetHourAM == 0 && offSetMin > 0)
                    alarmTime = DateTime.Now.AddHours(offSetHourAM).AddMinutes(offSetMin).AddSeconds(0 - DateTime.Now.Second);
                else
                    alarmTime = DateTime.Now.AddHours(offSetHourAM).AddMinutes(offSetMin).AddDays(1).AddSeconds(0 - DateTime.Now.Second);
            }
            else
                if (offSetHourPM >= 0)
                    alarmTime = DateTime.Now.AddHours(offSetHourPM).AddMinutes(offSetMin).AddSeconds(0 - DateTime.Now.Second);
                else if (offSetMin == 0)
                    alarmTime = DateTime.Now.AddHours(24).AddMinutes(offSetMin).AddSeconds(0 - DateTime.Now.Second);
                else if (offSetHourPM == 0 && offSetMin > 0)
                    alarmTime = DateTime.Now.AddHours(offSetHourAM).AddMinutes(offSetMin).AddSeconds(0 - DateTime.Now.Second);
                else
                    alarmTime = DateTime.Now.AddHours(offSetHourPM).AddMinutes(offSetMin).AddDays(1).AddSeconds(0 - DateTime.Now.Second);
            Console.WriteLine(alarmTime);
            return alarmTime;
        }

        private static void alarmRunning()
        {
            DateTime Now = DateTime.Now;

            while (Now < alarmTime)
            {
                int SleepMillisecs = (int)Math.Round((alarmTime - Now).TotalMilliseconds / 2);
                Console.WriteLine(SleepMillisecs);
                Thread.Sleep(SleepMillisecs > MinSleep ? SleepMillisecs : MinSleep);
                Now = DateTime.Now;
            }

            Output.Speak("The current time is " + DateTime.Now.Hour + DateTime.Now.Minute);
            weather("65807");
            WindowsMediaPlayer player = new WindowsMediaPlayer();
            string file = "F:\\Music\\City Of Evil\\01 Beast And The Harlot.mp3";
            player.URL = file;
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
