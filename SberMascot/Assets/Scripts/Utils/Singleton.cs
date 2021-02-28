using System;
using UnityEngine;

namespace Utils {
    /// <summary>
    /// Паттерн "одиночка"
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
        /// <summary>
        /// Единственный экземпляр класса
        /// </summary>
        public static T Instance { get; private set; }

        [SerializeField]
        private bool dontDestroy;

        protected virtual void Awake() {
            if (Instance == null) {
                Instance = GetInstance();
            } else if (Instance != this) {
                Destroy(this);
            }
        }

        /// <summary>
        /// Returns a single object
        /// </summary>
        private T GetInstance() {
            var instance = this as T;
            if (dontDestroy)
                DontDestroyOnLoad(instance);

            return instance;
        }

        /// <summary>
        /// Creates a new object on stage
        /// </summary>
        [Obsolete("No situations to call it")]
        private static T CreateInstance() {
            var singleton = new GameObject($"{nameof(T)} (Singleton)");

            return singleton.AddComponent<T>();
        }

        // protected virtual void OnDestroy() {
        //     if(Instance == this as T)
        //         Instance = null;
        // }
    }
}