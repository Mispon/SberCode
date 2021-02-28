using System;
using System.Collections;
using System.Linq;
using UI.Buttons;
using UnityEngine;
using Utils;
using Utils.Enums;
using Utils.Helpers.Commands;
using Random = UnityEngine.Random;

namespace Managers {
    public class ActionCommandsManager: Singleton<ActionCommandsManager> {
        //ToDo: подпиши пса на прием анимации триггера
        public event Action<string> onNewAnimation;
        
        [SerializeField] private ActionCommandAnimation[] actionCommandAnimations;
        [SerializeField] private CommandButton[] commandButtons;
        [SerializeField] private ActionCommands[] idleCommands;
        
        [Header("Время ожидания новой анимации")]
        [SerializeField] private float newAnimationWaitTime = 4f;

        public ActionCommands CurrentCommand {
            get => _currentCommand;
            set {
                commandButtons.FirstOrDefault(cb => cb.actionCommand == _currentCommand)?.SetActive(false);
                commandButtons.FirstOrDefault(cb => cb.actionCommand == value)?.SetActive(true);
                _currentCommand = value;
                
                var triggerName = actionCommandAnimations.FirstOrDefault(v => v.actionCommand == value)?.triggerName;
                if (!string.IsNullOrEmpty(triggerName)) {
                    onNewAnimation?.Invoke(triggerName);
                }
            }
        }

        private int _idleAnimationsCount;
        private ActionCommands _currentCommand;
        private WaitForSeconds _newAnimationWait;

        private void Start() {
            _idleAnimationsCount = idleCommands.Length;
            _newAnimationWait = new WaitForSeconds(newAnimationWaitTime);

            onNewAnimation += animationTrigger => ResetAnimationCoroutine();
            ResetAnimationCoroutine();
        }

        private void ResetAnimationCoroutine() {
            StopCoroutine(CorSetUpRandomAnimation());
            StartCoroutine(CorSetUpRandomAnimation());
        }

        private IEnumerator CorSetUpRandomAnimation() {
            while (true) {
                yield return _newAnimationWait;
                if (_idleAnimationsCount > 0) {
                    CurrentCommand = idleCommands[Random.Range(0, _idleAnimationsCount)];   
                }
            }
        }
    }
}