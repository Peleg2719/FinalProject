using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangImage : MonoBehaviour
{
    public Image imageComponent;
    public Sprite Recording;
    public Sprite NotRecording;
    void Start()
    {
        // Get the Image component attached to the same GameObject
        
    }

    public void ChangeImageSpriteToRecord()
    {
        if (imageComponent != null && Recording != null)
        {
            imageComponent.sprite = Recording;
        }
        else
        {
            Debug.LogError("Image component or new sprite is null.");
        }
    }
      public void ChangeImageSpriteToNotRecord()
    {
        if (imageComponent != null && NotRecording != null)
        {
            imageComponent.sprite = NotRecording;
        }
        else
        {
            Debug.LogError("Image component or new sprite is null.");
        }
    }
}

