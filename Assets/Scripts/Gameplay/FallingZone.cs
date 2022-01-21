using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingZone : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            // other.GetComponent<PlayerMediator>().AddNewInteractable(this);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            // other.GetComponent<PlayerMediator>().RemoveInteractable(this);
        }
    }
}
