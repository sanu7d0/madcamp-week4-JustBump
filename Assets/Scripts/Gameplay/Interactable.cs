using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Interactable : MonoBehaviourPunCallbacks
{
    [SerializeField] private Collider2D cc;

    protected virtual void Awake() {
        // cc = GetComponent<Collider2D>();
        // cc.isTrigger = true;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            other.GetComponent<PlayerMediator>().AddNewInteractable(this);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            other.GetComponent<PlayerMediator>().RemoveInteractable(this);
            StopInteract();
        }
    }

    public abstract void Interact();

    public abstract void Interact(PlayerMediator interactor);

    public abstract void StopInteract();

    public abstract void FinishInteract();

}
