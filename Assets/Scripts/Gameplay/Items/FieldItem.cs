using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FieldItem : Interactable
{
    [SerializeField] private GameObject itemPrefab;

    public override void OnEnable()
    {
        base.OnEnable();
        // ...
    }

    public override void Interact(PlayerMediator interactor)
    {
        GameObject itemClone = PhotonNetwork.Instantiate(itemPrefab.name, Vector3.zero, Quaternion.identity);
        interactor.PickUpItem(itemClone);
        Destroy(gameObject);
    }
}
