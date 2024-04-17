using RoR2.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RoRModNET4
{
    public class Loader : MonoBehaviour
    {
        public static void Init()
        {
            Loader.Load = new GameObject();
            Loader.Load.AddComponent<RoRMod>();
            UnityEngine.Object.DontDestroyOnLoad(Loader.Load);
        }

        public static void Unload()
        {
            _Unload();
        }

        private static void _Unload()
        {
            GameObject.Destroy(Load);
        }

        private static GameObject Load;
    }
}
