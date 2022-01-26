using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Players = System.Collections.Generic.SortedDictionary<int, IPlayer>;
using UnityEngine.SceneManagement;

public class ArenaUIManager : Singleton<ArenaUIManager>
{
    [SerializeField] TMP_Text text_timer;
    [SerializeField] TMP_Text text_score;
    [SerializeField] TMP_Text text_ping;
    [SerializeField] Image[] weaponIcons;
    [SerializeField]
    private GameObject scorePanel;
    [SerializeField]
    private GameObject winnerText;
    [SerializeField]
    private GameObject gameEndUI;
    [SerializeField]
    private GameObject KillLogText;
    public UnityEvent onLeaveButton;
    private GameManager gameManager;

    public PlayerMediator myPlayer;
    public EventTimer removeKillLogTimer;

    protected override void Awake()
    {
        base.Awake();
        gameManager = GameManager.Instance;
        gameManager.onChangePlayer.AddListener(OnChangeScorePanel);
        gameManager.onDead.AddListener(OnDiedUser);
    }

    void Start()
    {
        if(SceneManager.GetActiveScene().name == "Lobby") {
            return;
        }
        gameEndUI.SetActive(false);
        KillLogText.SetActive(false);

        Screen.SetResolution(1920, 1080, false);
        Screen.orientation = ScreenOrientation.LandscapeLeft;
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
        if (SceneManager.GetActiveScene().name == "Lobby") {
            return;
        }
        if (gameManager.isPlaying)
        {
            text_timer?.SetText($"TIMER {Mathf.Round(gameManager.gameElapsedTime)} / {gameManager.gameLimitTime}");
            text_score?.SetText($"SCORE {Mathf.Min(gameManager.bigScore, gameManager.gameGoalScore)} / {gameManager.gameGoalScore}");
            text_ping?.SetText($"PING {PhotonNetwork.GetPing()}");
        }
        if (!gameManager.isPlaying) {
            text_timer.gameObject.SetActive(false);
            text_ping.gameObject.SetActive(false);
            text_score.gameObject.SetActive(false);
            scorePanel.SetActive(false);
            KillLogText?.SetActive(false);
		}
    }

    public void UpdateWeapons() {
        if(SceneManager.GetActiveScene().name == "Lobby") {
            return;
		}
        Weapon[] weapons = myPlayer.weapons;
        for (int i = 0; i < weapons.Length; i++) {
            weaponIcons[i].sprite = weapons[i].weaponSprite;
        }
    }

    public void OnDiedUser(IPlayer killer, IPlayer victim) {
        if(removeKillLogTimer is not null) {
            removeKillLogTimer.Stop();
		}
        if(killer is not IPlayer) {
            KillLogText.GetComponent<TextMeshProUGUI>().text = $"{victim.nickname} died alone.";
		} else { 
            KillLogText.GetComponent<TextMeshProUGUI>().text = $"{killer.nickname} killed {victim.nickname}";
		}
        KillLogText.SetActive(true);
        
        removeKillLogTimer = TimerExtension.CreateEventTimer(() =>
        {
            removeKillLogTimer = null;
            KillLogText.SetActive(false);
            KillLogText.GetComponent<TextMeshProUGUI>().text = $"";
        }, 3);
    }
}
