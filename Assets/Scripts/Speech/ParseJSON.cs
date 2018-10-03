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

        [System.Serializable]
        public struct Dialog
        {
            public string Speech;
            public string Action;
            public string Level;
        }

        void awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        void Start()
        {
            ReadData();
        }

        private void ReadData()
        {
            jsonString = jsonTextAsset.text;
            Dialog[] speeches = JsonHelper.getJsonArray<Dialog>(jsonString);
            foreach (Dialog a in speeches)
            {
                Debug.Log(a.Speech);
                Debug.Log(a.Action);
                Debug.Log(a.Level);
            }
        }
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