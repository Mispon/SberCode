using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Buttons {
    public class SpeechButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private SpeechManager _speechManager;
        
        [SerializeField] private GameObject effect;
        [SerializeField] private float speedEffect = 1;
        [SerializeField] private float scaleEffect = 1.2f;
        private float _speed;
        private float _scale = 1;

        private void Start() {
            effect.SetActive(false);
            _speechManager = FindObjectOfType<SpeechManager>();
        }

        private void Update() {
            if (!effect.activeSelf) {
                return;
            }
            
            _scale += Time.deltaTime * _speed;
            if (_scale > scaleEffect) {
                _speed = -speedEffect;
            }

            if (_scale < scaleEffect - 0.1f) {
                _speed = speedEffect;
            }

            effect.transform.localScale = new Vector3(_scale, _scale, 1);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            effect.SetActive(true);
            _speechManager.StartRecording();
        }

        public void OnPointerUp(PointerEventData eventData) {
            effect.SetActive(false);
            _speechManager.StopRecording();
        }
    }
}
