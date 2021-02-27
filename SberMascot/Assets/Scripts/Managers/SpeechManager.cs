using System;
using System.Threading.Tasks;
using UI.DebugUi;
using UnityEngine;
using UnityEngine.Android;
using Utils.Helpers;

namespace Managers {
    public class SpeechManager : MonoBehaviour {
        private const string DEFAULT_LANGUAGE_CODE = "ru-RU";

        public event Action<string> onReceivedText;
    
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
            onReceivedText?.Invoke(message);
        }

        private void Initialize() {
            var androidDebug = FindObjectOfType<AndroidDebug>();
            if (Permission.HasUserAuthorizedPermission(Permission.Microphone)) {
                androidDebug.AddLog("Права на микровон даны!");
            } else {
                Permission.RequestUserPermission(Permission.Microphone);
                androidDebug.AddLog($"Права на микровон: {Permission.HasUserAuthorizedPermission(Permission.Microphone)}");
            }
            
            speechToTextManager = SpeechToText.Instance;
            speechToTextManager.Setting(DEFAULT_LANGUAGE_CODE);
            speechToTextManager.onResultCallback += OnResultSpeech;
            
            textToSpeechManager.Initialize(AudioDataSettings);
        }

        /// <summary>
        /// Speech synthesis can be called via REST API or Speech Service SDK plugin for Unity
        /// </summary>
        public async void SpeechPlayback(string msg, bool useSdk = true) {
            Debug.Log("GOT!");
            if (textToSpeechManager.IsReady) {
                if (useSdk) {
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
                Debug.Log("SpeechManager is not ready. Wait until authentication has completed.");
            }
        }
    }
}
