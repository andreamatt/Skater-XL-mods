using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MapBrowser
{
    static class WindowHelper
    {
        private static Font defaultFont;
        private static EventSystem eventSystem;

        public static EventSystem GetEventSystem() {
            if (eventSystem == null) {
                var es = UnityEngine.Object.FindObjectOfType<EventSystem>();
                if (es == null) {
                    GameObject gameObject = new GameObject("Event System");
                    eventSystem = gameObject.AddComponent<EventSystem>();
                    gameObject.AddComponent<StandaloneInputModule>();
                }
                else {
                    eventSystem = es;
                }
            }
            return eventSystem;
        }

        public static GameObject Text(GameObject parent, string content, Color color, int fontSize, TextAnchor alignment = TextAnchor.MiddleCenter) {
            if (defaultFont == null) {
                defaultFont = Resources.FindObjectsOfTypeAll<Font>()[0]; // other fonts?!
            }

            var go = new GameObject();
            go.transform.SetParent(parent.transform, false);

            var text = go.AddComponent<Text>();
            text.supportRichText = true;
            text.text = content;
            //text.rectTransform.sizeDelta = Vector2.zero; useless
            //text.rectTransform.anchorMin = new Vector2(0f, 0.9f);
            //text.rectTransform.anchorMax = new Vector2(0.3f, 1f);
            //text.rectTransform.anchoredPosition = new Vector2(0.5f, 0.5f);

            go.EditRectTransform(new Vector2(0, 0), new Vector2(1, 1));

            text.color = color;
            text.font = defaultFont;
            text.fontSize = fontSize;
            text.alignment = alignment;

            return go;
        }

        public static RectTransform EditRectTransform(this GameObject GO, Vector2 Amin, Vector2 Amax) {
            var rectT = GO.GetComponent<RectTransform>();
            if (rectT == null) {
                rectT = GO.AddComponent<RectTransform>();
                rectT.anchoredPosition = Vector2.zero; // position (IN PIXELS) of pivot relative to anchor
                rectT.pivot = new Vector2(0.5f, 0.5f);    // object point of reference? for rotation and location
            }
            rectT.anchorMin = Amin; // percentage of parent, indicate where to place anchors
            rectT.anchorMax = Amax;
            return rectT;
        }

        public static RectTransform EditRectTransform(this GameObject GO, Vector2 Amin, Vector2 Amax, Vector2 pos, Vector2 pivot) {
            var rectT = GO.EditRectTransform(Amin, Amax);
            rectT.anchoredPosition = pos; // position (IN PIXELS) of pivot relative to anchor
            rectT.pivot = pivot;    // object point of reference? for rotation and location
            var r = rectT.rect;
            return rectT;
        }
        public static RectTransform EditRectTransform(this GameObject GO, Vector2 Amin, Vector2 Amax, int w, int h) {
            var rectT = GO.EditRectTransform(Amin, Amax);
            var r = rectT.rect;
            r.width = w;
            r.height = h;
            return rectT;
        }

        public static RectTransform EditRectTransform(this GameObject GO, Vector2 Amin, Vector2 Amax, Vector2 pos, Vector2 pivot, int w, int h) {
            GO.EditRectTransform(Amin, Amax, pos, pivot);
            return GO.EditRectTransform(Amin, Amax, w, h);
        }

    }
}
