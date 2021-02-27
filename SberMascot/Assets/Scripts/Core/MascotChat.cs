using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace Core {
    public class MascotChat : MonoBehaviour {
        [SerializeField] private string url;
        [SerializeField] private string token;
        [Space]
        [SerializeField] private GameObject bubble;
        [SerializeField] private TextMeshProUGUI bubbleContent;
        [Space]
        [SerializeField] private SpeechManager speechManager;

        private readonly List<string> _history = new List<string>();

        private void Start() {
            TriggerBubble("");
            speechManager.onReceiveTextFromSpeech += OnMessage;
        }

        private void OnMessage(string message) {
            _history.Add(message);

            StartCoroutine(SendRequest(answer => {
                if (string.IsNullOrWhiteSpace(answer))
                    return;

                _history.Add(answer);
                StartCoroutine(ShowAnswer(answer));
                speechManager.SpeechPlayback(answer);
            }));
        }

        private IEnumerator SendRequest(Action<string> callback) {
            var payload = new Payload {
                data = new Data {
                    type = "chat",
                    attributes = new Attributes {
                        chat = new Chat {
                            engine = "latest",
                            history = _history.ToArray()
                        }
                    }
                }
            };
            string jsonData = JsonUtility.ToJson(payload);

            var request = new UnityWebRequest {
                url = url,
                method = "POST",
                downloadHandler = new DownloadHandlerBuffer(),
                uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonData)),
                timeout = 60
            };
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
            request.SetRequestHeader("Authorization", token);

            yield return request.SendWebRequest();

            if (request.isHttpError || request.isNetworkError) {
                callback(request.error);
            } else {
                var response = JsonUtility.FromJson<Reply>(request.downloadHandler.text);
                callback(response.reply);
            }
        }

        private IEnumerator ShowAnswer(string answer) {
            TriggerBubble(answer);
            float delay = CalcDelay(answer.Length);
            yield return new WaitForSeconds(delay);
            TriggerBubble("");
        }

        private void TriggerBubble(string text) {
            bubbleContent.text = text;
            bubble.SetActive(text.Length > 0);
        }

        private static float CalcDelay(int answerLength) {
            float delay = answerLength / 4f;
            return Mathf.Clamp(delay, 3, 15);
        }
    }

    [Serializable]
    public class Payload {
        public Data data;
    }

    [Serializable]
    public class Data {
        public string type;
        public Attributes attributes;
    }

    [Serializable]
    public class Attributes {
        public Chat chat;
    }

    [Serializable]
    public class Chat {
        public string engine;
        public string[] history;
    }

    [Serializable]
    public class Reply {
        public string reply;
    }
}
