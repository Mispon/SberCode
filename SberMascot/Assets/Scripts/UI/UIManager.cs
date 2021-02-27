using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using Utils.Enums;
using Utils.Helpers;

namespace UI {
    public class UIManager : MonoBehaviour {

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
        [SerializeField] public InputField inputMessage;
        
        [Header("Debug")]
        [SerializeField]
        private Transform shape;

        private void Start() {
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

            speechManager.onReceivedText += (message) => inputMessage.text = message;
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
        public void SpeechPlayback() {
            Debug.Log("GOT!");
            if (speechManager.IsReady) {
                string msg = inputMessage.text;
                speechManager.AudioDataSettings.Initialize(rateSlider.value,
                                                           Mathf.FloorToInt(voicePitchSlider.value),
                                                           sourcePitchSlider.value,
                                                           (VoiceName) voiceList.value);
                
                speechManager.SpeechPlayback(msg, useSDK.isOn);
            } else {
                Debug.LogError("SpeechManager is not ready. Wait until authentication has completed.");
            }
        }
    }
}
