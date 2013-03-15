using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SongDatabaseBuilder
{
    public class SongLoc
    {
        private string _name = null;
        private string _location = null;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }
    }
}
