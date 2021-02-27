using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons {
    
    [RequireComponent(typeof(Button), typeof(Image))]
    public class SpriteChangeButton: MonoBehaviour {
        protected event Action<bool> onStateChanged; 
        
        [Header("Icons")]
        [SerializeField] private Sprite activeSprite;
        [SerializeField] private Sprite inactiveSprite;

        public bool IsShown {
            get => _isShown;
            private set {
                _isShown = value;
                onStateChanged?.Invoke(_isShown);
            }
        }

        private bool _isShown;
        private Button _button;
        private Image _image;
        
        protected virtual void Start() {
            _image = GetComponent<Image>();
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => IsShown = !IsShown);
            
            onStateChanged += value => _image.sprite = value ? activeSprite : inactiveSprite;;
            IsShown = false;
        }
    }
}