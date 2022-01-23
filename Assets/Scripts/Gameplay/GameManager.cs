using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;
using Players = System.Collections.Generic.SortedDictionary<int, IPlayer>;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

sealed public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject defaultPlayerPrefab;
    [SerializeField] private GameObject missionPrefab;
    [SerializeField] private int _gameGoalScore;
    private LobbyManager lobbyManager;
    private readonly Players players = 
		new Players();
    [SerializeField] private int missionNum;

    public UnityEvent<Players> onChangePlayer;

    public int gameGoalScore {
     get { return _gameGoalScore; }
    }
    
    [SerializeField] private int _gameLimitTime;
        public int gameLimitTime {
            get { return _gameLimitTime; }
        }
    public float gameStartTime;
    public float gameElapsedTime {
        get { return Time.time - gameStartTime; }
    }

    public bool isPlaying {
        get;
        private set;
    }

    public bool DEBUG_OfflineMode;

    protected override void Awake() {
	    base.Awake();
        lobbyManager = LobbyManager.Instance;

        if (DEBUG_OfflineMode) {
            PhotonNetwork.OfflineMode = DEBUG_OfflineMode;
            PhotonNetwork.CreateRoom(null);
        }

        SetPhysics2DSettings();
    }

    void Start(){
        StartGame();
   }

    private void StartGame() {

        if(SceneManager.GetActiveScene().name == "Lobby") {
            return;
		}

        isPlaying = true;

        GameObject player;
        GameObject spawnGo = GameObject.Find("Spawn0");
        Vector3 spawn;
        if(spawnGo == null) {
            spawn = new Vector3(0, 0, 0);
		}  else {
            spawn = spawnGo.transform.position;
		}

        if(PhotonNetwork.IsConnected || DEBUG_OfflineMode) { 
            if(string.IsNullOrEmpty(lobbyManager.selectedCharacterName)){ 
				player = PhotonNetwork.Instantiate(defaultPlayerPrefab.name, spawn, Quaternion.identity);
		    } else {
				player = PhotonNetwork.Instantiate(lobbyManager.selectedCharacterName, spawn, Quaternion.identity);
			}
		} else { 
			player = Instantiate(defaultPlayerPrefab, spawn, Quaternion.identity);
		}

        Destroy(lobbyManager.gameObject);

        AttachMainCamera(player);
        gameStartTime = Time.time;
        StartCoroutine(StartTimer());
        Debug.Log("Game started");

        // TODO Mission object create
        // Mission Prefab에 프리팹넣기
        for (int i = 0; i < missionNum; i++) {
            GameObject missionLoc = GameObject.Find("MissionLoc" + i);
            if(missionLoc != null) { 
                if(PhotonNetwork.IsMasterClient) { 
				    GameObject mission = PhotonNetwork.Instantiate(missionPrefab.name, missionLoc.transform.position, Quaternion.identity);
				}
		    }
        }
        

        Debug.Log("Game started");
    }

    private void EndGame() {
        isPlaying = false;
        Debug.Log("Game ended");
    }

    private void AttachMainCamera(GameObject target) {
        if (Camera.main.TryGetComponent<CameraController>(out CameraController cc)) {
            cc.targetTransform = target.transform;
        }
    }

    IEnumerator StartTimer() {
        while (gameElapsedTime < gameLimitTime) {
            // Debug.Log("Time elapsed for " + gameElapsedTime);
            yield return new WaitForSeconds(1);
        }
        Debug.Log("Time limit reached!");
        EndGame();
    }

    public void AddPlayer(IPlayer player) {
        players.Add(player.id, player);
    }

    public void RemovePlayer(IPlayer player) {
        players.Remove(player.id);
    }
    
    public void OnChangePlayerState(IPlayer player) {
        players[player.id] = player;
    }   
    
    private void SetPhysics2DSettings() {
        Physics2D.queriesStartInColliders = false;
        // Physics2D.autoSyncTransforms = true;
    }

    public void IncrementScore(IPlayer player, int score) { 
        players[player.id].score += score;
    }

    public void InvokeOnchangePlayer() { 
        onChangePlayer.Invoke(players);
    }
     
}   
