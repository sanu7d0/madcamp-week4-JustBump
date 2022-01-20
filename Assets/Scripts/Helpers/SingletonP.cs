using UnityEngine;
using Photon.Pun;

// This is a singleton for photon mono behaviour
public class SingletonP<T> : MonoBehaviourPunCallbacks where T : Component
{
    private static T _instance;
    public static T Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<T>();
                if (_instance == null) {
                    GameObject newGO = new GameObject();
                    _instance = newGO.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake() {
        _instance = this as T;
    }
}
