using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour
{

	/*public string SecretKey = "123456789";
	public string RegisterPhpUrl = "";

	[Space(5)] 
	public InputField UserInput = null;
	public InputField EmailInput = null;
	public InputField PasswordInput = null;
	public InputField Re_PasswordInput = null;
	
	[Space(5)] 
	public int MaximumNameLenght = 20;
	public int MaximumEmailLenght = 30;

	protected string UserName = "";
	protected string Email = "";
	protected string Password = "";
	protected string Re_Password = "";

	private bool isRegister = false;

	private void Start()
	{
		if ((UserInput && EmailInput) != null)
		{
			UserInput.characterLimit = MaximumNameLenght;
			PasswordInput.characterLimit = MaximumEmailLenght;
		}
	}

	public static char[] destination = {'\0'};

	private void Update()
	{
		if (UserInput != null)
		{
			UserInput.text = Regex.Replace(UserInput.text, @"^a-zA-Z0-9", ""); // symbols which we do not approve!
			UserName = UserInput.text;
		}

		if (EmailInput != null)
		{
			string email = EmailInput.text;
			Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
			Match match = regex.Match(email);

			Email = match.ToString();
		}

		if (PasswordInput != null)
		{
			Password = PasswordInput.text;
		}
		if (Re_PasswordInput != null)
		{
			Re_Password = Re_PasswordInput.text;
		}
	}

	public void RegisterUser()
	{
		string hash = HashingData.Md5Sum(UserName + Email + Password + Re_Password);
		
		if (isRegister) // if user has already registered
		{
			return;
		}

		if (UserName != string.Empty && Email != String.Empty && Password != String.Empty && Re_Password != String.Empty)
		{
			if (Password == Re_Password)
			{
				StartCoroutine(RegisterRoutine());
				LoginManager.UpdateDescription("");
			}
			else
			{
				LoginManager.UpdateDescription("Passwords does not match");
			}
		}
		else
		{
			if (UserName == String.Empty)
			{
				LoginManager.UpdateDescription("Username is empty");
			}
			if (Email == String.Empty)
			{
				LoginManager.UpdateDescription("Email is empty or you have entered an invalid email address");
			}
			if (Password == String.Empty)
			{
				LoginManager.UpdateDescription("Password is empty");
			}
			if (Re_Password == String.Empty)
			{
				LoginManager.UpdateDescription("Password is empty");
			}
			LoginManager.UpdateDescription("Complete all the fields above.");
		}
	}

	IEnumerator RegisterRoutine()
	{
		if (isRegister)
		{
			yield return null;
		}

		isRegister = true;
		
		LoginManager.UpdateDescription("Registering.....");
		LoginManager.LoadingCache.ChangeText("Request Register", true);

		string hash = HashingData.Md5Sum(UserName + Email + Password + SecretKey).ToLower();
		
		WWWForm Form = new WWWForm();
		
		Form.AddField("name", UserName);
		Form.AddField("password", Password);
		Form.AddField("email", Email);
		Form.AddField("score", 0);
		Form.AddField("IP", LoginManager.IP);
		Form.AddField("hash", hash);
		
		WWW www = new WWW(RegisterPhpUrl, Form);
		
		LoginManager.UpdateDescription("Uploading your info to the DB. Stay still!");
		
		yield return www;
		
		LoginManager.LoadingCache.ChangeText("Getting the response from DB.....", true, 3f);

		if (www.text == "Done") // gonna check this for the server side programming
		{
			LoginManager.UpdateDescription("Registered succesfully, good job." + "You can Login now.");
			this.GetComponent<LoginManager>().ShowLogin();
		}
		else
		{
			LoginManager.UpdateDescription(www.text);
		}

		isRegister = false;
	}*/
	
	

}
