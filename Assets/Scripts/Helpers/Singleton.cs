using UnityEngine;
using System;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<T>();
                if (_instance == null) {
                    GameObject newGO = new GameObject();
                    _instance = newGO.AddComponent<T>();
                    newGO.name = typeof(T).FullName;

                    Transform managers = GameObject.Find("Managers")?.transform;
                    if (managers) {
                        newGO.transform.parent = managers;
                    }
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake() {
        _instance = this as T;
    }
}
