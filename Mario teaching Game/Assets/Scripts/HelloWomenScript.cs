using UnityEngine;
using UnityEngine.UI;
using GoogleCloudStreamingSpeechToText;
using TMPro;
using System.Collections;

public class HelloWomenScript : MonoBehaviour
{
    public TMP_Text dialogueText; // Reference TMP_Text dialogueText; Assign this in the Unity Editor
    public DialogManager dialogManager;
    private StreamingRecognizer recognizer;
    public AudioClip dialogueAudioClip; // The audio clip to play initially
    public AudioClip responseAudioClip; // The audio clip to play after correct response
    private AudioSource audioSource; // AudioSource to play the audio

    void Start()
    {
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

        // Initialize the AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
        if (dialogueAudioClip != null)
        {
            audioSource.clip = dialogueAudioClip;
        }
        else
        {
            Debug.LogError("No initial audio clip assigned!");
        }

        recognizer = FindObjectOfType<StreamingRecognizer>();
        if (recognizer == null)
        {
            Debug.LogError("StreamingRecognizer component not found!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger area.");
            if (dialogManager != null)
            {
                dialogueText.text = "Hey Mario, How are you?\n Say:\n I'm fine thank you, how are you?";
                dialogManager.ShowDialog();

                if (dialogueAudioClip != null && audioSource != null)
                {
                    audioSource.clip = dialogueAudioClip;
                    audioSource.Play(); // Play the initial audio clip
                }
                else
                {
                    Debug.LogError("Initial audio clip or audio source is missing!");
                }

                if (recognizer != null)
                {
                    recognizer.onFinalResult.AddListener(OnSpeechRecognized);
                    recognizer.StartListening();
                }
                else
                {
                    Debug.LogError("Recognizer is null when Player enters trigger area.");
                }
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

        if (dialogueText != null && text.Trim().ToLower() == "i'm fine thank you how are you")
        {
            Debug.Log("Correct speech recognized.");

            dialogueText.color = Color.red;

            // Play the response audio clip and hide the dialog after it finishes
            if (responseAudioClip != null && audioSource != null)
            {
                Debug.Log("Playing response audio clip.");
                audioSource.clip = responseAudioClip;
                audioSource.Play();
                StartCoroutine(HideDialogAfterAudio());
            }
            else
            {
                Debug.LogError("Response audio clip or audio source is missing!");
            }
        }
        else if (dialogueText == null)
        {
            Debug.LogError("dialogueText is null in OnSpeechRecognized.");
        }
        else
        {
            Debug.Log("Speech did not match expected response.");
        }
    }

    IEnumerator HideDialogAfterAudio()
    {
        // Wait until the audio clip finishes playing
        Debug.Log("Waiting for audio clip to finish playing.");
        yield return new WaitWhile(() => audioSource.isPlaying);

        if (dialogManager != null)
        {
            Debug.Log("Hiding dialog panel.");
            dialogManager.HideDialogPanel();
        }
        else
        {
            Debug.LogError("DialogManager is null in HideDialogAfterAudio.");
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
