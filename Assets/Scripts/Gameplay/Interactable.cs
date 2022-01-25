using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Interactable : MonoBehaviourPunCallbacks
{
    [SerializeField] protected Collider2D cc;

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            other.GetComponent<PlayerMediator>().AddNewInteractable(this);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            other.GetComponent<PlayerMediator>().RemoveInteractable(this);
        }
    }

    public abstract void Interact();

    public abstract void Interact(PlayerMediator interactor);

    public abstract void StopInteract();

    public abstract void FinishInteract();

}
