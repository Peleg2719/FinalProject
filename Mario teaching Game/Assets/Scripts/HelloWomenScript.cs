using UnityEngine;
using UnityEngine.UI;
using GoogleCloudStreamingSpeechToText;
using TMPro;
using System.Collections;
using System.Threading;

public class HelloWomenScript : MonoBehaviour
{
    public TMP_Text dialogueText; // Reference TMP_Text dialogueText; Assign this in the Unity Editor
    public DialogManager dialogManager;

    private StreamingRecognizer recognizer;
    public AudioClip dialogueAudioClip; // The audio clip to play initially
    public AudioClip responseAudioClip; // The audio clip to play after correct response
    public AudioClip notSuccessResponseAudioClip; // Audio clip for incorrect response
    private AudioSource audioSource; // AudioSource to play the audio
    public ChangImage changeImage;

    public Image image;
    private bool passedAlready = false;

    void Start()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
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
                Debug.LogError("Text component not found on GameObject with name 'HelloWomenText'.");
            }
        }
        else
        {
            Debug.LogError("GameObject with name 'HelloWomenText' not found in the scene.");
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
        if (other.CompareTag("Player") && !passedAlready)
        {
            Debug.Log("Player entered trigger area.");
            if (dialogManager != null)
            {
                dialogueText.text = "Hey Mario, How are you?\n\n Say:\n I'm fine thank you, how are you?";
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

        int percentAccuracyInt = LogicUtils.CalculateAccuracyPercentage("I'm fine thank you how are you", text);
        if (dialogueText != null && percentAccuracyInt > 90)
        {
            Debug.Log("Correct speech recognized.");
            passedAlready = true;
            dialogueText.text = "You said it perfectly!";
            dialogueText.color = Color.green;

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
        else
        {
            dialogueText.text = $"Your Score: {percentAccuracyInt}%";
            Debug.Log($"Speech did not match expected response: {text}.");
            Debug.Log("Playing not successful response audio clip.");

            // Set the unsuccessful response audio clip and play it
            if (notSuccessResponseAudioClip != null && audioSource != null)
            {
                audioSource.clip = notSuccessResponseAudioClip;
                audioSource.Play();
                StartCoroutine(HideDialogAfterAudio());
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