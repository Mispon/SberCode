using UnityEngine;

namespace Core {
    public class PlaneTargetRotator : MonoBehaviour
    {
        private void Update()
        {
            Vector3 lookDir = Vector3.zero - transform.position;
            lookDir.x = 0;
            lookDir.z = 0;
            transform.rotation = Quaternion.LookRotation(lookDir);
        }
    }
}
