using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyJson;
using UnityEngine;
using UnityEngine.Networking;

namespace MapBrowser
{
    class FileManager : MonoBehaviour
    {
        // filename to list of previews
        private Dictionary<string, List<Texture2D>> previews = new Dictionary<string, List<Texture2D>>();
        private Dictionary<string, int> previewIndex = new Dictionary<string, int>();

        public IEnumerator Start() {
            Logger.Log("Starting filemanager, imageDir: " + Main.ImageFolder);
            // settings are already loaded
            // get map files list from default path
            var mapPaths = Directory.GetFiles(Main.settings.defaultLoadingPath).ToList();
            // get all other paths
            foreach (var path in Main.settings.loadingPaths) {
                var filePaths = Directory.GetFiles(path);
                mapPaths.AddRange(filePaths);
            }
            // key=name, value=path
            var localMaps = mapPaths.ToDictionary(path => Path.GetFileName(path), path => path);


            // check for duplicates?? using hash??

            // get image files list
            // if image dir doesn't exist, create it
            if (!Directory.Exists(Main.ImageFolder)) {
                Directory.CreateDirectory(Main.ImageFolder);
            }
            var imgPaths = Directory.GetFiles(Main.ImageFolder);
            var images = new Dictionary<string, Texture2D>();
            foreach (var path in imgPaths) {
                var fileName = Path.GetFileName(path);
                var texture = new Texture2D(1, 1);
                texture.LoadImage(File.ReadAllBytes(path));
                images[fileName] = texture;
            }
            foreach (var i in images.Keys) {
                Logger.Log("localImage: " + i);
            }

            // delete mapinfos if file is missing (keep only mapinfos where map is present)
            Main.settings.installedMaps = Main.settings.installedMaps.Where(m => localMaps.Keys.Contains(m.fileName)).ToList();


            // download latest json and cache it
            string text = null;
            yield return StartCoroutine(GetURL("https://raw.githubusercontent.com/andreamatt/Skater-XL-maps/master/mapIndex.json", result => text = result));
            Logger.Log("TEXT: " + text);
            var newIndex = text.FromJson<MapIndex>();
            Main.settings.mapIndex = newIndex;  // save new index


            // get image index
            string imagesJson = null;
            yield return StartCoroutine(GetURL("https://api.github.com/repos/andreamatt/skater-xl-maps/contents/Images", result => imagesJson = result));
            var imageInfoList = imagesJson.FromJson<List<ImageInfo>>();

            foreach (var ii in imageInfoList) {
                Logger.Log("Image: " + ii.name);
            }

            // if there are new previews download them
            // preview names are "{map file name}_*.jpg"
            var saveImage = new Action<string, Texture2D>((name, tex) => images[name] = tex);
            foreach (var ii in imageInfoList) {
                // if not in local, download it
                if (!images.ContainsKey(ii.name)) {
                    // download them in parallel, maybe with coroutine, use Action to save the downloaded image in the previews dictionary (use lock for thread-safety) (load other images before this?)
                    yield return StartCoroutine(DownloadImage(ii, saveImage));
                }
            }


            // if there are new files without mapinfos, create mapinfos
            // using old index vs new index vs local map names
            var localMapInfoNames = Main.settings.installedMaps.Select(mi => mi.fileName);
            foreach (var localMap in localMaps) {
                var fileName = localMap.Key;
                var filePath = localMap.Value;
                // if localmapinfo is not present
                if (!localMapInfoNames.Contains(fileName)) {
                    // always create badinfo and update it asap
                    var clientMapInfo = new ClientMapInfo() {
                        completeInfo = false,
                        favourite = false,
                        filePath = filePath,
                        downloadDate = File.GetLastWriteTime(filePath).ToString(),
                        lastPlayed = DateTime.MinValue.ToString()
                    };
                    Main.settings.installedMaps.Add(clientMapInfo);
                }
            }

            // update local mapinfos that need online info (just update all of them)
            foreach (var cmi in Main.settings.installedMaps) {
                var indexInfo = Main.settings.mapIndex.maps.FirstOrDefault(m => m.fileName == cmi.fileName);
                if (indexInfo != null) {
                    cmi.completeInfo = true;
                    cmi.UpdateBaseInfo(indexInfo);
                }
            }

            // update previews dictionary
            // add keys from localMaps and mapIndex
            foreach (var localMap in Main.settings.installedMaps) {
                previewIndex[localMap.fileName] = 0;
                if (!previews.ContainsKey(localMap.fileName)) {
                    previews[localMap.fileName] = new List<Texture2D>();
                }
            }
            foreach (var map in Main.settings.mapIndex.maps) {
                previewIndex[map.fileName] = 0;
                if (!previews.ContainsKey(map.fileName)) {
                    previews[map.fileName] = new List<Texture2D>();
                }
            }

            Logger.Log("Started filemanager");
        }

        private IEnumerator GetURL(string url, Action<string> resultHandler) {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url)) {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError) {
                    Logger.Log("Can't download " + url + ": " + webRequest.error);
                }
                else {
                    var text = webRequest.downloadHandler.text;
                    resultHandler(text);
                }
            }
        }

        private IEnumerator DownloadImage(ImageInfo ii, Action<string, Texture2D> resultHandler) {
            var url = ii.download_url;
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url)) {
                var downloadHandler = new DownloadHandlerTexture();
                webRequest.downloadHandler = downloadHandler;
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError) {
                    Logger.Log("Can't download " + url + ": " + webRequest.error);
                }
                else {
                    var bytes = downloadHandler.data;
                    Logger.Log("Length: " + bytes.Length);
                    File.WriteAllBytes(Main.ImageFolder + ii.name, bytes);

                    var texture = downloadHandler.texture;
                    resultHandler(ii.name, texture);
                }
            }
        }
    }

    public enum SortBy
    {
        ReleaseDate,
        DownloadDate,
        Name,
        Size,
        LastPlayed
    }

    public enum SortOrder
    {
        Ascending,
        Descending
    }
}
