using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private int _gameGoalScore;
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

    private bool _isPlaying;
        public bool isPlaying {
            get { return _isPlaying; }
        }

    protected override void Awake() {
        base.Awake();
    }

    void Start() {
        StartGame();
    }

    private void StartGame() {
        _isPlaying = true;
        
        GameObject player = Instantiate(playerPrefab, GameObject.Find("Spawn0").transform.position, Quaternion.identity);
        AttachMainCamera(player);
        
        gameStartTime = Time.time;
        StartCoroutine(StartTimer());

        Debug.Log("Game started");
    }

    private void EndGame() {
        _isPlaying = false;
        Debug.Log("Game ended");
    }

    private void AttachMainCamera(GameObject target) {
        GameObject camera = GameObject.Find("Main Camera");
        camera.GetComponent<CameraController>().targetTransform = target.transform;
    }

    IEnumerator StartTimer() {
        while (gameElapsedTime < gameLimitTime) {
            Debug.Log("Time elapsed for " + gameElapsedTime);
            yield return new WaitForSeconds(1);
        }
        Debug.Log("Time limit reached!");
        EndGame();
    }
}
