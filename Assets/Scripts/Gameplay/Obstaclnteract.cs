using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Obstaclnteract : Interactable
{

    enum Direction { 
        Horizontal,
        Vertical,
    }
    [SerializeField]
    private float power;
    Vector3 pos;

    [SerializeField]
    float delta = 2.0f; 
    [SerializeField]
    float speed = 3.0f;
    [SerializeField]
    Direction direction;


    protected override void OnTriggerEnter2D(Collider2D other)
    {

        var interactor = other.GetComponent<PlayerMediator>();
        if(interactor is PlayerMediator) { 
			var playerPosition = interactor.transform.position;
			var opstaclePosition = transform.position;
			Vector2 force = (playerPosition - opstaclePosition).normalized *power;
			interactor.AddForce(force);
		}
    }


    public override void FinishInteract()
    {

    }

    public override void Interact()
    {

    }

    public override void Interact(PlayerMediator interactor)
    {

    }

    public override void StopInteract()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = pos;
        if(direction == Direction.Horizontal) { 
			v.x += delta * Mathf.Sin(Time.time * speed);
		}
        if(direction == Direction.Vertical) { 
			v.y += delta * Mathf.Sin(Time.time * speed);
		}

		transform.position = v;
    }
}
