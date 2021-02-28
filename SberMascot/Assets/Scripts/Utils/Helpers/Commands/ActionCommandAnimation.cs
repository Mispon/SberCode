using System;
using Utils.Enums;

namespace Utils.Helpers.Commands {
    [Serializable]
    public class ActionCommandAnimation {
        public ActionCommands actionCommand;
        public string triggerName;
    }
}