using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: interface로 구현?
public abstract class Interactable : MonoBehaviour
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

    public virtual void Interact() {

    }

    public virtual void StopInteract() {

    }

    public virtual void FinishInteract() {

    }
}
