using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.DebugUi {
    public class AndroidDebug : MonoBehaviour
    {
        public Text txtLog;
        public Text txtNewLog;
        public RectTransform RmsBar;

        private void Start() {
            var speechInstance = SpeechToText.Instance;
            
            speechInstance.onResultCallback += OnResultCallback;
#if UNITY_ANDROID
            speechInstance.onReadyForSpeechCallback += OnReadyForSpeechCallback;
            speechInstance.onEndOfSpeechCallback += OnEndOfSpeechCallback;
            speechInstance.onRmsChangedCallback += OnRmsChangedCallback;
            speechInstance.onBeginningOfSpeechCallback += OnBeginningOfSpeechCallback;
            speechInstance.onErrorCallback += OnErrorCallback;
            speechInstance.onPartialResultsCallback += OnPartialResultsCallback;
#else
        gameObject.SetActive(false);
#endif
        }

        public void AddLog(string log)
        {
            txtLog.text += "\n" + log;
            txtNewLog.text = log;
            Debug.Log(log);
        }
        private void OnResultCallback(string _data)
        {
            AddLog("Result: " + _data);
        }

        private void OnReadyForSpeechCallback(string _params)
        {
            AddLog("Ready for the user to start speaking");
        }

        private void OnEndOfSpeechCallback()
        {
            AddLog("User stops speaking");
        }

        private void OnRmsChangedCallback(float _value)
        {
            float _size = _value * 10;
            RmsBar.sizeDelta = new Vector2(_size, 5);
        }

        private void OnBeginningOfSpeechCallback()
        {
            AddLog("User has started to speak");
        }

        private void OnErrorCallback(string _params)
        {
            AddLog("Error: " + _params);
        }

        private void OnPartialResultsCallback(string _params)
        {
            AddLog("Partial recognition results are available " + _params);
        }
    }
}
