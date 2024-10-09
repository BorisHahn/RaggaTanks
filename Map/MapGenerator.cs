﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaggaTanks.map
{
    public class MapGenerator
    {
        private Dictionary<string, string[]> _mapsCollection = new();
        private string _dirName = "../../../map/mapFiles";
        public Dictionary<string, string[]> MapsCollection { get { return _mapsCollection; } }
        public MapGenerator()
        {
            InitLoadMapsFromStorage();
        }

        private void InitLoadMapsFromStorage()
        {
            var directory = new DirectoryInfo(_dirName);
            if (directory.Exists)
            {
                FileInfo[] files = directory.GetFiles("level*");
                foreach (FileInfo file in files)
                {
                    string[] maps = File.ReadAllLines(file.FullName);
                    AddToMapsCollection(file.Name.Split(".")[0], maps);
                }
            }
        }

        public string[] GetCurrentLevelMap(string levelName)
        {
            return _mapsCollection[levelName];
        }

        private void AddToMapsCollection(string mapKey, string[] mapValue)
        {
            if (!_mapsCollection.ContainsKey(mapKey))
            {
                _mapsCollection.Add(mapKey, mapValue);
            }
        }

        public char GetCurrentCharByCoords(int x, int y, string currLevel)
        {
            string[] curMap = GetCurrentLevelMap(currLevel);
            return curMap[y][x];
        }

        /*public void ReplaceViewMapSection(int x, int y, )
        {

        }*/
    }
}
