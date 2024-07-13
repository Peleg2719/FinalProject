using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{

/*	public string LoginPhpUrl = "";
	public string NextLevel = "";

	[HideInInspector] public string User = "";
	[HideInInspector] public string Password = "";
	[HideInInspector] public bool KeepMe = false;

	[Space(6)] 
	public Toggle toggle = null;
	public InputField UserInput = null;
	public InputField PasswordInput = null;

	private bool isLogin = false;
	private bool isAlreadyLogin = false;

	private void Awake()
	{
		if (toggle != null)
		{
			if (PlayerPrefs.GetString(LoginManager.SavedUser) != String.Empty)
			{
				toggle.isOn = true;

				if (User != null)
				{
					UserInput.text = PlayerPrefs.GetString(LoginManager.SavedUser);
				}
				else
				{
					toggle.isOn = false;
				}
			}
		}
	}

	private void FixedUpdate()
	{
		if (toggle != null)
		{
			KeepMe = toggle.isOn;
		}

		if (UserInput != null)
		{
			User = UserInput.text;
		}

		if (PasswordInput != null)
		{
			Password = PasswordInput.text;
		}
	}

	public void LoginUser()
	{
		if (isLogin || isAlreadyLogin)
		{
			return;
		}

		if (User != String.Empty && Password != String.Empty)
		{
			StartCoroutine(LoginProcess());
		}
		else
		{
			if (User == String.Empty)
			{
				Debug.LogWarning("The username field is empty");
			}
			if (Password == String.Empty)
			{
				Debug.LogWarning("The password field is empty");
			}
			LoginManager.UpdateDescription("give info in both fields");
		}
	}

	IEnumerator LoginProcess()
	{
		if (isLogin || isAlreadyLogin)
			yield return null;

		isLogin = true;

		LoginManager.UpdateDescription("We are logging you in.....");

		LoginManager.LoadingCache.ChangeText("Request the Login", true);

		WWWForm Form = new WWWForm();

		// We will have our rows in the database and the fields needed
		Form.AddField("name", User);
		Form.AddField("password", Password);
		Form.AddField("IP",
			1); // We will check with if statment on the server side if we succeeded with taking IP from user.

		WWW w = new WWW(LoginPhpUrl, Form);
		yield return w;

		LoginManager.LoadingCache.ChangeText("Getting the response from the server......", true, 2f);

		string serverResult = w.text;

		if (w.error != null)
		{
			Debug.LogError(w.error);
			LoginManager.LoadingCache.ChangeText(w.error, true, 3f);
		}
		else
		{
			string[] Split = serverResult.Split("|"[0]);

			if (Split[0] == "Login Done"
			) // Here we will use that string to check on server if we have withdrawn the data successfully
			{
				Debug.Log("Login " + Split[1]);
				LoginManager.LoadingCache.ChangeText("Successfully logged in.", true, 1.4f);
				isAlreadyLogin = true;

				string name = Split[1];
				string email = String.Empty;
				int score = int.Parse(Split[4]);
				int status = int.Parse(Split[3]);
				string ip = Split[5];

				Debug.Log(serverResult);

				if (Split[5] != null && !string.IsNullOrEmpty(Split[5]))
				{
					ip = Split[5];
				}

				LoginManager.UpdateDescription("Welcome, you are in." + Split[1] +
				                               " Your score from the last game is: " + Split[3]);

				if (KeepMe)
				{
					PlayerPrefs.SetString(LoginManager.SavedUser, User);
				}
				else
				{
					PlayerPrefs.SetString(LoginManager.SavedUser, String.Empty);
				}

				LoginManager.OnLoginEvent(name, email, score, status, ip);

				yield return new WaitForSeconds(1f);

				SceneManager.LoadScene(NextLevel);
			}
			
			isLogin = false;

		}

	}*/
}
