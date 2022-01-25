using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInteraction : Interactable
{
    public override void Interact(PlayerMediator interactor)
    {
        interactor.PickUpItem(gameObject);
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
