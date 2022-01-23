using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Players = System.Collections.Generic.SortedDictionary<int, IPlayer>;


public class ArenaUIManager : Singleton<ArenaUIManager>
{

    [SerializeField] TMP_Text text_timer;
    [SerializeField] TMP_Text text_score;
    [SerializeField] TMP_Text text_ping;
    [SerializeField]
    private GameObject scorePanel;
    [SerializeField]
    private GameObject winnerText;
    [SerializeField]
    private GameObject gameEndUI;
    public UnityEvent onLeaveButton;
    private GameManager gameManager;

    protected override void Awake()
    {
        base.Awake();
        gameManager = GameManager.Instance;
        gameManager.onChangePlayer.AddListener(OnChangeScorePanel);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameEndUI.SetActive(false);
    }
    
    public void OnGameEnd(string winnerName) {
        gameEndUI.SetActive(true);
        winnerText.GetComponent<TextMeshProUGUI>().text = $"winner : {winnerName}";
    }

    public void OnClickLeaveButton() {
        onLeaveButton.Invoke();
    }
    

    private void OnChangeScorePanel(Players players) {
        var childCount = scorePanel.transform.childCount;
        var sortedPlayers = MapSortedPlayersByScore(players);
        var sortedPlayersLength = sortedPlayers.Count;
         
        for (int i = 0; i < childCount; i++) {
	        var childObject = scorePanel.transform.GetChild(i).gameObject;
            if(i < sortedPlayersLength) {
                var player = sortedPlayers.Values[i];
                var nickname = player.nickname;
                var score = player.score;
                var dead = player.isDead;
                var textMesh = childObject.GetComponent<TextMeshProUGUI>();
                textMesh.text = $"{nickname} : {score}";
                if (dead) {
                    textMesh.color = Color.red;
				} else { 
                    textMesh.color = Color.black;
				}
                childObject.SetActive(true);
		    } else { 
                childObject.SetActive(false);
		    }

        }
    }

    private SortedList<int, IPlayer> MapSortedPlayersByScore(Players players){
        var sortedPlayers = new SortedList<int, IPlayer>(comparer: new DuplicateKeyComparer<int>());
        foreach(KeyValuePair<int, IPlayer> kvp in players) {
            sortedPlayers.Add(kvp.Value.score, kvp.Value);
		}
        return sortedPlayers;
	}


    void Update()
    {
        if (gameManager.isPlaying)
        {
            text_timer?.SetText($"TIMER {Mathf.Round(gameManager.gameElapsedTime)} / {gameManager.gameLimitTime}");
            text_score?.SetText($"SCORE {Mathf.Min(gameManager.bigScore, gameManager.gameGoalScore)} / {gameManager.gameGoalScore}");
            text_ping?.SetText($"PING {PhotonNetwork.GetPing()}");
        }
    }
}
