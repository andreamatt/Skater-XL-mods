using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapBrowser
{
    [Serializable]
    public class MapIndex
    {
        public string saveTime = DateTime.Now.ToString();
        public List<MapInfo> maps = new List<MapInfo>();
    }

    [Serializable]
    public class MapInfo
    {
        public string mapName;
        public string author;
        public string fileName;
        public int size;    // in MB

        // format must be dd/mm/yyyy ? or yyyy/mm/dd
        private DateTime release;
        public string releaseDate {
            get => release.ToString("dd/MM/yyyy");
            set => release = DateTime.Parse(value);
        }
        public DateTime GetReleaseDate() => release;
    }

    [Serializable]
    public class ClientMapInfo : MapInfo
    {
        public bool favourite;
        public string filePath;

        private DateTime download;
        public string downloadDate {
            get => download.ToString("dd/MM/yyyy hh:mm");
            set => download = DateTime.Parse(value);
        }
        public DateTime GetDownloadDate() => download;

        private DateTime lastPlay;
        public string lastPlayed {
            get => lastPlay.ToString("dd/MM/yyyy hh:mm");
            set => lastPlay = DateTime.Parse(value);
        }
        public DateTime GetLastPlayDate() => lastPlay;
    }

    public class ImageInfo
    {
        public string name;
        public string download_url;
    }
}
