using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Speech.Recognition;

namespace Jarvis.Core
{
    class Engine
    {
        public static List<IJarvisPlugin> _plugins;

        public static void LoadPlugins()
        {
            //Get all assemblies in the Plugins folder
            List<Assembly> assemblies = LoadAssemblies();

            //Load the plugins
            _plugins = GetPlugIns(assemblies);
        }

        private static List<Assembly> LoadAssemblies()
        {
            //Look in the plugin dir for .dlls
            DirectoryInfo dInfo = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Plugins"));
            FileInfo[] files = dInfo.GetFiles("*.dll");
            List<Assembly> plugInAssemblyList = new List<Assembly>();

            if ( null != files)
                foreach (FileInfo file in files)
                    plugInAssemblyList.Add(Assembly.LoadFile(file.FullName));
            
            return plugInAssemblyList;
        }

        private static List<IJarvisPlugin> GetPlugIns(List<Assembly> assemblies)
        {
            List<Type> availableTypes = new List<Type>();

            foreach (Assembly currentAssembly in assemblies)
                availableTypes.AddRange(currentAssembly.GetTypes());

            // get a list of objects that implement the IJarvisPlugin interface AND have the JarvisPluginAttribute

            List<Type> pluginList = availableTypes.FindAll(delegate(Type t)
            {
                List<Type> interfaceTypes = new List<Type>(t.GetInterfaces());
                object[] arr = t.GetCustomAttributes(typeof(JarvisPluginAttribute), true);
                return !(arr == null || arr.Length == 0) && interfaceTypes.Contains(typeof(IJarvisPlugin));
            });

            // convert the list of Objects to an instatiated list of IJarvisPlugins

            return pluginList.ConvertAll<IJarvisPlugin>(delegate(Type t) { return Activator.CreateInstance(t) as IJarvisPlugin;});

        }

        public static void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            foreach (IJarvisPlugin plugin in _plugins)
                if (e.Result.Grammar.Name == plugin.getGrammarName())
                    plugin.handleSpeechInput(e);
        }

        public static SpeechRecognitionEngine InitializeSRE()
        {
            //Create the speech recognition engine
            SpeechRecognitionEngine sre = new SpeechRecognitionEngine();
            using (sre)
            {

                //Set the audio device to the OS default
                sre.SetInputToDefaultAudioDevice();

                // Reset the Grammar
                sre.UnloadAllGrammars();

                // Load the plugins
                LoadPlugins();

                //Load all of the grammars
                foreach (IJarvisPlugin plugin in _plugins)
                    sre.LoadGrammar(plugin.getGrammar());

                //Set the recognition mode

                sre.RecognizeAsync(RecognizeMode.Multiple);

                //Add an event Handler
                sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Engine.SpeechRecognized);
                while (!Jarvis.JarvisMain.stop)
                {
                }
            }
            return sre;
        }

    }
}
