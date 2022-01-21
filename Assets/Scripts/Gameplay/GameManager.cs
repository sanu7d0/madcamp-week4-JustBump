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

    protected override void Awake() {
        lobbyManager = LobbyManager.Instance;
	    base.Awake();
    }

    void Start() {
        StartGame();
    }

    private void StartGame() {
        isPlaying = true;

        GameObject player;
        if(PhotonNetwork.IsConnected) { 
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
        GameObject camera = GameObject.Find("Main Camera");
        camera.GetComponent<CameraController>().targetTransform = target.transform;
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
