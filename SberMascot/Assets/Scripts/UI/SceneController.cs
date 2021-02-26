using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI {
    public enum SwapDirection {
        ToRight,
        ToLeft
    }

    public class SceneController : MonoBehaviour {
        [SerializeField] private string groundPlaneName;
        [SerializeField] private string imageTargetName;

        private Touch _touch;
        private Vector2 _initialPosition;

        private void Update() {
            if (Input.touchCount == 1) {
                _touch = Input.GetTouch(0);

                if (_touch.phase == TouchPhase.Began) {
                    _initialPosition = _touch.position;
                    return;
                }

                if (_touch.phase == TouchPhase.Ended) {
                    Vector2 delta = _touch.position - _initialPosition;
                    TrySwapScene(delta.x > 0 ? SwapDirection.ToRight : SwapDirection.ToLeft);
                }
            }
        }

        private void TrySwapScene(SwapDirection direction) {
            Scene scene =  SceneManager.GetActiveScene();

            if (direction == SwapDirection.ToLeft && scene.name == groundPlaneName) {
                SceneManager.LoadScene(imageTargetName);
            }

            if (direction == SwapDirection.ToRight && scene.name == imageTargetName) {
                SceneManager.LoadScene(groundPlaneName);
            }
        }
    }
}
