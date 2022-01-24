using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] TMP_Text text_timer;
    [SerializeField] TMP_Text text_score;
    [SerializeField] TMP_Text text_ping;
    [SerializeField] Image weapon1;
    [SerializeField] Image weapon2;

    private GameManager gameManager;

    public PlayerMediator myPlayer;

    protected override void Awake()
    {
        base.Awake();
        gameManager = GameManager.Instance;
    }

    void Start() {

    }

    void Update() {
        if (gameManager.isPlaying) {
            text_timer?.SetText($"TIMER {Mathf.Round(gameManager.gameElapsedTime)} / {gameManager.gameLimitTime}"); 
            text_score?.SetText($"SCORE {0} / {gameManager.gameGoalScore}");
            text_ping?.SetText($"PING {PhotonNetwork.GetPing()}"); 
        }
    }

    public void UpdateWeapons() {
        foreach (Weapon w in myPlayer.weapons) {
            weapon1.sprite = w.weaponSprite;
        }
    }
}
