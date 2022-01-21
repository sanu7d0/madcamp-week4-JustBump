using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Grenade : Weapon
{
    [SerializeField] private float explosionRadius;

    protected override void Start()
    {
        base.Start();
    }

    public override bool Use(Vector2 targetPosition)
    {
        if (weapon.durability <= 0) {
            return false;
        }

        // 테스트용 즉발 폭발
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(targetPosition, explosionRadius);
        foreach (Collider2D target in hitTargets) {
            if (target.TryGetComponent<Rigidbody2D>(out Rigidbody2D targetRb)) {
                Rigidbody2DExtension.AddExplosionForce(targetRb, weapon.power, targetPosition, explosionRadius);
                Debug.Log($"Grenade smashed {target.name}");
            }
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
