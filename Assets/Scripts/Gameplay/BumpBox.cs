using UnityEngine;

public class BumpBox : MonoBehaviour, IBumpable
{
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void BumpSelf(Vector2 force) {
        rb.AddForce(force);
    }
}
