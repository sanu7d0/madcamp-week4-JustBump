using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FieldItem : Interactable
{
    public GameObject itemPrefab;
    public GameObject itemObject;
    public int durability;

    public void InitItem() {
        photonView.RPC("_InitItem", RpcTarget.All);
    }

    [PunRPC]
    private void _InitItem() {
        if (PhotonNetwork.IsMasterClient) {
            itemObject = 
                PhotonNetwork.Instantiate(itemPrefab.name, transform.position, Quaternion.identity);
            itemObject.name = itemPrefab.name;
        }
        itemObject.SetActive(false);
    }

    public override void Interact(PlayerMediator interactor)
    {
        interactor.PickUpItem(itemObject);
        PhotonNetwork.Destroy(gameObject);
    }

    public override void Interact()
    {
    }

    public override void StopInteract()
    {
    }

    public override void FinishInteract()
    {
    }
}
