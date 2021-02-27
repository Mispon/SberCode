using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI {
    public class SpeechButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public SpeechManager sample;

        private float Scale {
            set => transform.localScale = value * Vector3.one;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Scale = 2;
            sample.StartRecording();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Scale = 1;
            sample.StopRecording();
        }
    }
}
