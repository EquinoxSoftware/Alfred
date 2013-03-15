using System;

namespace Jarvis.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class JarvisPluginAttribute : Attribute
    {
        public JarvisPluginAttribute(string name, string description, string version)
        {
            _description = description;
            _name = name;
            _version = version;
        }

        private string _description;
        private string _name;
        private string _version;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }
    }
}
