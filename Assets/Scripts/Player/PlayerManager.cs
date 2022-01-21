using UnityEngine;
using System.Collections;
using Photon.Pun;
using System;
using System.Threading.Tasks;

public class PlayerManager: MonoBehaviourPunCallbacks
{
    private GameObject go;
    private Rigidbody2D rigidBody;
    private int myPhotonViewId;
    private GameObject mainCamera;
    private Vector3 beforeCameraPos;
    private bool isDeaded;

    [SerializeField] [Range(0.01f, 0.1f)] float shakeRange = 0.05f;
    [SerializeField] [Range(0.1f, 1f)] float duration = 0.5f;
    [SerializeField]
    private const string cameraName = "Main Camera";
    

    // Use this for initialization

    void Awake()
    {
        go = gameObject;
        rigidBody = go.GetComponent<Rigidbody2D>();
        myPhotonViewId = this.photonView.ViewID;
        mainCamera = GameObject.Find(cameraName);
    }

    public void Hitted(float weaphonPower, Vector3 weaponPos) {
        photonView.RPC("_Hitted", RpcTarget.All, new object[] {weaphonPower, weaponPos} );
    }
    
    [PunRPC]
    private void _Hitted(float weaphonPower, Vector3 weaponPos) {
        rigidBody.AddForce(
              (transform.position - weaponPos).normalized * weaphonPower);
    
        if(photonView.IsMine) {
            ShakePlayerCamera();
		}
    }
    
    public void Revive() {
        photonView.RPC("_Revive", RpcTarget.All);
    }
    
    [PunRPC]
    private void _Revive() { 
        // TODO Revive..
    }

    public void Dead() { 
        photonView.RPC("_Dead", RpcTarget.All);
    }
    
    [PunRPC]
    private void _Dead() {
        // TODO Dead... 
    }
    
    private void ShakePlayerCamera() {
        beforeCameraPos = mainCamera.transform.position;
        InvokeRepeating("StartShake", 0f, 0.005f);
        Invoke("StopShake", duration);
    }

    void StartShake() { 
		float camearPosX = UnityEngine.Random.value * shakeRange * 2 - shakeRange;
		float camearPosY = UnityEngine.Random.value * shakeRange * 2 - shakeRange;
	    Vector3 cameraPos = mainCamera.transform.position;
		cameraPos.x += camearPosX;
		cameraPos.y += camearPosY;
		mainCamera.transform.position = cameraPos;
	}       

	void StopShake() {
	    CancelInvoke("StartShake");
	    mainCamera.transform.position = beforeCameraPos;
	}

}
