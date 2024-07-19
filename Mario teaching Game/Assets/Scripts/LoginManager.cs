using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class LoginManager : MonoBehaviour
{
    // If you want to load a new scene after login

    public TMP_InputField usernameInput; // Use TMP_InputField instead of InputField
    public TMP_InputField passwordInput;
    public Button loginButton;
    public Button registerButton;
    public GameObject LoginCanvas;
    public GameObject microphoneCanvas;
    public GameObject coinCanvas;
    public TextMeshProUGUI errorMessage;
    public FirebaseManager firebaseManager;

    public GameObject CanvasScore;

    public GameObject LanguageCanvas;
   

    // Replace this with your own user validation logic
    private void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonClicked);
        registerButton.onClick.AddListener(OnRegisterButtonClicked);
        GameManager.StopGame();
        LoginCanvas.SetActive(true);
        microphoneCanvas.SetActive(false);
        coinCanvas.SetActive(false);
        CanvasScore.SetActive(false);
        errorMessage.text = ""; // Initialize the error message as empty
          if(firebaseManager.userData.username!="")
        {
            LoginCanvas.SetActive(false);
        }
    }
     
      public void StartAgain()
    {
        
        if(firebaseManager.userData.username!="")
        {
            LoginCanvas.SetActive(false);
            LanguageCanvas.SetActive(true);
        }
    }
    private void OnLoginButtonClicked()
    {
        
        string username = usernameInput.text;
        string password = passwordInput.text;

        firebaseManager.ReadUserData(username, userData =>
        {
            
            if (userData == null)
            {
                Debug.Log("Invalid credentials");
                errorMessage.text = "Invalid username or password. Please try again.";
            }
            if (userData != null && userData.password == password)
            {
                UserManager.Instance.SetCurrentUser(userData);
                LoginCanvas.SetActive(false); // Disable the login canvas
              
               
                
            }
            else
            {
                Debug.Log("Invalid credentials");
                errorMessage.text = "Invalid username or password. Please try again.";
            }
            return;
        });
        
       
    }


    private void OnRegisterButtonClicked()
    {
        // Implement registration logic here
    }

    private bool ValidateCredentials(string username, string password)
    {
        // Replace with your own validation logic, e.g., check against a database
        return username == "1" && password == "1";
    }
}


