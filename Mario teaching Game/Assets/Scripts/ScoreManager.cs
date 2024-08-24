using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using GoogleCloudStreamingSpeechToText;
using System.Threading;
public class ScoreManager : MonoBehaviour
{
    public Button ExitButton;
    public Button PlayAgainButton;
    public GameObject ChooshLangugaeCanvas;
    public GameObject ScoreCanvas;
    public GameObject LoginCanvas;
     public PointCounter pointCounter;
      public TextMeshProUGUI textCoin;
      public GameManager gameManager;
        public GameObject microphoneCanvas;
    public GameObject coinCanvas;
    public LanguageManager1 languageManager1;
    public LoginManager loginManager;
  
  public FirebaseManager firebaseManager;
  

    // Start is called before the first frame update
    void Start()
    {
        
        ExitButton.onClick.AddListener(OnExitButtonClicked);
        PlayAgainButton.onClick.AddListener(OnPlayAgainButtonClicked);
        if(languageManager1.Language.Equals("es"))
        {
             textCoin.text="You have: "+pointCounter.GetCoin()+$" Amount of coins.\n Your level is:{firebaseManager.userData.levelEs}";
        }
        else
        {
                textCoin.text="You have: "+pointCounter.GetCoin()+$" Amount of coins.\n Your level is:{firebaseManager.userData.levelEn}";
        }
        
        microphoneCanvas.SetActive(false);
        coinCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(languageManager1.Language.Equals("es"))
        {
           if(firebaseManager.userData.levelEs==1)
           {
                  textCoin.text="Spanish game:\n-You have: "+pointCounter.GetCoin()+$" Amount of coins.\n-Your level is:{firebaseManager.userData.levelEs}.\nYou have {50-pointCounter.GetCoin()} more points left to reach level 2!";
           } 
           else
           {
                  textCoin.text="Spanish game:\n-You have: "+pointCounter.GetCoin()+$" Amount of coins.\n-Your level is:{firebaseManager.userData.levelEs}.";

           }
          
        }
        else
        {
            if(firebaseManager.userData.levelEn==1)
            {
                textCoin.text="-English game:\n-You have: "+pointCounter.GetCoin()+$" Amount of coins.\nYour level is:{firebaseManager.userData.levelEn}.\nYou have {50-pointCounter.GetCoin()} more points left to reach level 2!";

            }
            else
            {
                textCoin.text="-English game:\n-You have: "+pointCounter.GetCoin()+$" Amount of coins.\nYour level is:{firebaseManager.userData.levelEn}.";

            }
              
        }
        
    }
    public void StartAgain()
    {
         microphoneCanvas.SetActive(false);
        coinCanvas.SetActive(false);
    }
    
    public void OnPlayAgainButtonClicked()
    {
        var user=firebaseManager.userData;
          
        GameManager.Instance.ResetLevel(0);
      
    
        
    }
    
    public void OnExitButtonClicked()
    {
        
        GameManager.Instance.ResetLevel(1f);
        

    }
}
