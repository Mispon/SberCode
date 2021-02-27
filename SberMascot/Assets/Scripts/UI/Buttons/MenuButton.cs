using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons {
    
    [RequireComponent(typeof(Button), typeof(Image), typeof(Animation))]
    public class MenuButton: SpriteChangeButton {
        
        [Header("FoldoutAnimations")]
        [SerializeField] private string activeAnimation;
        [SerializeField] private string inactiveAnimation;

        protected override void Start() {
            base.Start();

            var foldoutAnimation = GetComponent<Animation>();
            onStateChanged += value => foldoutAnimation.Play(value ? activeAnimation : inactiveAnimation);
        }
    }
}