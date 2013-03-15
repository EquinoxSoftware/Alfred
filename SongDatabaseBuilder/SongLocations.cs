using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace SongDatabaseBuilder
{
    public class SongLocations
    {
        private IList<SongLoc> _listOfSongs = new List<SongLoc>();
        public SongLoc this[int index]
        {
            get
            {
                return _listOfSongs[index];
            }
            set
            {
                _listOfSongs[index] = value;
            }
        }

        public void Add(SongLoc item)
        {
            _listOfSongs.Add(item);
        }
        public int Count
        {
            get { return _listOfSongs.Count; }
        }
        public IEnumerator<SongLoc> GetEnumerator()
        {
            return _listOfSongs.GetEnumerator();
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string songName]
        {
            get
            {
                foreach (SongLoc song in _listOfSongs)
                {
                    if (song.Name.Contains(songName))
                    {
                        return song.Location;
                    }
                }
                return null;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        

    }
}
