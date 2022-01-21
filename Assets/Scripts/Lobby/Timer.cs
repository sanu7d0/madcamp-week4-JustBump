using System;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class Timer : MonoBehaviour
{
    [SerializeField]
    public string arenaName = "Arena2";
    public static Timer Instance;
    public int LimitTime = 5;
    private float startTime = 0f;
    private float elapsedTime {
        get { return LimitTime - (Time.time - startTime); }
    }
    private TMP_Text textView;
    private CancellationTokenSource cancellationTokenSource;
    

    public void Destory() { 
        PhotonNetwork.Destroy(gameObject);
    }

    private void Awake()
    {
        Debug.Log("TIMER AWAKE");
        textView = GetComponent<TMP_Text>();
        cancellationTokenSource = new CancellationTokenSource();
        Instance = this;
    }

    private void Start()
    {
        LobbyUIManager.Instance.appendChild(gameObject);
        transform.position += new Vector3(0, 160, 0);
        StartTimer();
    }


    private void Update()
    {
        if(PhotonNetwork.IsMasterClient) { 
		 var photonView = PhotonView.Get(this);
		 photonView.RPC("SetText", RpcTarget.All, elapsedTime);
		}
    }

    public async Task StartTimer() {
        Debug.Log("Start Timer");
        startTime = Time.time;
        await Task.Delay((LimitTime/4) * 1000 * 3, cancellationTokenSource.Token);
        PhotonNetwork.CurrentRoom.IsVisible = false;
        await Task.Delay((LimitTime/4) * 1000, cancellationTokenSource.Token);
        if(PhotonNetwork.IsMasterClient) {
            PhotonNetwork.LoadLevel(this.arenaName);
		}
    }

    [PunRPC]
    public void SetText(float time) {
        textView.text = time.ToString();
    }

    private void OnDestroy()
    {
        Debug.Log("On Destory And Cancel");
        cancellationTokenSource.Cancel();
    }
}
