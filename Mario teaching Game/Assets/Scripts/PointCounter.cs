using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleCloudStreamingSpeechToText;
using TMPro;

public class PointCounter : MonoBehaviour
{
    public TMP_Text pointsText; // Reference to the Text component displaying points
    public static int points = 0;
    
    void Start()
    {
        ResetPoints();
        UpdatePointsDisplay();

    }
      public void ResetPoints()
    {
        points = 0;
        UpdatePointsDisplay(); // Update display after resetting
    }

   /* void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            points++;
            UpdatePointsDisplay();
        }
    }*/
    public void UpdateCoin(int pointCount)
    {
            points+=pointCount;
            UpdatePointsDisplay();
    }

    void UpdatePointsDisplay()
    {
       
        pointsText.text = points.ToString();
    }
}
