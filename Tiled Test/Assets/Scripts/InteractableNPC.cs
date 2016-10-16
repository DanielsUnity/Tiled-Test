using UnityEngine;
using System.Collections;

[RequireComponent(typeof(DialogManager))]
public class InteractableNPC : InteractableBase
{
    public Sprite facingDown;
    public Sprite facingUp;
    public Sprite facingLeft;
    public Sprite facingRight;

    private DialogManager dialogManager;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        dialogManager = GetComponent<DialogManager>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();//Don't do that if the character may have more than one sprite renderer (maybe carrying an object)
        if (!spriteRenderer) { Debug.LogError("Sprite renderer not found in child!", this); }
    }

    public override void OnInteractFromBelow(Character character)
    {
        spriteRenderer.sprite = facingDown;
        spriteRenderer.flipX = false;
        dialogManager.Manage();
    }

    public override void OnInteractFromAbove(Character character)
    {
        spriteRenderer.sprite = facingUp;
        spriteRenderer.flipX = false;
        dialogManager.Manage();
    }

    public override void OnInteractFromLeftSide(Character character)
    {
        if (!facingLeft)
        {
            spriteRenderer.sprite = facingRight;
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.sprite = facingLeft;
            spriteRenderer.flipX = false;
        }
        dialogManager.Manage();
    }

    public override void OnInteractFromRightSide(Character character)
    {
        if (!facingRight)
        {
            spriteRenderer.sprite = facingLeft;
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.sprite = facingRight;
            spriteRenderer.flipX = false;
        }
        dialogManager.Manage();
    }
}