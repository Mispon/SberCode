using UnityEngine;
using Vuforia;

namespace Utils {
    public class VuforiaLightEstimator : MonoBehaviour {

        public Light ambientLight;
        [Space]
        [Range(0f, 1f)] public float ambientLightWeight = 1f;  // max percentage of light change according to scene light
        [Range(0f, 1f)] public float damping = 1f;             // rate of change in time
        public int framesToSkip = 5;                           // number of frames to skip between lights estimations (free some cycles)

        private PIXEL_FORMAT mPixelFormat = PIXEL_FORMAT.UNKNOWN_FORMAT;
        private bool mAccessCameraImage = true;
        private bool mFormatRegistered;
        private byte[] pixels;
        private double totalLuminance;
        private float accLuminance;
        private float iniLuminance;
        private int frameCount;


        void Start() {
            // Grab Light intensity
            iniLuminance = ambientLight.intensity;
            accLuminance = iniLuminance;

#if UNITY_EDITOR
            mPixelFormat = PIXEL_FORMAT.GRAYSCALE; // Need Grayscale for Editor
#else
            mPixelFormat = PIXEL_FORMAT.RGB888; // Use RGB888 for mobile
#endif
            // Register Vuforia life-cycle callbacks:
            VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
            VuforiaARController.Instance.RegisterTrackablesUpdatedCallback(OnTrackablesUpdated);
            VuforiaARController.Instance.RegisterOnPauseCallback(OnPause);
        }

        private void OnVuforiaStarted() {
            //Called after vuforia camera init
            // Try register camera image format
            mFormatRegistered = CameraDevice.Instance.SetFrameFormat(mPixelFormat, true);
        }

        void OnTrackablesUpdated() { // Called each time the Vuforia state is updated
            if ((mFormatRegistered) && (mAccessCameraImage)) {
                if (frameCount <= 0) {
                    frameCount = framesToSkip;
                    Image image = CameraDevice.Instance.GetCameraImage(mPixelFormat);
                    if (image != null) {
                        pixels = image.Pixels;
                        totalLuminance = 0.0;
                        if (pixels != null && pixels.Length > 0) {

                            for (int p = 0; p < pixels.Length; p += 64) {
                                totalLuminance += pixels[p] * 0.299 + pixels[p + 1] * 0.587 + pixels[p + 2] * 0.114;
                            }
                            totalLuminance /= pixels.Length / 16;
                            totalLuminance /= 255.0;
                            accLuminance = (float)((8 * totalLuminance) * ambientLightWeight) + iniLuminance * (1 - ambientLightWeight);
                            if (accLuminance <= 0f) accLuminance = 0f;
                        }
                    }
                }
                accLuminance = ambientLight.intensity * (damping) + accLuminance * (1 - damping);
                ambientLight.intensity = accLuminance;
                frameCount -= 1;
            }
        }

        // Called when app is paused / resumed
        void OnPause(bool paused) {
            if (paused) {
                UnregisterFormat();
            } else {
                RegisterFormat();
            }
        }

        // Register the camera pixel format
        void RegisterFormat() {
            if (CameraDevice.Instance.SetFrameFormat(mPixelFormat, true)) {
                mFormatRegistered = true;
            } else {
                mFormatRegistered = false;
            }
        }

        // Unregister the camera pixel format (e.g. call this when app is paused)
        void UnregisterFormat() {
            CameraDevice.Instance.SetFrameFormat(mPixelFormat, false);
            mFormatRegistered = false;
        }
    }
}