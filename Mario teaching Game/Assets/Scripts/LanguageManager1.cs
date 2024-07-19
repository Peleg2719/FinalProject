using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using GoogleCloudStreamingSpeechToText;
public class LanguageManager1 : MonoBehaviour
{
    public Button SpanishButton;
    public Button EnglishButton;
    public GameObject ChooshLangugaeCanvas;
    public GameObject microphoneCanvas;
    public GameObject coinCanvas;
     public PointCounter pointCounter;
      public FirebaseManager firebaseManager;
      public StreamingRecognizer streamingRecognizer; // Reference to the StreamingRecognizer

      public string Language{get;set;}

    // Start is called before the first frame update
    void Start()
    {
        SpanishButton.onClick.AddListener(OnSpanishButtonClicked);
        EnglishButton.onClick.AddListener(OnEnglishButtonClicked);
        GameManager.StopGame();
        ChooshLangugaeCanvas.SetActive(true);
        microphoneCanvas.SetActive(false);
        coinCanvas.SetActive(false);
        


    }
    public void StartAgain()
    {
          SpanishButton.onClick.AddListener(OnSpanishButtonClicked);
        EnglishButton.onClick.AddListener(OnEnglishButtonClicked);
        GameManager.StopGame();
        ChooshLangugaeCanvas.SetActive(true);
        microphoneCanvas.SetActive(false);
        coinCanvas.SetActive(false);
    }


   public void OnSpanishButtonClicked()
   {
     pointCounter.UpdateCoin(firebaseManager.userData.scoreEs);
     GameManager.StartGame();
     ChooshLangugaeCanvas.SetActive(false); // Disable the login canvas
     microphoneCanvas.SetActive(true);
     coinCanvas.SetActive(true);
     Language="es";
     pointCounter.ResetPoints(firebaseManager.userData.scoreEs);
     //streamingRecognizer.SetLanguageCode("es"); // Set language to Spanish
     
   }
   
   public void OnEnglishButtonClicked()
   {
        pointCounter.UpdateCoin(firebaseManager.userData.scoreEn);
        GameManager.StartGame();
        ChooshLangugaeCanvas.SetActive(false); // Disable the login canvas
        microphoneCanvas.SetActive(true);
        coinCanvas.SetActive(true);
        Language="en";
         pointCounter.ResetPoints(firebaseManager.userData.scoreEn);
   }
    // Update is called once per frame
    void Update()
    {
        
    }
}
