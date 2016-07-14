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
        UpdateDirection();
	}

    void UpdateDirection()
    {
        Vector3 direction = playerModel.GetDirection();

        if (direction != Vector3.zero)
        {
            animator.SetFloat("DirectionX", direction.x);
            animator.SetFloat("DirectionY", direction.y);
        }

        animator.SetBool("isMoving", playerModel.IsMoving());
    }
}
