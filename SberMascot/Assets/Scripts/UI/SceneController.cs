using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI {
    public class SceneController : MonoBehaviour {

        [SerializeField] private Button nextSceneButton;
        [Space]
        [SerializeField] private string[] sceneNames;

        private void Start() {
            nextSceneButton.onClick.AddListener(() => {
                nextSceneButton.interactable = false;
                Scene scene = SceneManager.GetActiveScene();
                var index = Array.IndexOf(sceneNames, scene.name);
                index = index == -1
                    ? 0
                    : (index + 1) % sceneNames.Length;

                SceneManager.LoadScene(sceneNames[index]);
            });
        }
    }
}