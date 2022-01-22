using UnityEngine;

public interface IBumpable
{
    void BumpSelf(IPlayer bumperId, Vector2 force) {}

    void BumpExplosionSelf(float explosionForce, Vector2 explosionPosition, float explosionRadius) {}
}
