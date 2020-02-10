using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AreYouSure
{
    class AreYouSure : MonoBehaviour
    {
        private Window window;
        public void Start() {
            window = this.gameObject.AddComponent<Window>();
        }
    }
}
