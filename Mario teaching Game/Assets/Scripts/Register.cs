using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Register : MonoBehaviour
{

    public TMP_InputField usernameInput; // Use TMP_InputField instead of InputField
    public TMP_InputField passwordInput;
     public TMP_InputField email;
	 public TMP_InputField rePasswordInput;
     public Button registerButton;

     public TextMeshProUGUI Message;
     public  FirebaseManager firebaseManager;  

    private void Start()
    {
        registerButton.onClick.AddListener(OnRegisterButtonClicked);
        GameManager.StopGame();
        
    }
    
	private void OnRegisterButtonClicked()
    {
		if(!passwordInput.text.Equals(rePasswordInput.text))
		{
            Message.text = "The passwords do not match, please retype..";
		
		}
		else
		{
			string username = usernameInput.text;
            string password = passwordInput.text;

        // For simplicity, we assume new users start at level 1 with a score of 0 and language set to English
            firebaseManager.WriteUserData(username, password, 1, 0, 0);
            Message.text = "Registration successful! Please log in.";
		}
         
    }

	
	

}
