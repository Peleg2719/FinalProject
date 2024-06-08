using UnityEngine;
using UnityEngine.UI;
using GoogleCloudStreamingSpeechToText;
using TMPro;
public class HelloWomenScript : MonoBehaviour
{
    public  TMP_Text dialogueText; // Updated to reference TMP_Tex dialogueText; // Assign this in the Unity Editor
    public DialogManager dialogManager;
    private StreamingRecognizer recognizer;

    void Start()
    {
        /*recognizer = GetComponent<StreamingRecognizer>();
        if (recognizer == null)
        {
            Debug.LogError("StreamingRecognizer component not found!");
            return;
        }*/

       // Find the TextHelloWomen object directly
    GameObject helloWomenObject = GameObject.Find("HelloWomenText");
    if (helloWomenObject != null)
    {
        // Get the Text component from the HelloWomenText object
        dialogueText = helloWomenObject.GetComponent<TMP_Text>();
        if (dialogueText != null)
        {
            Debug.Log("Text component found and assigned successfully.");
        }
        else
        {
            Debug.LogError("Text component not found on GameObject with name 'TextHelloWomen'.");
        }
    }
    else
    {
        Debug.LogError("GameObject with name 'TextHelloWomen' not found in the scene.");
    }
     
        dialogManager = FindObjectOfType<DialogManager>();
        if (dialogManager == null)
        {
            Debug.LogError("DialogManager not found in the scene!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger area.");
            if (dialogManager != null)
            {
                dialogueText.text="Hey mario, How are you?\n Say:\n I'm fine thank you, how are you?";
             
                dialogManager.ShowDialog();
                recognizer.onFinalResult.AddListener(OnSpeechRecognized);
                recognizer.StartListening();
            }
            else
            {
                Debug.LogError("DialogManager is null when Player enters trigger area.");
            }
        }
    }

    void OnSpeechRecognized(string text)
    {
        Debug.Log("Speech Recognized: " + text);

        if (dialogueText != null && text.ToLower() == "im good thanks how are you")
        {
            dialogueText.color = Color.red;
            if (dialogManager != null)
            {
                dialogManager.HideDialogPanel();
            }
            else
            {
                Debug.LogError("DialogManager is null in OnSpeechRecognized.");
            }
        }
        else if (dialogueText == null)
        {
            Debug.LogError("dialogueText is null in OnSpeechRecognized.");
        }
    }

    void OnDestroy()
    {
        if (recognizer != null)
        {
            recognizer.StopListening();
            recognizer.onFinalResult.RemoveListener(OnSpeechRecognized);
        }
    }
}