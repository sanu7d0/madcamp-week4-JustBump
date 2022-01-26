using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// ** Dont inherit this Wepon_Fist class! **
sealed public class Weapon_Fist : Weapon
{
    [SerializeField] private Collider2D hitBox;

    protected override void Start()
    {
        base.Start();
    }

    public override WeaponUseResult Use()
    {
        if (TryMeleeAttack(hitBox)) {
            base.PlayUseSound();
            return base.Use();
        } else {
            return WeaponUseResult.NoHit;
        }
    }

    protected override void PlayUseMotion() {
        
    }
    
    /*void OnDrawGizmosSelected() {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }*/
}
