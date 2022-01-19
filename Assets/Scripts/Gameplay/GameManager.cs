using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject playerPrefab;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start() {
        
    }
}
