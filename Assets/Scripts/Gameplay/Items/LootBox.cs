using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class LootBox : Interactable
{
    [SerializeField] GameObject[] ItemPrefabs;
    [SerializeField] float regenTime;
    [SerializeField] Sprite sprite_Normal;
    [SerializeField] Sprite sprite_Looted;

    private SpriteRenderer spriteRenderer;

    private enum State {
        Normal,
        Looted
    };
    private State state;

    private UnityEvent<State> onStateChange;

    protected override void Awake()
    {
        base.Awake();

        spriteRenderer = GetComponent<SpriteRenderer>();

        onStateChange ??= new UnityEvent<State>();
        onStateChange.Invoke(State.Normal);
        onStateChange.AddListener(OnStateChange);
    }

    public override void Interact(PlayerMediator playerMediator)
    {
        if (state == State.Looted) {
            return;
        }

        state = State.Looted;
        onStateChange.Invoke(State.Looted);
        SpitOutItem();

        // Switch to normal state after regen time
        TimerExtension.CreateEventTimer(() => {
            state = State.Normal;
            onStateChange.Invoke(State.Normal);
        }, regenTime);
    }

    private void SpitOutItem() {
        GameObject spitItem = ItemPrefabs[Random.Range(0, ItemPrefabs.Length)];

        float radian = Random.Range(0f, 2 * Mathf.PI);
        Vector3 spitPosition = new Vector3(
            transform.position.x + 1f * Mathf.Cos(radian),
            transform.position.y + 1f * Mathf.Sin(radian),
            0
        );
        
        GameObject newItem =
            PhotonNetwork.Instantiate(spitItem.name, spitPosition, Quaternion.identity);
        
        FieldItem fi = newItem.GetComponent<FieldItem>();
        fi.InitItem();
    }

    private void OnStateChange(State s) {
        photonView.RPC("_OnStateChange", RpcTarget.All, s);
    }
    [PunRPC]
    private void _OnStateChange(State s) {
        switch (s) {
        case State.Normal:
            spriteRenderer.sprite = sprite_Normal;
            break;
        
        case State.Looted:
            spriteRenderer.sprite = sprite_Looted;
            break;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
    }
}
