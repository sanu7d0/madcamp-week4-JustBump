using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Weapon_Grenade : Weapon
{
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionDelayTime;
    [SerializeField] private GameObject grenade_throwed;

    protected override void Start()
    {
        base.Start();
    }

    public override bool Use(Vector2 targetPosition)
    {
        if (weapon.durability <= 0) {
            // Debug.Log($"Durability: {weapon.durability}");
            return false;
        }

        GameObject throwedGrenade 
            = PhotonNetwork.Instantiate(grenade_throwed.name, targetPosition, Quaternion.identity);
        
        if (throwedGrenade.TryGetComponent<TimerBomb>(out TimerBomb timerBomb)) {
            IPlayer player = transform.root.GetComponent<IPlayer>();
            timerBomb.InitBomb(weapon.power, explosionRadius, explosionDelayTime, player);
        } else {
            Debug.LogError("Failed to TryGetComponent TimerBomb");
        }

        return base.Use();
    }

    protected override void PlayUseMotion()
    {
        // base.PlayUseMotion();
    }

    protected override void AllUsed()
    {
        // base.AllUsed();
    }
}
