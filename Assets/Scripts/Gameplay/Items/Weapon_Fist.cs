using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // check cooltime
        if (Time.time < lastUseTime + weapon.coolTime) {
            Debug.Log("Fist not ready yet");
            return false;
        }

        // Attack
        Collider2D[] hitTargets = new Collider2D[10];
        ContactFilter2D contactFilter = new ContactFilter2D();
        int hitCount = hitBox.OverlapCollider(contactFilter, hitTargets);
        // Debug.Log("Hitcount: " + hitCount);
        
        foreach(Collider2D target in hitTargets) {
            // Check the target is not itself
            if (target == null || target.transform.GetInstanceID() == transform.root.GetInstanceID()) {
                continue;
            } else {
                // 데미지 로직
                Debug.Log("Player hit " + target.name);

                target.GetComponent<Rigidbody2D>().AddForce(
                    (target.transform.position - transform.position).normalized * weapon.power);
            }
            
        }

        return base.Use();
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
