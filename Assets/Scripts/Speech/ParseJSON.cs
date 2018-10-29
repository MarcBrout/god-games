using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace GodsGame
{
    public class ParseJSON : MonoBehaviour
    {
        public static ParseJSON Instance;

        string jsonString;
        public TextAsset jsonTextAsset;

        [HideInInspector]
        public Dialog[] dialog;

        [System.Serializable]
        public struct Dialog
        {
            public string Speech;
            public string Action;
            public string Level;
        }

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        void Start()
        {
            ReadData();
        }

        private void ReadData()
        {
            jsonString = jsonTextAsset.text;
            dialog = JsonHelper.getJsonArray<Dialog>(jsonString);
        }

        public class JsonHelper
        {
            public static T[] getJsonArray<T>(string json)
            {
                string newJson = "{ \"array\": " + json + "}";
                Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
                return wrapper.array;
            }

            [System.Serializable]
            private class Wrapper<T>
            {
                public T[] array;
            }
        }
    }
}

