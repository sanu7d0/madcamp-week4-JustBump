using UnityEngine;
using System.Collections;
using Photon.Pun;

public class PlayerManager: MonoBehaviourPunCallbacks
{

    public static GameObject LocalPlayerInstance;
    // Use this for initialization

    private void Awake()
    {
        if(photonView.IsMine) {
            PlayerManager.LocalPlayerInstance = this.gameObject;
		}
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
