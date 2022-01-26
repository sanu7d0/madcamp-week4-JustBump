using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ArenaManager : SingletonP<ArenaManager>
{
    private ArenaUIManager uiManager;

    protected override void Awake()
    {
        base.Awake();
        uiManager = ArenaUIManager.Instance;
    }

    private void Start()
    {
        uiManager.onLeaveButton.AddListener(() =>
        {
            LeaveRoom();
        });
    }

    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
        SceneManager.LoadScene(0);
    }

    private void LeaveRoom()
    {
        Debug.Log("LeaveRoom");
        PhotonNetwork.LeaveRoom();
    }
}
