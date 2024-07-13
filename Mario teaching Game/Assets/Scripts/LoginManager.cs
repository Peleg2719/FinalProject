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


    // Replace this with your own user validation logic
    private void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonClicked);
        registerButton.onClick.AddListener(OnRegisterButtonClicked);
        GameManager.IsGamePaused = true; // Pause the game
        LoginCanvas.SetActive(true);
        microphoneCanvas.SetActive(false);
        coinCanvas.SetActive(false);
        
    }

    private void OnLoginButtonClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (ValidateCredentials(username, password))
        {
            // Load the main game scene
            GameManager.StartGame();
            LoginCanvas.SetActive(false); // Disable the login canvas
            microphoneCanvas.SetActive(true);
            coinCanvas.SetActive(true);
        }
        else
        {
            Debug.Log("Invalid credentials");
            // Show an error message to the user
        }
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


