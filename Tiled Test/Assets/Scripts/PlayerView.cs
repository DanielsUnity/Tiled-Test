using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerModel))]
public class PlayerView : MonoBehaviour {

    private Animator animator;
    private PlayerModel playerModel; 
	
	void Start () {
        animator = GetComponent<Animator>();
        playerModel = GetComponent<PlayerModel>();
	}
	
    
	void Update () {
        UpdateFacingDirection();
	}

    void UpdateFacingDirection()
    {
        Vector3 direction = playerModel.GetFacingDirection();

        if (direction != Vector3.zero)
        {
            animator.SetFloat("DirectionX", direction.x);
            animator.SetFloat("DirectionY", direction.y);
        }

        animator.SetBool("isMoving", playerModel.IsMoving());
    }
}
