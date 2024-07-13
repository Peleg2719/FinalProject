using UnityEngine;
using UnityEngine.UI;
using GoogleCloudStreamingSpeechToText;
using TMPro;
using System.Collections;

public class CoffeSellerScript : MonoBehaviour
{
    public TMP_Text dialogueText;
    public DialogManagerCoffeSeller dialogManager;
    public PointCounter pointCounter;
    private StreamingRecognizer recognizer;
    public AudioClip dialogueAudioClip;
    public AudioClip responseAudioCoffeSeller;
    public AudioClip notSuccessResponseAudioClipCoffeSeller;
    private AudioSource audioSource;
    public ChangImage changeImage;
    public Image image;
    private bool passedAlready = false;
    private FirebaseManager firebaseManager;
    private string expectedAnswer;

    void Start()
    {
        firebaseManager = FindObjectOfType<FirebaseManager>();
        if (firebaseManager == null)
        {
            Debug.LogError("FirebaseManager not found in the scene!");
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        if (dialogueAudioClip == null)
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
        if (other.CompareTag("Player") && !passedAlready)
        {
            Debug.Log("Player entered trigger area.");
            if (dialogManager != null && firebaseManager != null)
            {
                StartCoroutine(FetchQuestionData());
            }
            else
            {
                Debug.LogError("DialogManager or FirebaseManager is null when Player enters trigger area.");
            }
        }
    }

    private IEnumerator FetchQuestionData()
    {
        yield return StartCoroutine(firebaseManager.GetQuestionData("question_5", OnQuestionDataReceived));
    }

    private void OnQuestionDataReceived(QuestionData questionData)
    {
        if (questionData != null)
        {
            dialogueText.text = questionData.question + "\n\nSay:\n" + questionData.answer;
            dialogueText.fontSize = 30;
            expectedAnswer = questionData.answer;

            dialogManager.ShowDialog();

            if (dialogueAudioClip != null && audioSource != null)
            {
                audioSource.clip = dialogueAudioClip;
                audioSource.Play();
                StartCoroutine(StartListeningAfterAudio());
            }
            else
            {
                Debug.LogError("Initial audio clip or audio source is missing!");
            }
        }
        else
        {
            Debug.LogError("Failed to retrieve question data from Firebase.");
        }
    }

    IEnumerator StartListeningAfterAudio()
    {
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

        int percentAccuracyInt = LogicUtils.CalculateAccuracyPercentage(expectedAnswer, text);
        if (dialogueText != null && percentAccuracyInt > 80)
        {
            Debug.Log("Correct speech recognized.");
            passedAlready = true;
            dialogueText.text = "You said it perfectly!";
            dialogueText.color = Color.green;
            pointCounter.UpdateCoin(5);

            if (responseAudioCoffeSeller != null && audioSource != null)
            {
                Debug.Log("Playing response audio clip.");
                audioSource.clip = responseAudioCoffeSeller;
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

            if (notSuccessResponseAudioClipCoffeSeller != null && audioSource != null)
            {
                audioSource.clip = notSuccessResponseAudioClipCoffeSeller;
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
        Debug.Log("Waiting for audio clip to finish playing.");
        yield return new WaitWhile(() => audioSource.isPlaying);

        if (dialogManager != null)
        {
            Debug.Log("Hiding dialog panel.");
            dialogManager.HideDialogPanel();
            GameManager.StartGame();
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