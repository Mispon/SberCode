using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Android;
using Utils;
using Utils.Helpers.Speech;

namespace Managers.Speech {
    public class SpeechManager : Singleton<SpeechManager> {
        private const string DEFAULT_LANGUAGE_CODE = "ru-RU";

        public event Action<string> onReceiveTextFromSpeech;
    
        [Header("Manager")]
        [SerializeField] private TextToSpeech textToSpeechManager;
        [SerializeField] private SpeechToText speechToTextManager;

        public bool IsReady => textToSpeechManager.IsReady && speechToTextManager.IsReady;

        public readonly AudioDataSettings AudioDataSettings = new AudioDataSettings();
        
        private void Start()
        {
            Initialize();
        }

        public void StartRecording()
        {
#if UNITY_EDITOR
#else
        speechToTextManager.StartRecording("Speak any");
#endif
        }

        public void StopRecording()
        {
#if UNITY_EDITOR
            OnResultSpeech("Not support in editor.");
#else
        speechToTextManager.StopRecording();
#endif
        }

        private void OnResultSpeech(string message) {
            onReceiveTextFromSpeech?.Invoke(message);
        }

        private void Initialize() {
            if (!Permission.HasUserAuthorizedPermission(Permission.Microphone)) {
                Permission.RequestUserPermission(Permission.Microphone);
            }
            
            speechToTextManager = SpeechToText.Instance;
            speechToTextManager.Setting(DEFAULT_LANGUAGE_CODE);
            speechToTextManager.onResultCallback += OnResultSpeech;
            
            textToSpeechManager.Initialize(AudioDataSettings);
        }

        /// <summary>
        /// Speech synthesis can be called via REST API or Speech Service SDK plugin for Unity
        /// </summary>
        public async void SpeechPlayback(string msg) {
            if (textToSpeechManager.IsReady) {
                if (AudioDataSettings.UseSdk) {
                    try {
                        await Task.Run(() => textToSpeechManager.SpeakWithSDKPlugin(msg));
                        Debug.LogError("Run async");
                    } catch (Exception ex) {
                        Debug.LogError(ex);
                    }
                } else {
                    textToSpeechManager.SpeakWithRESTAPI(msg);
                }
            } else {
                Debug.LogError("SpeechManager is not ready. Wait until authentication has completed.");
            }
        }
    }
}
