using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleCloudStreamingSpeechToText;
using TMPro;
using System.Threading;

public class CopScript : MonoBehaviour
{
    public TMP_Text dialogueText; // Reference TMP_Text dialogueText; Assign this in the Unity Editor
    public PointCounter pointCounter;
    public DialogManagerCopSCript dialogManager;
    private StreamingRecognizer recognizer;
    public AudioClip dialogueAudioClip; // The audio clip to play initially
    public AudioClip responseAudioCop; // The audio clip to play after correct response
    public AudioClip notSuccessResponseAudioClipCop; // Audio clip for incorrect response
    private AudioSource audioSource; // AudioSource to play the audio
    public ChangImage changeImage;
    public Image image;
    private bool passedAlready = false;

    void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Find the DoctorText object directly
        GameObject DoctorsText = GameObject.Find("CopText");
        if (DoctorsText != null)
        {
            // Get the TMP_Text component from the BikeRiderText object
            dialogueText = DoctorsText.GetComponent<TMP_Text>();
            if (dialogueText != null)
            {
                Debug.Log("Text component found and assigned successfully.");
            }
            else
            {
                Debug.LogError("TMP_Text component not found on GameObject with name 'CopText'.");
            }
        }
        else
        {
            Debug.LogError("GameObject with name 'CopText' not found in the scene.");
        }

        dialogManager = FindObjectOfType<DialogManagerCopSCript>();
        if (dialogManager == null)
        {
            Debug.LogError("DialogManager not found in the scene!");
        }
        else
        {
            dialogManager.HideDialogPanel(); // Hide the dialog panel initially
        }

        // Initialize the AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
        if (dialogueAudioClip != null)
        {
            audioSource.clip = dialogueAudioClip;
        }
        else
        {
            Debug.LogError("No initial audio source found or added!");
        }

        recognizer = FindObjectOfType<StreamingRecognizer>();
        if (recognizer == null)
        {
            Debug.LogError("StreamingRecognizer component not found!");
        }
        if (other.CompareTag("Player") && !passedAlready)
        {
            Debug.Log("Player entered trigger area.");
            if (dialogManager != null)
            {
                dialogueText.text = "Hi Mario, what do you want to report??\n\n Say: My wallet was stolen, I need help";
                dialogueText.fontSize = 30;
                dialogManager.ShowDialog();

                if (dialogueAudioClip != null && audioSource != null)
                {
                    audioSource.clip = dialogueAudioClip;
                    audioSource.Play(); // Play the initial audio clip
                    StartCoroutine(StartListeningAfterAudio());
                }
                else
                {
                    Debug.LogError("Initial audio clip or audio source is missing!");
                }
            }
            else
            {
                Debug.LogError("DialogManager is null when Player enters trigger area.");
            }
        }
    }

    IEnumerator StartListeningAfterAudio()
    {
        // Wait until the initial audio clip finishes playing
        yield return new WaitWhile(() => audioSource.isPlaying);

        if (recognizer != null)
        {
            changeImage.ChangeImageSpriteToRecord();
            recognizer.onFinalResult.AddListener(OnSpeechRecognized);
            recognizer.StartListening();
        }
        else
        {
            Debug.LogError("Recognizer is null when trying to start listening.");
        }
    }

    void OnSpeechRecognized(string text)
    {
        Debug.Log("Speech Recognized: " + text);

        int percentAccuracyInt = LogicUtils.CalculateAccuracyPercentage("my wallet was stolen, I need help", text);

        if (dialogueText != null && percentAccuracyInt > 90)
        {
            Debug.Log("Correct speech recognized.");
            passedAlready = true;
            dialogueText.text = "You said it perfectly!";
            dialogueText.color = Color.green;
            pointCounter.UpdateCoin(5);

            // Play the response audio clip and hide the dialog after it finishes
            if (responseAudioCop != null && audioSource != null)
            {
                Debug.Log("Playing response audio clip.");
                audioSource.clip = responseAudioCop;
                audioSource.Play();
                StartCoroutine(HideDialogAfterAudio());
            }
            else
            {
                Debug.LogError("Response audio clip or audio source is missing!");
            }
        }
        else
        {
            dialogueText.text = $"Your Score: {percentAccuracyInt}%";
            Debug.Log($"Speech did not match expected response: {text}.");
            Debug.Log("Playing not successful response audio clip.");

            // Set the unsuccessful response audio clip and play it
            if (notSuccessResponseAudioClipCop != null && audioSource != null)
            {
                audioSource.clip = notSuccessResponseAudioClipCop;
                audioSource.Play();
                StartCoroutine(HideDialogAfterAudio());
                pointCounter.UpdateCoin(-1);
            }
            else
            {
                Debug.LogError("Unsuccessful response audio clip or audio source is missing!");
            }
        }

        changeImage.ChangeImageSpriteToNotRecord();
        recognizer.StopListening();
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
            OnDestroy();
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
            changeImage.ChangeImageSpriteToNotRecord();
            recognizer.StopListening();
            recognizer.onFinalResult.RemoveListener(OnSpeechRecognized);
        }
    }
}
