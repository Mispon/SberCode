using UnityEngine;

namespace Core {
    public class Mascot : MonoBehaviour {
        [SerializeField] private Transform arCamera;

        private void OnBecameVisible() {
            transform.LookAt(arCamera);
        }
    }
}
