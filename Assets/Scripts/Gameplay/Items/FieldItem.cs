using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FieldItem : Interactable
{
    public GameObject itemPrefab;
    public GameObject itemObject;
    public int durability;

    void Start() {
        itemObject = PhotonNetwork.Instantiate(itemPrefab.name, transform.position, Quaternion.identity);
        itemObject.name = itemPrefab.name;
        itemObject.SetActive(false);
    }

    // public override void OnEnable()
    // {
    //     base.OnEnable();
    //     itemObject.SetActive(false);
    // }

    public override void Interact(PlayerMediator interactor)
    {
        itemObject.SetActive(true);
        interactor.PickUpItem(itemObject);
        Debug.Log($"Destorying {gameObject.name}");
        PhotonNetwork.Destroy(gameObject);
    }
}
