using System;
using Jarvis.Core;
namespace Jarvis
{
    public class JarvisMain
    {
        public static bool stop = false;

        static void Main(string[] args)
        {
            Engine.InitializeSRE();
        }

        public static void stopJarvis()
        {
            stop = true;
        }
    }
}
