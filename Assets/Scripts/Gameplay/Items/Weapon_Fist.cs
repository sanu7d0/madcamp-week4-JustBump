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
        // check cooltime
        if (Time.time < lastUseTime + weapon.coolTime) {
            Debug.Log("Fist not ready yet");
            return false;
        }

        // Attack
        Collider2D[] hitTargets = new Collider2D[16];
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = true;
        int hitCount = hitBox.OverlapCollider(contactFilter, hitTargets);

        foreach(Collider2D target in hitTargets) {
            if (target == null) break; // if null, all the next is null, so break
            
            // Check the target is not itself
            if (target.transform.GetInstanceID() == transform.root.GetInstanceID()) {
                Debug.Log("Invalid target");
		        continue;
            } else {

                if (target.TryGetComponent<IBumpable>(out IBumpable bumpTarget)) {
                    Debug.Log("Player hit " + target.name);
                    if (transform.root.TryGetComponent<PhotonView>(out PhotonView photonview)) { 
					    bumpTarget.BumpSelf(
						(target.transform.position - transform.position).normalized
						* weapon.power);
				    }
				}
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
