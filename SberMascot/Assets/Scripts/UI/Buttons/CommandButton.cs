using Managers;
using Utils.Enums;

namespace UI.Buttons {
    public class CommandButton : SpriteChangeButton {
        public ActionCommands actionCommand;

        private bool _calledFromManager;
        protected override void Start() {
            base.Start();
            onStateChanged += value => {
                if (!value || _calledFromManager) {
                    _calledFromManager = false; 
                    return;
                }

                ActionCommandsManager.Instance.CurrentCommand = actionCommand;
            };
        }
        
        public void SetActive(bool value) {
            _calledFromManager = value;
            IsShown = value;
        }
    }
}