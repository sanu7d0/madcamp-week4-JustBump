using System;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class Timer : MonoBehaviour
{
    [SerializeField]
    public string arenaName = "Arena3";
    public int LimitTime = 5;
    private float startTime = 0f;
    private float elapsedTime {
        get { return LimitTime - (Time.time - startTime); }
    }
    private TMP_Text textView;
    

    public void Destory() { 
        if(PhotonNetwork.IsMasterClient) { 
			PhotonNetwork.Destroy(gameObject);
		}
    }

    private void Awake()
    {
        textView = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        LobbyUIManager.Instance.appendChild(gameObject);
        transform.position += new Vector3(0, 160, 0);
    }


    private void Update()
    {
        if(PhotonNetwork.IsMasterClient) { 
			 var photonView = PhotonView.Get(this);
			 photonView.RPC("SetText", RpcTarget.All, elapsedTime);
		}

        if(elapsedTime < 2 && PhotonNetwork.CurrentRoom.IsVisible) {
            PhotonNetwork.CurrentRoom.IsVisible = false;
		}
    
        if(elapsedTime < 0.1) { 
			if(PhotonNetwork.IsMasterClient) { 
			    PhotonNetwork.LoadLevel(arenaName);
			    PhotonNetwork.Destroy(gameObject);
			}
		}
    }

    [PunRPC]
    public void SetText(float time) {
        textView.text = Mathf.Ceil(time).ToString();
    }
}
