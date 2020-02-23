using GameManagement;
using ReplayEditor;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace MapBrowser
{
    public class MapBrowser : MonoBehaviour
    {
        private Window window;
        private FileManager fileManager;

        public void Start() {
            DontDestroyOnLoad(gameObject);

            //window = gameObject.AddComponent<Window>();
            fileManager = gameObject.AddComponent<FileManager>();
        }
    }
}
