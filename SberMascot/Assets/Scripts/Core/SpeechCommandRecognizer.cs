using System;
using System.Collections.Generic;
using System.Linq;
using Utils.Enums;
using Utils.Helpers.Commands;

namespace Core {
    public class SpeechCommandRecognizer {
        private readonly ICollection<SpeechCommandData> _speechCommandsCollection;

        public SpeechCommandRecognizer() {
            var speechCommandDataValues = Enum.GetValues(typeof(ActionCommands))
                                              .Cast<ActionCommands>()
                                              .Select(v => new SpeechCommandData(v))
                                              .Where(d => !d.IsEmpty);
            _speechCommandsCollection = new List<SpeechCommandData>(speechCommandDataValues);
        }

        public bool TryGetCommand(string message, out ActionCommands? actionCommand) {
            actionCommand = _speechCommandsCollection.FirstOrDefault(d => d.IsThisCommand(message))?.ActionCommand;
            return actionCommand != null;
        }
    }
}