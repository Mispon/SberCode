using UnityEngine;

namespace Core {
    public class Mascot : MonoBehaviour {
        private void Update() {
            transform.LookAt(Vector3.zero);
        }
    }
}
