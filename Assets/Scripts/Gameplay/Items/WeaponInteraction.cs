using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInteraction : Interactable
{
    public override void Interact(PlayerMediator interactor)
    {
        if (transform.parent != null) {
            return;
        }

        interactor.PickUpItem(gameObject);
        DisableInteraction();
    }
    
    public void EnableInteraction() {
        cc.enabled = true;
        this.enabled = true;
    }

    public void DisableInteraction() {
        cc.enabled = false;
        this.enabled = false;
    }

    public override void Interact()
    {
        throw new System.NotImplementedException();
    }

    public override void StopInteract()
    {
        throw new System.NotImplementedException();
    }

    public override void FinishInteract()
    {
        throw new System.NotImplementedException();
    }
}
