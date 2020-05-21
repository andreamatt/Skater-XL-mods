using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLShredLib;

namespace MapBrowser
{
    public class Window : MonoBehaviour
    {

        #region GUI content
        private Texture2D paypalTexture;
        #endregion

        #region GUI style
        #endregion

        #region GUI status
        private bool showUI = false;
        private GameObject master;
        //private GameObject canvasGO;
        //private Canvas canvas;
        private bool setUp;
        #endregion

        public void Update() {
            if (Input.GetKeyUp(KeyCode.M)) {
                if (showUI == false) {
                    Open();
                }
                else {
                    Close();
                }
            }
        }

        private void Open() {
            showUI = true;
            //multiplayerMenu.SetActive(true);
            ModMenu.Instance.ShowCursor(Main.modId);
        }

        private void Close() {
            showUI = false;
            //multiplayerMenu.SetActive(false);
            ModMenu.Instance.HideCursor(Main.modId);
            Main.Save();
        }

        private void SetUp() {
            if (master == null) {
                master = GameObject.Find("New Master Prefab");
                if (master != null) {
                    UnityEngine.Object.DontDestroyOnLoad(master);
                }
            }

            var eventSystem = WindowHelper.GetEventSystem();

            Logger.Log("Starting ui setup");


            var canvasGO = new GameObject();
            var canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvasGO.AddComponent<RectTransform>();
            var graphicRaycaster = canvasGO.AddComponent<GraphicRaycaster>();
            var canvasScaler = canvasGO.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920, 1080);
            canvasScaler.referencePixelsPerUnit = 100;
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            canvasScaler.matchWidthOrHeight = 0.5f;

            //var gridGO = new GameObject();
            //gridGO.transform.SetParent(canvasGO.transform, false);
            //gridGO.EditRectTransform(new Vector2(0.0f, 0.5f), new Vector2(0.2f, 0.4f), new Vector2(0.8f, 0.6f), new Vector2(0.5f, 0.5f));
            //var grid = gridGO.AddComponent<GridLayoutGroup>();
            //grid.cellSize = new Vector2(100, 100);
            //for (var i = 0; i < 20M; i++) {
            //    var title = WindowHelper.Text(gridGO, "AA", Color.black, 35);
            //}

            /////////////////////////////////////////////////
            var textGO = WindowHelper.Text(canvasGO, "Title", Color.blue, 35);
            textGO.EditRectTransform(new Vector2(0.5f, 0.9f), new Vector2(0.5f, 1f));

            /////////////////////////////////////////////////
            var scrollViewGO = new GameObject();
            scrollViewGO.transform.SetParent(canvasGO.transform, false);
            scrollViewGO.EditRectTransform(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
            var scrollBackground = scrollViewGO.AddComponent<Image>();
            scrollBackground.color = Color.red;
            ///////////////////////////////////////////////////
            var viewPortGO = new GameObject();
            viewPortGO.transform.SetParent(scrollViewGO.transform, false);
            var viewPortMask = viewPortGO.AddComponent<Mask>();
            var viewPortMaskImage = viewPortGO.AddComponent<Image>();
            viewPortMaskImage.color = Color.grey;
            viewPortGO.AddComponent<RectTransform>();
            ///////////////////////////////////////////////////
            //var gridGO = new GameObject();
            //gridGO.transform.SetParent(viewPortGO.transform, false);
            //gridGO.EditRectTransform(Vector3.zero, new Vector2(0, 0), new Vector2(1, 1), new Vector2(0.5f, 0.5f), 0, 0);
            //var gridLayout = gridGO.AddComponent<GridLayoutGroup>();
            //gridLayout.cellSize = new Vector2(100, 100);

            //var contentSizeFitter = gridGO.AddComponent<ContentSizeFitter>();
            //contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            //contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            ///////////////
            //scrollViewGO.GetComponent<ScrollRect>().content = gridGO.GetComponent<RectTransform>();
            /////////////////////////////////////////////////
            //for (int i = 0; i < 10; i++) {
            //    var buttonGO = new GameObject();
            //    buttonGO.transform.SetParent(gridGO.transform, false);
            //    buttonGO.AddComponent<RectTransform>();
            //    var image = buttonGO.AddComponent<Image>();
            //    image.color = UnityEngine.Random.ColorHSV();
            //    var button = buttonGO.AddComponent<Button>();
            //    button.interactable = true;
            //    button.targetGraphic = image;
            //    button.onClick.AddListener(() => Logger.Log($"Pressed button {i}"));
            //}







            ////////
            /// OLD WORKING CODE
            ////////
            //var canvasGO = new GameObject();
            //var canvas = canvasGO.AddComponent<Canvas>();
            //canvas.renderMode = RenderMode.ScreenSpaceCamera;
            //canvasGO.AddComponent<GraphicRaycaster>();

            //var scaler = canvasGO.AddComponent<CanvasScaler>();
            //scaler.scaleFactor = 10f;
            //scaler.referenceResolution = new Vector2(1920, 1080);
            //scaler.referencePixelsPerUnit = 100;
            //scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;






            Logger.Log("End ui setup");
        }

        public void OnGUI() {
            if (!setUp) {
                setUp = true;
                //SetUp();
            }

            if (showUI) {
                //windowRect = GUILayout.Window(GUIUtility.GetControlID(FocusType.Passive), windowRect, RenderWindow, "Map browser", windowStyle, GUILayout.Width(400));
            }
        }

        void RenderWindow(int windowID) {

        }
    }
}