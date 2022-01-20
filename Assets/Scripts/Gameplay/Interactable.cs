using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: interface로 구현?
public abstract class Interactable : MonoBehaviour
{
    private CircleCollider2D cc;

    protected virtual void Awake() {
        cc = GetComponent<CircleCollider2D>();
        cc.isTrigger = true;
    }

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

    public virtual void Interact() {

    }
}
