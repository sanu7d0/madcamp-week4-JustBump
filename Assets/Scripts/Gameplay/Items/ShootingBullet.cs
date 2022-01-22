using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float duration;
    [SerializeField] private float distance;
    public LayerMask isLayer;

    public Vector2 direction = Vector2.zero;

    private void Awake() {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyBullet", duration);
    }

    // Update is called once per frame
    void Update()
    {
        moveBullet();
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }

    void moveBullet() {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, distance, isLayer);
        if (ray.collider != null) {
            // if (ray.collider.tag == "")
            DestroyBullet();
        }
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }
}
