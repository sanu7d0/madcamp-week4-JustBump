using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] TMP_Text text_timer;
    [SerializeField] TMP_Text text_score;

    private GameManager gameManager;

    protected override void Awake()
    {
        base.Awake();
        gameManager = GameManager.Instance;
    }

    void Update() {
        if (gameManager.isPlaying) {
            text_timer?.SetText($"TIMER {Mathf.Round(gameManager.gameElapsedTime)} / {gameManager.gameLimitTime}"); 
            text_score?.SetText($"SCORE {0} / {gameManager.gameGoalScore}"); 
        }
    }
}
