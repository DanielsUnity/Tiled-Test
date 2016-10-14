using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterBehaviorModel))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Character))]
public class CharacterInteractionModel : MonoBehaviour {

    public float maxAngleAbleToInteract = 30f;

    private CharacterBehaviorModel playerModel;
    private Collider2D characterCollider;
    private Character character;


	void Start () {
        character = GetComponent<Character>();
        playerModel = GetComponent<CharacterBehaviorModel>();
        characterCollider = GetComponent<Collider2D>();
	}
	

	void Update () {
	
	}


    public void OnInteract()
    {
        InteractableBase usableInteractable = FindUsableInteractable();
        if (!usableInteractable) { return; }

        if(playerModel.GetFacingDirection() == Vector3.right)
        {
            usableInteractable.OnInteractFromLeftSide(character);
        }

        if (playerModel.GetFacingDirection() == Vector3.left)
        {
            usableInteractable.OnInteractFromRightSide(character);
        }

        if (playerModel.GetFacingDirection() == Vector3.up)
        {
            usableInteractable.OnInteractFromBelow(character);
        }

        if (playerModel.GetFacingDirection() == Vector3.down)
        {
            usableInteractable.OnInteractFromAbove(character);
        }

    }


    //Loop through all colliders inside an area to see who is interactable
    //Then select the one with the closest angle to us (we also must be facing it)
    InteractableBase FindUsableInteractable()
    {
        //Collider2D[] closeColliders = Physics2D.OverlapCircleAll(transform.position, interactableRadius);

        Collider2D[] closeColliders = Physics2D.OverlapAreaAll(characterCollider.bounds.min, characterCollider.bounds.max); //It has to touch our collider

        InteractableBase closestInteractable = null;
        float angleToClosestInteractable = Mathf.Infinity;

        for (int i=0; i < closeColliders.Length; i++)
        {
            InteractableBase interactable = closeColliders[i].GetComponent<InteractableBase>();
            if (!interactable) { continue; }

            Vector3 directionToInteractableObject = closeColliders[i].transform.position - transform.position;
            float angleToInteractableObject = Vector3.Angle(playerModel.GetFacingDirection(), directionToInteractableObject);

            if (angleToInteractableObject < maxAngleAbleToInteract)
            {
                if (angleToInteractableObject < angleToClosestInteractable)
                {
                    closestInteractable = interactable;
                }
            }
        }
       
        return closestInteractable;

    }
}
