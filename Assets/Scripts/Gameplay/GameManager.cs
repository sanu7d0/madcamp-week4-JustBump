using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

sealed public class GameManager : SingletonP<GameManager>
{
    [SerializeField] private GameObject defaultPlayerPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private int _gameGoalScore;
    private LobbyManager lobbyManager;

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
    }

    void Start() {
        StartGame();
    }

    private void StartGame() {
        isPlaying = true;

        GameObject player;
        if(PhotonNetwork.IsConnected || DEBUG_OfflineMode) { 
			player = PhotonNetwork.Instantiate(lobbyManager.selectedCharacterName ?? defaultPlayerPrefab.name, GameObject.Find("Spawn0").transform.position, Quaternion.identity);
		} else { 
			player = Instantiate(defaultPlayerPrefab, GameObject.Find("Spawn0").transform.position, Quaternion.identity);
		}

        AttachMainCamera(player);
        gameStartTime = Time.time;
        StartCoroutine(StartTimer());
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
}
