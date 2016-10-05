using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterBehaviorModel))]
public class CharacterView : MonoBehaviour {

    private Animator animator;
    private CharacterBehaviorModel characterModel; 
	
	void Start () {
        animator = GetComponent<Animator>();
        characterModel = GetComponent<CharacterBehaviorModel>();
	}
	
    
	void Update () {
        UpdateFacingDirection();
	}

    void UpdateFacingDirection()
    {
        Vector3 direction = characterModel.GetFacingDirection();

        if (direction != Vector3.zero)
        {
            animator.SetFloat("DirectionX", direction.x);
            animator.SetFloat("DirectionY", direction.y);
        }

        animator.SetBool("isMoving", characterModel.IsMoving());
    }
}
