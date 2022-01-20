using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionInteract : Interactable
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        // Debug.Log($"{other.name} entered {this.name}");
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        // Debug.Log($"{other.name} exited {this.name}");
    }

    public override void Interact()
    {
        base.Interact();
        Debug.Log($"??? interacted with {this.name}");
    }
}
