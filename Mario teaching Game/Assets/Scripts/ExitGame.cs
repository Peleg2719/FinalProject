using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleCloudStreamingSpeechToText;
using TMPro;

public class ExitGame : MonoBehaviour
{
    public Button ExitGamebtn;
    
    void Start()
    {
        ExitGamebtn.onClick.AddListener(Exit);
    }
    
    // This function will be called when the Exit button is clicked
    public void Exit()
    {
        // Quits the application
        Application.Quit();
        
        // This will only work in the Unity editor to simulate the exit during development
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
