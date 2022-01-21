using UnityEngine;
using System.Collections;
using Photon.Pun;

public class PlayerManager: MonoBehaviourPunCallbacks
{

    private GameObject go;
    private Rigidbody2D rigidBody;
    private int myPhotonViewId;


    // Use this for initialization

    void Awake()
    {
        Debug.Log("Player Manager Awake");
        Debug.Log("Player Manager Awake");
        go = gameObject;
        rigidBody = go.GetComponent<Rigidbody2D>();
        myPhotonViewId = this.photonView.ViewID;

        Debug.Log("Player Manager Awake");
        Debug.Log(photonView.ViewID);
        Debug.Log(photonView.IsMine);
    }


    public void Hitted(float weaphonPower, Vector3 weaponPos) {
        photonView.RPC("RPC_HITTED", RpcTarget.All, new object[] {weaphonPower, weaponPos} );
    }
    
    [PunRPC]
    private void RPC_HITTED(float weaphonPower, Vector3 weaponPos) {
        rigidBody.AddForce(
              (transform.position - weaponPos).normalized * weaphonPower);
    }
}
