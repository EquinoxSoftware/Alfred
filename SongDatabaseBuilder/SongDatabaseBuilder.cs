using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace SongDatabaseBuilder
{
    public class SongDatabaseBuilder
    {
        public static void BuildDatabase(ref SongLocations songs)
        {
            XDocument SongDB = new XDocument();
            SongDB.Declaration = new XDeclaration("1.0", UTF8Encoding.ASCII.ToString(), "yes");
            XElement songsElem = new XElement("Songs");
            SongDB.Add(songsElem);
            // Find Drives
            DriveInfo[] drives =  DriveInfo.GetDrives();

            foreach (DriveInfo driveInfo in drives)
            {
                if(driveInfo.DriveType.Equals(DriveType.Fixed))
                    FindSongs(driveInfo.Name, ref songs);
            }

            foreach (SongLoc song in songs)
            {
                string name = song.Name;
                RemoveNoise(ref name);
                if (name.Equals(""))
                    name = "Unknown";
                name = name.Replace('-', ' ');
                name = name.Replace('_', ' ');
                char[] separator = new char[] { '[', ']', '{', '}' };
                string[] tokens = name.Split(separator);

                if (tokens.Count() > 0)
                {
                    foreach (string str in tokens)
                    {
                        string temp = str.Trim();
                        if (!temp.Equals(""))
                            song.Name = temp;
                    }
                }
                else
                {
                    song.Name = name;
                }

                CreateSongStructure(ref songsElem, song);
            }
            SongDB.Save("SongDB.xml");
        }

        private static void CreateSongStructure(ref XElement rootElement, SongLoc songLocation)
        {
            try
            {
                string name = songLocation.Name;
                XElement songElement = new XElement("song", new XAttribute("Name", songLocation.Name));

                songElement.Value = songLocation.Location;
                rootElement.Add(songElement);
               
            }
            catch(Exception ex)
            {
                Console.WriteLine("Songs not added - " + songLocation.Name);
            }
        }

        private static void RemoveNoise (ref string song)
        {
            song = song.Replace('0', ' ');
            song = song.Replace('1', ' ');
            song = song.Replace('2', ' ');
            song = song.Replace('3', ' ');
            song = song.Replace('4', ' ');
            song = song.Replace('5', ' ');
            song = song.Replace('6', ' ');
            song = song.Replace('7', ' ');
            song = song.Replace('8', ' ');
            song = song.Replace('9', ' ');

            song = song.Replace('>', ' ');
            song = song.Replace('<', ' ');

            song = song.Replace(',', ' ');

            song = song.Trim();

        }
        private static void FindSongs(string path, ref SongLocations songs)
        {
            if (Directory.Exists(path))
            {
                string[] files = null;
                try
                {
                    files = Directory.GetFiles(path);
                }
                catch(UnauthorizedAccessException ex)
                {
                }
                if (files != null)
                {
                    foreach (string file in files)
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        if ((fileInfo.Extension.Equals(".mp3")) || (fileInfo.Extension.Equals(".MP3")))
                        {
                            TagLib.File tagFile = null;

                            SongLoc song = new SongLoc();
                            try
                            {
                                tagFile = new TagLib.Aac.File(file);
                            }
                            catch { }

                            string songName = null;
                            if (tagFile != null)
                                songName = tagFile.Tag.Title;
                            else
                                songName = fileInfo.Name.Replace('_', ' ').Substring(0, fileInfo.Name.IndexOf('.'));

                            song.Location = fileInfo.FullName;
                            if (songName == null)
                                continue;
                            song.Name = songName;
                            songs.Add(song);
                        }
                    }
                }
                string[] directories = null;
                try
                {
                    directories = Directory.GetDirectories(path);
                }
                catch (UnauthorizedAccessException ex) { }
                if (directories != null)
                {
                    foreach (string directory in directories)
                    {
                        FindSongs(directory, ref songs);
                    }
                }
            }

        }
    }
}
