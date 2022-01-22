using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Photon.Pun;
using System;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine.UI;
using TMPro;

public class PlayerManager: MonoBehaviourPunCallbacks, IBumpable, IPlayer
{
    private Rigidbody2D rb;
    private GameObject mainCamera;
    private Vector3 beforeCameraPos;
    private IPlayer lastBumperPlayer;
    private GameManager gameManager;
    private CancellationTokenSource cancellationToken = new CancellationTokenSource();
    private GameObject nameInstance;
    [SerializeField] [Range(0.01f, 0.1f)] float shakeRange = 0.05f;
    [SerializeField] [Range(0.1f, 1f)] float duration = 0.5f;
    [SerializeField] private GameObject nameField;

    public UnityEvent onDead;

    public int score { get; set; }
    public bool isDead { get; set; }
    public int id { get; set; }
    public string nickname { get; set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameManager.Instance;
        isDead = false;
        score = 0;
        id = photonView.ViewID;
        nickname = photonView.Owner.NickName;
        
        nameInstance = Instantiate(nameField, GameObject.Find("WorldSpaceCanvas").transform);
        nameInstance.GetComponent<TextMeshProUGUI>().text = nickname;
    }

    void OnEnable() {
        // reset
    }

    private void Start()
    {
        onDead.AddListener(Dead);
        gameManager?.AddPlayer(this);
        gameManager?.InvokeOnchangePlayer();
   }

    private void Update()
    {
        nameInstance.transform.position = new Vector3(
            transform.position.x,
            transform.position.y + 0.2f,
            0
        );
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
        gameObject.transform.position = new Vector3(0, 0, 0);
        nameInstance.GetComponent<TextMeshProUGUI>().color = Color.black;
        gameManager.OnChangePlayerState(this);
        gameManager?.InvokeOnchangePlayer();
    }

    public void Dead() { 
        photonView.RPC("_Dead", RpcTarget.All);

        TimerExtension.CreateEventTimer(() => {
            Revive();
		 }, 10);
    }
    
    [PunRPC]
    private void _Dead() {
        if(isDead) {
            Debug.Log("Can not Die Because Dead");
            return;
		}
        Debug.Log("_Dead Start");

		isDead = true;
		gameManager.OnChangePlayerState(this);
		if(lastBumperPlayer != null && lastBumperPlayer.id != id) { 
				gameManager.IncrementScore(lastBumperPlayer, 3);
		}
		nameInstance.GetComponent<TextMeshProUGUI>().color = Color.red;
		gameManager.InvokeOnchangePlayer();

        TimerExtension.CreateEventTimer(() => { 
			gameObject.SetActive(false);
		}, 2);
    }

    public void BumpSelf(Vector2 force, IPlayer lastBumperPlayer) {
        photonView.RPC("_BumpSelf", RpcTarget.All, new object[] { lastBumperPlayer.id, lastBumperPlayer.score, lastBumperPlayer.isDead, force } );
    }
    
    [PunRPC]
    private void _BumpSelf(int id, int score, bool isDead, Vector2 force) {
        rb.AddForce(force);

	    lastBumperPlayer = new ConcretePlayer(){ id = id, isDead = isDead, score = score };
        Debug.Log(lastBumperPlayer.id);
        Debug.Log(lastBumperPlayer.isDead);
        Debug.Log(lastBumperPlayer.score);
	    cancellationToken.Cancel();
        TimerExtension.CreateEventTimer(() =>
        {

			lastBumperPlayer = null;
			Debug.Log(lastBumperPlayer.id);
			Debug.Log(lastBumperPlayer.isDead);
			Debug.Log(lastBumperPlayer.score);
        }, 3);
        if(photonView.IsMine) {
            ShakePlayerCamera();
		}
    }


    public void BumpExplosionSelf(float explosionForce, Vector2 explosionPosition, float explosionRadius) {
        photonView.RPC("_BumpExplosionSelf", RpcTarget.All, new object[] { explosionForce, explosionPosition, explosionRadius } );

        if(photonView.IsMine) {
            ShakePlayerCamera();
		}
    }

    [PunRPC]
    private void _BumpExplosionSelf(float explosionForce, Vector2 explosionPosition, float explosionRadius) {
        Rigidbody2DExtension.AddExplosionForce(rb, explosionForce, explosionPosition, explosionRadius);
    }
    
    private void ShakePlayerCamera() {
        beforeCameraPos = Camera.main.transform.position;
        InvokeRepeating("StartShake", 0f, 0.005f);
        Invoke("StopShake", duration);
    }

    void StartShake() { 
		float camearPosX = UnityEngine.Random.value * shakeRange * 2 - shakeRange;
		float camearPosY = UnityEngine.Random.value * shakeRange * 2 - shakeRange;
	    Vector3 cameraPos = Camera.main.transform.position;
		cameraPos.x += camearPosX;
		cameraPos.y += camearPosY;
		Camera.main.transform.position = cameraPos;
	}       

	void StopShake() {
	    CancelInvoke("StartShake");
	    Camera.main.transform.position = beforeCameraPos;
	}

    public void AddScore(int score) {
        // 자기의 점수를 ++
        gameManager.IncrementScore(this, score);
        gameManager.InvokeOnchangePlayer();
    }

    private void OnDestroy()
    {
        gameManager.RemovePlayer(this);
        gameManager.InvokeOnchangePlayer();
    }

    class ConcretePlayer : IPlayer
    {
        public int id { get; set; }
        public int score { get; set; }
        public bool isDead { get; set; }
        public string nickname { get; set; }
    }

}