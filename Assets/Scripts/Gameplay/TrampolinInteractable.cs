using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolinInteractable : Interactable
{
    public enum Direction {
		Left,
	    Right,
	    Up,
	    Down
    }
	
	[SerializeField]
    private Direction direction;
	[SerializeField]
    private float power;


    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        Debug.Log("Hello");
        if (other.tag == "Player")
        {
            var player = other.GetComponent<PlayerMediator>();

            if(direction == Direction.Left) {
                player.AddForce(new Vector2(-1, 0)*power);
		    } 

            if(direction == Direction.Right) {
                player.AddForce(new Vector2(1, 0)*power);
		    }
            
            if(direction == Direction.Up) {
                player.AddForce(new Vector2(0, 1)*power);
		    }

            if(direction == Direction.Down) {
                player.AddForce(new Vector2(0, -1)*power);
		    }

        }
    }

    public override void Interact()
    {
        throw new System.NotImplementedException();
    }

    public override void Interact(PlayerMediator interactor)
    {
        throw new System.NotImplementedException();
    }

    public override void StopInteract()
    {
        throw new System.NotImplementedException();
    }

    public override void FinishInteract()
    {
        throw new System.NotImplementedException();
    }
}
