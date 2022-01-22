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

    public UnityEvent onFall;

    public UnityEvent onBumped;

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

    public override void OnEnable()
    {
        base.OnEnable();
    }

    private void Start()
    {
        onFall.AddListener(Dead);
        gameManager?.AddPlayer(this);
        gameManager?.InvokeOnchangePlayer();
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
        gameManager.OnChangePlayerState(this);
        gameManager?.InvokeOnchangePlayer();
        nameInstance.GetComponent<TextMeshProUGUI>().color = Color.red;
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
        Debug.Log("_Dead Start");

        Task.Run(() => {
			Debug.Log("_Dead Delay Start");
            Task.Delay(2000).Wait();
			gameObject.SetActive(false);
			isDead = true;
			gameManager.OnChangePlayerState(this);

			if(lastBumperPlayer != null) { 
					gameManager.IncrementScore(lastBumperPlayer, 3);
			}
			nameInstance.GetComponent<TextMeshProUGUI>().color = Color.red;
			gameManager.InvokeOnchangePlayer();

            Task.Run(() =>
            {
				Debug.Log("Revive");
                Revive();
            }); 
		});
    }

    public void BumpSelf(Vector2 force, IPlayer lastBumperPlayer) {
        photonView.RPC("_BumpSelf", RpcTarget.All, new object[] { lastBumperPlayer.id, lastBumperPlayer.score, lastBumperPlayer.isDead, force } );
    }
    
    [PunRPC]
    private void _BumpSelf(int id, int score, bool isDead, Vector2 force) {
        rb.AddForce(force);
        onBumped.Invoke();

	    lastBumperPlayer = new ConcretePlayer(){ id = id, isDead = isDead, score = score };
	    cancellationToken.Cancel();

	    Task.Run(() => {
			Task.Delay(3 * 1000).Wait();
			lastBumperPlayer = null;
	    }, cancellationToken.Token);

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


    class ConcretePlayer : IPlayer
    {
        public int id { get; set; }
        public int score { get; set; }
        public bool isDead { get; set; }
        public string nickname { get; set; }
    }

}