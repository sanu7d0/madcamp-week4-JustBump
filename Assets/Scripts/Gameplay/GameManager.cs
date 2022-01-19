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
        GameObject player = Instantiate(playerPrefab, GameObject.Find("Spawn0").transform.position, Quaternion.identity);
        AttachMainCamera(player);
    }

    private void AttachMainCamera(GameObject target) {
        GameObject.Find("Main Camera");
    }
}
