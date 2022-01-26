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
        if (Contains2D(other.bounds.min) && 
            Contains2D(other.bounds.max)) {
            // Debug.Log($"{other.name} entered fully");
            other.transform.GetComponent<PlayerMediator>().StartFalling();
            other.GetComponent<Collider2D>().enabled = false;
        }
    }

    // Prevent unexpected contain from not zero position.z
    private bool Contains2D(Vector3 _target) {
        Vector3 target = new Vector3(_target.x, _target.y, 0);
        return col.bounds.Contains(target);
    }
}
