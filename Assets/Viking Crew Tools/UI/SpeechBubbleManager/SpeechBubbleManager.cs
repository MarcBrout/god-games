using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace VikingCrewTools.UI {
    public class SpeechBubbleManager : MonoBehaviour
    {
        public enum SpeechbubbleType
        {
            NORMAL,
            SERIOUS,
            ANGRY,
            THINKING,
        }
        [System.Serializable]
        public class SpeechbubblePrefab
        {
            public SpeechbubbleType type;
            public GameObject prefab;
        }

        [Header("Default settings:")]
        [FormerlySerializedAs("defaultColor")]
        [SerializeField]
        private Color _defaultColor = Color.white;

        [FormerlySerializedAs("defaultTimeToLive")]
        [SerializeField]
        private float _defaultTimeToLive = 1;

        [FormerlySerializedAs("is2D")]
        [SerializeField]
        private bool _is2D = true;

        [Tooltip("If you want to change the size of your speechbubbles in a scene without having to change the prefabs then change this value")]
        [FormerlySerializedAs("sizeMultiplier")]
        [SerializeField]
        private float _sizeMultiplier = 1f;

        [Tooltip("If you want to use different managers, for example if you want to have one manager for allies and one for enemies in order to style their speech bubbles differently set this to false. Note that you will need to keep track of a reference some other way in that case.")]
        [SerializeField]
        private bool _isSingleton = true;

        [Header("Prefabs mapping to each type:")]
        [FormerlySerializedAs("prefabs")]
        [SerializeField]
        private List<SpeechbubblePrefab> _prefabs;

        private Dictionary<SpeechbubbleType, GameObject> _prefabsDict = new Dictionary<SpeechbubbleType, GameObject>();
        private Dictionary<SpeechbubbleType, Queue<SpeechBubbleBehaviour>> _speechBubbleQueueDict = new Dictionary<SpeechbubbleType, Queue<SpeechBubbleBehaviour>>();

        [SerializeField]
        [Tooltip("Will use main camera if left as null")]
        private Camera _cam;

        private static SpeechBubbleManager _instance;
        public static SpeechBubbleManager Instance {
            get {
                UnityEngine.Assertions.Assert.IsNotNull(_instance, "The static variable for Instance has not been set. Did you do this call before Awake() has finished or unchecked \"Is Singleton\" maybe?");
                return _instance;
            }
        }

        public Camera Cam
        {
            get
            {
                return _cam;
            }

            set
            {
                _cam = value;
                foreach (var bubbleQueue in _speechBubbleQueueDict.Values)
                {
                    foreach (var bubble in bubbleQueue)
                    {
                        bubble.Cam = _cam;
                    }
                }
            }
        }

        protected void Awake()
        {
            if (_cam == null) _cam = Camera.main;

            if (_isSingleton) {
                UnityEngine.Assertions.Assert.IsNull(_instance, "_intance was not null. Do you maybe have several Speech Bubble Managers in your scene, all trying to be singletons?");
                _instance = this;
            }
            _prefabsDict.Clear();
            _speechBubbleQueueDict.Clear();
            foreach (var prefab in _prefabs)
            {
                _prefabsDict.Add(prefab.type, prefab.prefab);
                _speechBubbleQueueDict.Add(prefab.type, new Queue<SpeechBubbleBehaviour>());
            }
        }
        
        private IEnumerator DelaySpeechBubble(float delay, Transform objectToFollow, string text, SpeechbubbleType type, float timeToLive, Color color, Vector3 offset)
        {
            yield return new WaitForSeconds(delay);
            if (objectToFollow)
                AddSpeechBubble(objectToFollow, text, type, timeToLive, color, offset);
        }

        /// <summary>
        /// Adds a speechbubble to a certain position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="text"></param>
        /// <param name="type"></param>
        /// <param name="timeToLive"></param>
        /// <param name="color"></param>
        public SpeechBubbleBehaviour AddSpeechBubble(Vector3 position, string text, SpeechbubbleType type = SpeechbubbleType.NORMAL, float timeToLive = 0, Color color = default(Color))
        {
            if (timeToLive == 0) timeToLive = _defaultTimeToLive;
            if (color == default(Color)) color = _defaultColor;
            SpeechBubbleBehaviour bubbleBehaviour = GetBubble(type);
            bubbleBehaviour.Setup(position, text, timeToLive, color, Cam);
            _speechBubbleQueueDict[type].Enqueue(bubbleBehaviour);
            return bubbleBehaviour;
        }

        /// <summary>
        /// Adds a speechbubble that will follow a certain transform.
        /// It is recommended you use a character's head or mouth transform.
        /// </summary>
        /// <param name="objectToFollow"></param>
        /// <param name="text"></param>
        /// <param name="type"></param>
        /// <param name="timeToLive">if 0 then will use default time to live</param>
        /// <param name="color">Color to tint, default will be white</param>
        /// <param name="offset">Offset from objectToFollow</param>
        public SpeechBubbleBehaviour AddSpeechBubble(Transform objectToFollow, string text, SpeechbubbleType type = SpeechbubbleType.NORMAL, float timeToLive = 0, Color color = default(Color), Vector3 offset = new Vector3())
        {
            if (timeToLive == 0) timeToLive = _defaultTimeToLive;
            if (color == default(Color)) color = _defaultColor;
            SpeechBubbleBehaviour bubbleBehaviour = GetBubble(type);
            bubbleBehaviour.Setup(objectToFollow, offset, text, timeToLive, color, Cam);
            _speechBubbleQueueDict[type].Enqueue(bubbleBehaviour);
            return bubbleBehaviour;
        }

        /// <summary>
        /// Adds a speechbubble that will follow a certain transform.
        /// It is recommended you use a character's head or mouth transform.
        /// 
        /// The speech bubble will be delayed and will only show up after the delay, making it possiblew to add a whole monologue or conversation between characters at once.
        /// If objectToFollow should be destroyed then no speech bubble will show up.
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="objectToFollow"></param>
        /// <param name="text"></param>
        /// <param name="type"></param>
        /// <param name="timeToLive"></param>
        /// <param name="color"></param>
        /// <param name="offset"></param>
        public void AddDelayedSpeechBubble(float delay, Transform objectToFollow, string text, SpeechbubbleType type = SpeechbubbleType.NORMAL, float timeToLive = 0, Color color = default(Color), Vector3 offset = new Vector3())
        {
            StartCoroutine(DelaySpeechBubble(delay, objectToFollow, text, type, timeToLive, color, offset));
        }
        
        /// <summary>
        /// Gets a reused speechbubble from the queue or, if no free ones exist already, creates
        /// a new one.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private SpeechBubbleBehaviour GetBubble(SpeechbubbleType type = SpeechbubbleType.NORMAL)
        {
            SpeechBubbleBehaviour bubbleBehaviour;
            //Check to see if there is a free speechbuble of the right kind to reuse
            if (_speechBubbleQueueDict[type].Count == 0 || _speechBubbleQueueDict[type].Peek().gameObject.activeInHierarchy)
            {
                GameObject newBubble = (GameObject)GameObject.Instantiate(GetPrefab(type));
                newBubble.transform.SetParent(transform);
                newBubble.transform.localScale = _sizeMultiplier * GetPrefab(type).transform.localScale;
                bubbleBehaviour = newBubble.GetComponent<SpeechBubbleBehaviour>();
                //If this is not 2D then the speechbubble will need a world space canvas.
                if (!_is2D)
                {
                    var canvas = newBubble.AddComponent<Canvas>();
                    canvas.renderMode = RenderMode.WorldSpace;
                    canvas.overrideSorting = true;
                }
            }
            else
            {
                bubbleBehaviour = _speechBubbleQueueDict[type].Dequeue();
            }
            //Set as last to always place latest on top (in case of screenspace ui that is..)
            bubbleBehaviour.transform.SetAsLastSibling();
            return bubbleBehaviour;
        }

        private GameObject GetPrefab(SpeechbubbleType type)
        {
            return _prefabsDict[type];
        }

        public SpeechbubbleType GetRandomSpeechbubbleType()
        {
            return _prefabs[Random.Range(0, _prefabs.Count)].type;
        }

        /// <summary>
        /// Clears all visible Speech Bubbles from the scene, causing them to be instantly recycled
        /// </summary>
        public void Clear() {
            foreach (var bubbleQueue in _speechBubbleQueueDict) {
                foreach (var bubble in bubbleQueue.Value) {
                    bubble.Clear();
                }
            }
        }

    }
}