using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;
using Players = System.Collections.Generic.SortedDictionary<int, IPlayer>;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

sealed public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject defaultPlayerPrefab;
    [SerializeField] private GameObject missionPrefab;
    [SerializeField] private GameObject lootBoxPrefab;
    [SerializeField] private int _gameGoalScore;
    private LobbyManager lobbyManager;
    private readonly Players players = 
		new Players();
    [SerializeField] private int missionNum;
    [SerializeField] private int lootBoxNum;
    [SerializeField] private int spawnNum;

    public UnityEvent<Players> onChangePlayer;
    public int bigScore = 0;

    public int gameGoalScore {
     get { return _gameGoalScore; }
    }
    private ArenaUIManager arenaUIManager;
    
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
        arenaUIManager = ArenaUIManager.Instance;
        
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
        int randomSpawn = UnityEngine.Random.Range(0, spawnNum);
        GameObject spawnGo = GameObject.Find("Spawn" + randomSpawn);
        Vector3 spawn;
        if(spawnGo == null) {
            spawn = new Vector3(8, -5, 0);
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

        for (int i = 0; i < lootBoxNum; i++)
        {
            GameObject lootLoc = GameObject.Find("LootLoc" + i);
            if (lootLoc != null)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    GameObject lootBox = PhotonNetwork.Instantiate(lootBoxPrefab.name, lootLoc.transform.position, Quaternion.identity);
                }
            }
        }
        

        Debug.Log("Game started");
    }

    private void EndGame() {
        if(SceneManager.GetActiveScene().name == "Lobby") {
            return;
        }
        isPlaying = false;
        arenaUIManager.OnGameEnd(getWinnerPlayer().nickname);
    }

    private IPlayer getWinnerPlayer() {
        IPlayer player = null;
	   
        foreach(KeyValuePair<int, IPlayer> pair in players) { 
            if(player == null) {
                player = pair.Value;
                continue;
		    }  

            if(pair.Value.score >= player.score) {
                player = pair.Value;
		    }
		}

        return player;
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
        players[player.id].score = Mathf.Min(players[player.id].score, gameGoalScore);
    }

    public void InvokeOnchangePlayer() {
        if(SceneManager.GetActiveScene().name == "Lobby") {
            return;
		}
        var tempBigScore = 0;
        foreach(KeyValuePair<int, IPlayer> player in this.players) {
            tempBigScore = Mathf.Max(player.Value.score, tempBigScore);
		}
        bigScore = tempBigScore;

        if(bigScore >= gameGoalScore) {
            EndGame();
		}
        onChangePlayer.Invoke(players);
    }
     
}   
