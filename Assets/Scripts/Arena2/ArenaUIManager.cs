using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Players = System.Collections.Generic.SortedDictionary<int, IPlayer>;


public class ArenaUIManager : Singleton<ArenaUIManager>
{
    [SerializeField]
    private GameObject scorePanel;
    [SerializeField]
    private GameObject scoreItemPrefab;
    public UnityEvent onLeaveButton;
    private GameManager gameManager;

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < 10; i++) {
            var parentTansfrom = scorePanel.GetComponent<RectTransform>();
            var childObject = Instantiate(scoreItemPrefab);
            childObject.transform.SetParent(parentTansfrom, false);
            childObject.transform.position += new Vector3(0, 10 * i, 0);
        }
        gameManager = GameManager.Instance;
        gameManager.onChangePlayer.AddListener(OnChangeScorePanel);
    }

    // Start is called before the first frame update
    void Start()
    {

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
                    textMesh.color = Color.gray;
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
}
