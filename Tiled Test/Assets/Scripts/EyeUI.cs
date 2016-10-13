using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EyeUI : MonoBehaviour {

    public Sprite halfOpen;
    public Sprite fullOpen;

    private Image currentImage;

    void Awake()
    {
        currentImage = GetComponent<Image>();
        if (!currentImage) { Debug.LogError("No image in Eye UI", this); }
    }

    public void SetFullOpen()
    {
        currentImage.sprite = fullOpen;
    }

    public void SetHalfOpen()
    {
        currentImage.sprite = halfOpen;
    }
}
