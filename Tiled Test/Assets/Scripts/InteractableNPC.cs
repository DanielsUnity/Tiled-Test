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
        dialogManager.Manage();
    }

    public override void OnInteractFromAbove(Character character)
    {
        spriteRenderer.sprite = facingUp;
        dialogManager.Manage();
    }

    public override void OnInteractFromLeftSide(Character character)
    {
        spriteRenderer.sprite = facingLeft;
        dialogManager.Manage();
    }

    public override void OnInteractFromRightSide(Character character)
    {
        spriteRenderer.sprite = facingRight;
        dialogManager.Manage();
    }
}