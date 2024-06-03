using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HelloWomenScript : MonoBehaviour
{


    public GameObject canvas;
    public Text sentenceText;
    public string sentence = "The man touched the woman!";

   void Start()
    {
        // Ensure the canvas is initially inactive
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HelloWomen"))
        {
            // Display the canvas with the sentence
            if (canvas != null && sentenceText != null)
            {
                sentenceText.text = sentence;
                canvas.SetActive(true);
                Debug.Log("mario touch women");
            }
            Debug.Log("mario touch women2");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("HelloWomen"))
        {
            // Hide the canvas when the man exits the trigger area
            if (canvas != null)
            {
                canvas.SetActive(false);
            }
        }
    }
}

