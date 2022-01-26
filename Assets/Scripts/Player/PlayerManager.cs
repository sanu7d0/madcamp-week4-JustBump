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
    private PlayerMediator playerMediator;
    private GameObject mainCamera;
    private Vector3 beforeCameraPos;
    private IPlayer lastBumperPlayer;
    private GameManager gameManager;
    private GameObject nameInstance;
    [SerializeField] private int spawnTime = 5;
    [SerializeField] [Range(0.01f, 0.1f)] float shakeRange = 0.05f;
    [SerializeField] [Range(0.1f, 1f)] float duration = 0.5f;
    [SerializeField] private GameObject nameField;
    [SerializeField] private int spawnNum;
    [SerializeField] private GameObject spawnImage;

    public UnityEvent onDead;

    public UnityEvent onBumped;

    public int score { get; set; }
    public bool isDead { get; set; }
    public int id { get; set; }
    public string nickname { get; set; }
    private EventTimer cancellableTimer;
    public bool isDeading = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMediator = GetComponent<PlayerMediator>();
        gameManager = GameManager.Instance;
        isDead = false;
        score = 0;
        id = photonView.ViewID;
        nickname = photonView.Owner.NickName;
            
	    if(GameObject.Find("WorldSpaceCanvas") != null) { 
			nameInstance = Instantiate(nameField, GameObject.Find("WorldSpaceCanvas").transform);
			nameInstance.GetComponent<TextMeshProUGUI>().text = nickname;
		}   
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient) { 
			onDead.AddListener(Dead);
		}

        gameManager?.AddPlayer(this);
        gameManager?.InvokeOnchangePlayer();

        if (photonView.IsMine) {
            // Add listener to UI Manager
            playerMediator.onWeaponChange.AddListener(ArenaUIManager.Instance.UpdateWeapons);
            ArenaUIManager.Instance.myPlayer = playerMediator;
        }
   }

    void Update()
    {
        if (nameInstance != null)
        {
            nameInstance.transform.position = new Vector3(
                transform.position.x,
                transform.position.y + 0.2f,
                -1
            );
        }
    }

    public void Revive() {
        if (PhotonNetwork.IsMasterClient) { 
			photonView.RPC("_Revive", RpcTarget.All);
		}
    }
    
    [PunRPC]
    private void _Revive() { 
        if(!isDead) {
            Debug.Log("Can not Revive Because live");
            return;
		}

        isDead = false;
        lastBumperPlayer = null;
        gameObject.SetActive(true);
        nameInstance.GetComponent<TextMeshProUGUI>().color = Color.black;

        if(gameManager != null) { 
			gameManager.OnChangePlayerState(this);
			gameManager?.InvokeOnchangePlayer();
		}
    }

    public void Dead() {
        if (isDeading || isDead) {
            return;
        }
        if (PhotonNetwork.IsMasterClient) { 
			int randomSpawn = UnityEngine.Random.Range(0, spawnNum);
			Transform spawnLoc = GameObject.Find("Spawn" + randomSpawn).transform;
			GameObject spawnObject = PhotonNetwork.Instantiate(spawnImage.name, spawnLoc.position , Quaternion.identity);
            isDeading = true;
			TimerExtension.CreateEventTimer(() =>
			{
			    Revive();
			    PhotonNetwork.Destroy(spawnObject);
			}, spawnTime);

			photonView.RPC("_Dead", RpcTarget.All,spawnObject.transform.position);
		}
    }
    
    [PunRPC]
    private void _Dead(Vector3 spawPosition) {
        if(isDead) {
            Debug.Log("Can not Die Because Dead");
            return;
		}
        isDeading = false;
		isDead = true;

        if(gameManager != null) { 
			gameManager.OnChangePlayerState(this);
			if(lastBumperPlayer != null && lastBumperPlayer.id != id) { 
					gameManager.IncrementScore(lastBumperPlayer, 3);
			}
			gameManager.InvokeOnchangePlayer();
		    gameManager.InvokeOnDead(lastBumperPlayer, this);
		}

        if(nameInstance != null) { 
			nameInstance.GetComponent<TextMeshProUGUI>().color = Color.red;
		}

        TimerExtension.CreateEventTimer(() => { 
			gameObject.SetActive(false);
            gameObject.transform.position = spawPosition;
		}, 2);

    }


    public void BumpSelf(Vector2 force, IPlayer lastBumperPlayer) {
        photonView.RPC("_BumpSelf", RpcTarget.All, new object[] { lastBumperPlayer.id, lastBumperPlayer.score, lastBumperPlayer.isDead, lastBumperPlayer.nickname, force } );
    }
    
    [PunRPC]
    private void _BumpSelf(int id, int score, bool isDead, string nickname, Vector2 force) {
        rb.AddForce(force, ForceMode2D.Impulse);
        onBumped.Invoke();
            
        if(id != this.id) { 
		    lastBumperPlayer = new ConcretePlayer(){ id = id, isDead = isDead, score = score, nickname = nickname };
		}

        if(cancellableTimer != null) { 
			cancellableTimer.Stop();
		}
        cancellableTimer = TimerExtension.CreateEventTimer(() =>
        {
			lastBumperPlayer = null;
        }, 3);
        if(photonView.IsMine) {
            ShakePlayerCamera();
		}
    }

    public void BumpSelf(Vector2 force)
    {
        photonView.RPC("__BumpSelf", RpcTarget.All, new object[] { force });
    }

    [PunRPC]
    private void __BumpSelf(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
        onBumped.Invoke();
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
        photonView.RPC("_AddScore", RpcTarget.All, new object[] { score });
    }
    [PunRPC]
    public void _AddScore(int score) { 
        if(gameManager != null) { 
			gameManager.IncrementScore(this, score);
			gameManager.InvokeOnchangePlayer();
		}
    }
    private void OnDestroy()
    {
        if(gameManager != null) { 
			gameManager.RemovePlayer(this);
			gameManager.InvokeOnchangePlayer();
		}
    }

    class ConcretePlayer : IPlayer
    {
        public int id { get; set; }
        public int score { get; set; }
        public bool isDead { get; set; }
        public string nickname { get; set; }
    }

}