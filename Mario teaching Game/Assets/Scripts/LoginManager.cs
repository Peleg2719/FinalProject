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
    public TextMeshProUGUI messageText; // Use TextMeshProUGUI for message text
    public Button loginButton;

    // Replace this with your own user validation logic
    private bool ValidateCredentials(string username, string password)
    {
        // Example: Check if username is "admin" and password is "password"
        return username == "admin" && password == "password";
    }

    private void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonClicked);
    }

    private void OnLoginButtonClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (ValidateCredentials(username, password))
        {
            messageText.text = "Login successful!";
            // Load another scene or perform some action
            // SceneManager.LoadScene("YourNextScene");
        }
        else
        {
            messageText.text = "Invalid username or password.";
        }
    }
}


