using System.Text.RegularExpressions;
using Attributes;
using Utils.Enums;
using Utils.Extensions;

namespace Utils.Helpers.Commands {
    public class SpeechCommandData {

        public readonly ActionCommands ActionCommand;
        public readonly bool IsEmpty;
        
        private readonly Regex _regex;
        
        public SpeechCommandData(ActionCommands actionCommand) {
            ActionCommand = actionCommand;
            var speechAttribute = actionCommand.GetAttribute<SpeechCommandAttribute>();
            IsEmpty = (speechAttribute == null);
            if (IsEmpty) {
                return;
            }

            _regex = new Regex(speechAttribute.RegexValue, RegexOptions.IgnoreCase);
        }

        public bool IsThisCommand(string message) {
            if (IsEmpty) {
                return false;
            }

            return _regex.Match(message).Success;
        }
    }
}