using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SongDatabaseBuilder
{
    public interface ISongLoc
    {
        string Name { get; set; }
        string Location { get; set; }
    }
}
