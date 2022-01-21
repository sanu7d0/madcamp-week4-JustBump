using UnityEngine;
using System.Collections;
using Photon.Pun;

public class PlayerManager: MonoBehaviourPunCallbacks, IBumpable
{
    private Rigidbody2D rb;
    private int myPhotonViewId;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myPhotonViewId = this.photonView.ViewID;
    }

    public void BumpSelf(Vector2 force) {
        photonView.RPC("RPC_BumpSelf", RpcTarget.All, new object[] { force } );
    }
    
    [PunRPC]
    private void RPC_BumpSelf(Vector2 force) {
        rb.AddForce(force);
    }
}
