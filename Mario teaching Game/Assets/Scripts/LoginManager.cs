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


    // Replace this with your own user validation logic
    private void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonClicked);
        registerButton.onClick.AddListener(OnRegisterButtonClicked);
        GameManager.StopGame();
        LoginCanvas.SetActive(true);
        microphoneCanvas.SetActive(false);
        coinCanvas.SetActive(false);
        errorMessage.text = ""; // Initialize the error message as empty

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
                GameManager.StartGame();
                LoginCanvas.SetActive(false); // Disable the login canvas
                microphoneCanvas.SetActive(true);
                coinCanvas.SetActive(true);
            }
            else
            {
                Debug.Log("Invalid credentials");
                errorMessage.text = "Invalid username or password. Please try again.";
            }
            return;
        });
        Debug.Log("Invalid credentials");
        errorMessage.text = "Invalid username or password. Please try again.";
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


