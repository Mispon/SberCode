using System;
using System.Collections.Generic;
using Managers;
using Managers.Speech;
using UnityEngine;
using UnityEngine.UI;
using Utils.Enums;
using Utils.Helpers;
using Utils.Helpers.Speech;

namespace UI {
    public class UiSpeechManager : MonoBehaviour {

        [Header("Manager")]
        [SerializeField] private SpeechManager speechManager;
        
        [Header("UI")]
        [SerializeField] private Toggle useSDK;
        [Space]
        [SerializeField] private Slider rateSlider;
        [SerializeField] private Slider voicePitchSlider;
        [SerializeField] private Slider sourcePitchSlider;
        [Space]
        [SerializeField] private Text rateText;
        [SerializeField] private Text voicePitchText;
        [SerializeField] private Text sourcePitchText;
        [Space]
        [SerializeField] private Dropdown voiceList;
        [Space]
        [SerializeField] private InputField inputMessage;

        [Space]
        [SerializeField] private Button speechButton;
        [SerializeField] private Button settingsButton;
        
        [Header("Debug")]
        [SerializeField]
        private Transform shape;

        private void Start() {
            if (speechManager == null) {
                speechManager = SpeechManager.Instance;
            }
            
            List<string> voices = new List<string>(Enum.GetNames(typeof(VoiceName)));
            voiceList.AddOptions(voices);
            rateSlider.onValueChanged.AddListener((value) => rateText.text = $"{value: 0.00}");
            voicePitchSlider.onValueChanged.AddListener((value) => voicePitchText.text = $"{value: 0.00}");
            sourcePitchSlider.onValueChanged.AddListener((value) => sourcePitchText.text = $"{value: 0.00}");
            
            var defaultSettings = new AudioDataSettings();
            voiceList.value = ((int)defaultSettings.VoiceName);
            rateSlider.value = (defaultSettings.Rate);
            voicePitchSlider.value = (defaultSettings.VoicePitch);
            sourcePitchSlider.value = (defaultSettings.SourcePitch);

            speechButton.onClick.AddListener(SpeechPlayback);
            settingsButton.onClick.AddListener(SetUp);
            
            speechManager.onReceiveTextFromSpeech += (message) => inputMessage.text = message;
        }

        // The spinning cube is only used to verify that speech synthesis doesn't introduce
        // game loop blocking code.
        public void Update()
        {
            if (shape != null)
                shape.Rotate(Vector3.forward, 1);
        }

        /// <summary>
        /// Speech synthesis can be called via REST API or Speech Service SDK plugin for Unity
        /// </summary>
        private void SpeechPlayback() {
            Debug.Log("GOT!");
            if (speechManager.IsReady) {
                speechManager.SpeechPlayback(inputMessage.text);
            } else {
                Debug.LogError("SpeechManager is not ready. Wait until authentication has completed.");
            }
        }

        private void SetUp() {
            speechManager.AudioDataSettings.Initialize(useSDK.isOn,
                                                       rateSlider.value,
                                                       Mathf.FloorToInt(voicePitchSlider.value),
                                                       sourcePitchSlider.value,
                                                       (VoiceName) voiceList.value);
        }
    }
}
