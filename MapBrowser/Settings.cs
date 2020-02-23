using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;
using UnityModManagerNet;

namespace MapBrowser
{
    [Serializable]
    public class Settings : UnityModManager.ModSettings
    {
        public readonly string defaultLoadingPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SkaterXL\\Maps";
        public List<string> loadingPaths = new List<string>();
        public SortBy sortBy = SortBy.Name;
        public SortOrder sortOrder = SortOrder.Ascending;
        public List<ClientMapInfo> installedMaps = new List<ClientMapInfo>();
        public MapIndex mapIndex = new MapIndex();

        public Settings() {

        }

        public override void Save(UnityModManager.ModEntry modEntry) {
            Save();
        }

        public Task Save() {
            return Task.Run(() => {
                var filepath = $"{Main.modEntry.Path}Settings.xml";
                try {
                    using (var writer = new StreamWriter(filepath)) {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
                        xmlSerializer.Serialize(writer, this);
                    }
                }
                catch (Exception e) {
                    Logger.Log($"Can't save {filepath}. ex: {e}");
                }
            });
        }

        public static Settings Load() {
            var filepath = $"{Main.modEntry.Path}Settings.xml";
            Settings settings = null;
            if (File.Exists(filepath)) {
                try {
                    using (var reader = new StreamReader(filepath)) {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
                        settings = (Settings)xmlSerializer.Deserialize(reader);
                    }
                }
                catch (Exception e) {
                    Logger.Log($"Can't read {filepath}. ex: {e}");
                }
            }
            else {
                Logger.Log($"No settings found, using defaults");
                settings = new Settings();
            }
            return settings;
        }
    }
}
