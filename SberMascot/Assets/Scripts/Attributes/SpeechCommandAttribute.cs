using System.ComponentModel;

namespace Attributes {
    public class SpeechCommandAttribute: DescriptionAttribute {

        public string RegexValue { get; }

        public SpeechCommandAttribute(string command, string regex = "\b{0}$") : base(command) {
            RegexValue = regex.Contains("{0}") ? string.Format(regex, command) : regex;
        }
    }
}