using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SongDatabaseBuilder
{
    public interface ISongLocations : IList<ISongLocations>
    {
        string this[string songName] { get; set; }
        void Add(ISongLoc song);
    }
}
