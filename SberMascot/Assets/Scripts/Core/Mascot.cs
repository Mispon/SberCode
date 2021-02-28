using System;
using Managers;
using Managers.Speech;
using UnityEngine;

namespace Core {
    [RequireComponent(typeof(MascotChat))]
    public class Mascot : MonoBehaviour {
        [SerializeField]
        private SpeechManager speechManager;
        
        private MascotChat _chat;
        private readonly SpeechCommandRecognizer _speechCommandRecognizer = new SpeechCommandRecognizer(); 

        private void Start() {
            _chat = GetComponent<MascotChat>();
            if (speechManager == null) {
                speechManager = SpeechManager.Instance;
            }
            speechManager.onReceiveTextFromSpeech += OnSpeechCommand;
        }

        private void OnSpeechCommand(string message) {
            bool recognizeCommand = _speechCommandRecognizer.TryGetCommand(message, out var actionCommand);
            if (recognizeCommand) {
                ActionCommandsManager.Instance.CurrentCommand = actionCommand ?? throw new ArgumentException("Not found");
            } else {
                _chat.OnMessage(message);   
            }
        }

        /// <summary>
        /// Place actor callback
        /// </summary>
        public void OnPlaced() {
            Vector3 lookDir = Vector3.zero - transform.position;
            lookDir.y = 0; // keep only the horizontal direction
            transform.rotation = Quaternion.LookRotation(lookDir);
        }
    }
}
