using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpeedUpItem: Interactable
{
    [SerializeField]
    float speed = 1;
    [SerializeField]
    float duration = 10;

    public override void FinishInteract()
    {
        //throw new System.NotImplementedException();
    }

    public override void Interact()
    {
        //throw new System.NotImplementedException();
    }

    public override void Interact(PlayerMediator interactor)
    {
        //throw new System.NotImplementedException();
    }

    public override void StopInteract()
    {
        //throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    protected override void OnTriggerEnter2D(Collider2D other)
    {

        var interactor = other.GetComponent<PlayerMediator>();
        if (PhotonNetwork.IsMasterClient && interactor is PlayerMediator) {
            interactor.IncreaseSpeed(speed);
            TimerExtension.CreateEventTimer(() =>
            {
				interactor.InitSpeed();
            }, duration);
		    PhotonNetwork.Destroy(gameObject);
		}
    }

}
