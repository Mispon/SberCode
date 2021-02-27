using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Buttons {
    public class SpeechButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private SpeechManager _speechManager;

        private void Start() {
            _speechManager = FindObjectOfType<SpeechManager>();
        }

        private float Scale {
            set => transform.localScale = value * Vector3.one;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Scale = 2;
            _speechManager.StartRecording();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Scale = 1;
            _speechManager.StopRecording();
        }
    }
}
