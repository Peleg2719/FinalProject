using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UserManager : MonoBehaviour
{
    public static UserManager Instance { get; private set; }

    public UserData CurrentUser { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetCurrentUser(UserData userData)
    {
        this.CurrentUser = userData;
    }

    public void UpdateUserScore(int scoreEn, int scoreEs)
    {
        if (CurrentUser != null)
        {
            CurrentUser.scoreEn = scoreEn;
            CurrentUser.scoreEs = scoreEs;
            // You may want to save this data to Firebase here
        }
    }

    public void UpdateUserLevel(int level)
    {
        if (CurrentUser != null)
        {
            CurrentUser.level = level;
            // You may want to save this data to Firebase here
        }
    }

    // Add more user-related methods as needed
}