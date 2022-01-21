using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingZone : MonoBehaviour
{
    private Collider2D col;

    void Awake() {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.tag != "Player") {
            return;
        }

        // NOTE: if position.z != 0 -> Something go wrong!
        if (col.bounds.Contains(other.bounds.min) && 
            col.bounds.Contains(other.bounds.max)) {
            // Debug.Log($"{other.name} entered fully");
            other.transform.GetComponent<PlayerMediator>().StartFalling();
        }
    }
}
