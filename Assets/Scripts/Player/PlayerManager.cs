using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Photon.Pun;
using System;
using System.Threading.Tasks;
public class PlayerManager: MonoBehaviourPunCallbacks, IBumpable, IPlayer
{
    private Rigidbody2D rb;
    private GameObject mainCamera;
    private Vector3 beforeCameraPos;
    private PlayerManager lastBumper;
    private GameManager gameManager;
       
    [SerializeField] [Range(0.01f, 0.1f)] float shakeRange = 0.05f;
    [SerializeField] [Range(0.1f, 1f)] float duration = 0.5f;
    [SerializeField]

    public UnityEvent onFall;

    public int score { get; set; }
    public bool isDead { get; set; }
    public int id { get; set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameManager.Instance;
        isDead = false;
        score = 0;
        id = photonView.ViewID;
    }

    private void Start()
    {
        onFall.AddListener(Dead);
        gameManager.AddPlayer(this);
    }

    public void Revive() {
        photonView.RPC("_Revive", RpcTarget.All);
    }
    
    [PunRPC]
    private void _Revive() { 
        if(!isDead) {
            Debug.Log("Can not Revive Because live");
            return;
		}
        isDead = false;
        gameObject.SetActive(true);
        gameManager.OnChangePlayerState(this);
    }

    public void Dead() { 
        photonView.RPC("_Dead", RpcTarget.All);
    }
    
    [PunRPC]
    private void _Dead() {
        if(isDead) {
            Debug.Log("Can not Die Because Dead");
            return;
		}
        gameObject.SetActive(false);
        isDead = true;
        gameManager.OnChangePlayerState(this);
    }

    public void BumpSelf(Vector2 force) {
        _BumpSelf(force);
        photonView.RPC("_BumpSelf", RpcTarget.All, new object[] { force } );

    }
    
    [PunRPC]
    private void _BumpSelf(Vector2 force) {
        rb.AddForce(force);

        if(photonView.IsMine) {
            ShakePlayerCamera();
		}
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