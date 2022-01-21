using UnityEngine;
using System.Collections;
using Photon.Pun;

public class PlayerManager: MonoBehaviourPunCallbacks
{

    private GameObject go;
    private Rigidbody rigidBody;
    private int myPhotonViewId;


    // Use this for initialization

    void Awake()
    {
        go = this.gameObject;
        rigidBody = go.GetComponent<Rigidbody>();
        myPhotonViewId = photonView.ViewID;
    }

    public void Hitted(float weaphonPower, Vector3 wephonPos) {
        photonView.RPC("RCP_Hitted", RpcTarget.All, weaphonPower, wephonPos);
    }
    
    [PunRPC]
    void RPC_Hitted(float weaphonPower, Vector3 weaponPos) {
        rigidBody.AddForce(
               (transform.position - weaponPos).normalized * weaphonPower);
    }

}
