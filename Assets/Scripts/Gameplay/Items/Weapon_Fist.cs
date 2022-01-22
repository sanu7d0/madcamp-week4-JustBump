using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Weapon_Fist : Weapon
{
    private Collider2D hitBox;

    protected override void Start()
    {
        base.Start();
        hitBox = GetComponentInChildren<Collider2D>();
    }

    public override bool Use()
    {
        if (TryMeleeAttack(hitBox)) {
            base.PlayUseSound();
            return base.Use();
        } else {
            return false;
        }
    }

    protected override void AllUsed()
    {
        // Do Something

        base.AllUsed();
    }
    
    /*void OnDrawGizmosSelected() {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }*/
}
