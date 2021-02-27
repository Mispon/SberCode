using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons {
    [RequireComponent(typeof(Button))]
    public class SettingsPanelShowButton: MonoBehaviour {
        [SerializeField]
        private CanvasGroup canvasGroup;

        private void Start() {
            var button = GetComponent<Button>(); 
            button.onClick.AddListener(() => {
                canvasGroup.alpha = 1;
                canvasGroup.blocksRaycasts = true;
                canvasGroup.interactable = true;
                button.gameObject.SetActive(false);
            });
        }
    }
}