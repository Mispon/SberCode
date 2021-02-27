using System;
using Utils.Enums;

namespace Utils.Helpers {
    public class AudioDataSettings {
        public event Action<AudioDataSettings> onInitialized;
        
        public float Rate { get; private set; } = 0.75f;
        public int VoicePitch { get; private set; } = 0;
        public float SourcePitch { get; private set; } = 1.55f;
        public VoiceName VoiceName { get; private set; } = VoiceName.ruRUEkaterinaRUS;

        public void Initialize() {
            onInitialized?.Invoke(this);
        }
        
        public void Initialize(float? rate = null, int? voicePitch = null, float? sourcePitch = null, VoiceName? voiceName = null) {
            if (rate.HasValue) {
                Rate = rate.Value;
            }
            
            if (voicePitch.HasValue) {
                VoicePitch = voicePitch.Value;
            }
            
            if (sourcePitch.HasValue) {
                SourcePitch = sourcePitch.Value;
            }
            
            if (voiceName.HasValue) {
                VoiceName = voiceName.Value;
            }
            
            onInitialized?.Invoke(this);
        }
    }
}