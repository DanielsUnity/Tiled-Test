using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

    public void ShrinkPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (!player) { Debug.LogError("Player not found by portal", this); }
        player.GetComponent<Animator>().SetTrigger("Shrink Trigger");
    }

    public void RestartStage()
    {
        DisableAnimator();

        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.ReloadCurrrentScene();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (!player) { Debug.LogError("Player not found by portal", this); }
        player.GetComponent<Animator>().SetTrigger("Reappear Trigger");
        player.GetComponent<CharacterBehaviorModel>().Unfreeze();
    }

    void DisableAnimator()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        Animator animator = GetComponent<Animator>();
        animator.enabled = false;

    }
}
