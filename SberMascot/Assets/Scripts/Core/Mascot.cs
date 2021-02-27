using UnityEngine;

namespace Core {
    public class Mascot : MonoBehaviour {
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
