using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using GoogleCloudStreamingSpeechToText;

public class ExplanationManager : MonoBehaviour
{
    public GameObject ChooshLangugaeCanvas;
    public GameObject ExplanationCanvas;
 
    public Button OkButton;
 

    // Start is called before the first frame update
    void Start()
    {
        OkButton.onClick.AddListener(OnOkClick);
        GameManager.StopGame();
        ExplanationCanvas.SetActive(true);


    }
    public void StartAgain()
    {
         OkButton.onClick.AddListener(OnOkClick);
        GameManager.StopGame();
        ExplanationCanvas.SetActive(true);
   
    }


   public void OnOkClick()
   {
     
     ExplanationCanvas.SetActive(false); // Disable the explanation canvas
     ChooshLangugaeCanvas.SetActive(true); // enable the choosh langugae canvas

   }
   
 
    // Update is called once per frame
    void Update()
    {
        
    }
}
