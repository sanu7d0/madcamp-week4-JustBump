using UnityEngine;

public interface IBumpable
{
    void BumpSelf(Vector2 force) {}

    void BumpExplosionSelf(float explosionForce, Vector2 explosionPosition, float explosionRadius) {}
}
