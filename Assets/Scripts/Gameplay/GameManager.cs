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

    public bool isPlaying {
        get;
        private set;
    }

        protected override void Awake() {
            base.Awake();
        }

    void Start() {
        StartGame();
    }

    private void StartGame() {
        isPlaying = true;
        
        
        // TODO: Player 오브젝트들 찾고, phothonview.ismine 이면 카메라 붙이기
        // GameObject.FindGameObjectsWithTag("Player");
        GameObject player = Instantiate(playerPrefab, GameObject.Find("Spawn0").transform.position, Quaternion.identity);
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
