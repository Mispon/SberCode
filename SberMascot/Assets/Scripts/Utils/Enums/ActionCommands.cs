using Attributes;

namespace Utils.Enums {
    public enum ActionCommands {
        [SpeechCommand("сидеть")]
        Stay,
        [SpeechCommand("виляй")]
        Wiggle,
        [SpeechCommand("язык")]
        Tongue
    }
}