using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

namespace VikingCrewTools.UI {
	public class SpeechBubbleBehaviour : MonoBehaviour {
        private float _timeToLive = 1f;

        private Transform _objectToFollow;
        private Vector3 _offset;
        [FormerlySerializedAs("text")]
        [SerializeField]
        private Text _text;
        
        [FormerlySerializedAs("image")]
        [SerializeField]
        private Image _image;
        private int _iteration;
        private Camera _cam;
        /// <summary>
        /// Use this to see if a speech bubble can be updated (i.e, is still the same speech bubble, following the same character)
        /// using UpdateText
        /// </summary>
        public int Iteration
        {
            get
            {
                return _iteration;
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
            }
        }

        // Update is called once per frame
        protected void Update() {
            _timeToLive -= Time.unscaledDeltaTime;
            
            // When text is about to die start fadin out
            if (0 < _timeToLive && _timeToLive < 1) {
                _image.color = new Color(this._image.color.r, this._image.color.g, this._image.color.b, _timeToLive);
                _text.color = new Color(this._text.color.r, this._text.color.g, this._text.color.b, _timeToLive);
            }
            if (_timeToLive <= 0)
            {
                Clear();
            }
        }

        protected void LateUpdate() {
            if (_objectToFollow != null)
                transform.position = _objectToFollow.position + _offset;
            
            transform.rotation = _cam.transform.rotation;
        }

        /// <summary>
        /// Instantly removes this speech bubble, sending it to be recycled
        /// </summary>
        public void Clear() {
            gameObject.SetActive(false);
            _iteration++;
        }

        /// <summary>
        /// You can use this method to update the text inside an existing speech bubble.
        /// 
        /// Note that the speech bubble will be recycled at the end of its timeToLive so you will need to check that it is still on 
        /// the same Iteration as when you first created it. If it is on a later iteration then create a new one instead
        /// </summary>
        /// <param name="text"></param>
        /// <param name="newTimeToLive"></param>
        public void UpdateText(string text, float newTimeToLive)
        {
            _text.text = text;
            _timeToLive = newTimeToLive;
        } 

        /// <summary>
        /// Called by Speech bubble manager.
        /// Hands off!
        /// </summary>
        /// <param name="position"></param>
        /// <param name="text"></param>
        /// <param name="timeToLive"></param>
        /// <param name="color"></param>
        public void Setup(Vector3 position, string text, float timeToLive, Color color, Camera cam) {
            Setup(text, timeToLive, color, cam);

            transform.position = position;
            transform.rotation = _cam.transform.rotation;
            
            _objectToFollow = null;
            _offset = Vector3.zero;
            
            if (timeToLive > 0)
                gameObject.SetActive(true);
        }

        /// <summary>
        /// Called by Speech bubble manager.
        /// Hands off!
        /// </summary>
        /// <param name="objectToFollow"></param>
        /// <param name="offset"></param>
        /// <param name="text"></param>
        /// <param name="timeToLive"></param>
        /// <param name="color"></param>
        public void Setup(Transform objectToFollow, Vector3 offset, string text, float timeToLive, Color color, Camera cam) {
            Setup(text, timeToLive, color, cam);

            _objectToFollow = objectToFollow;

            transform.position = objectToFollow.position + offset;
            transform.rotation = _cam.transform.rotation;

            _offset = offset;
            
           

            if (timeToLive > 0)
                gameObject.SetActive(true);
        }

        private void Setup(string text, float timeToLive, Color color, Camera cam)
        {
            if (cam)
                _cam = cam;
            else
                _cam = Camera.main;

            _timeToLive = timeToLive;
            _text.text = text;
            _image.color = color;
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 1);
        }

        
    }
}