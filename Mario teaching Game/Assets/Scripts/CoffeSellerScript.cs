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
    public AudioClip[] dialogueAudioClips;
    public AudioClip[] responseAudioClips;
    public AudioClip notSuccessResponseAudioClipCoffeSeller;
    private AudioSource audioSource;
    public ChangImage changeImage;
    public Image image;
    private bool passedAlready = false;
    private FirebaseManager firebaseManager;
    private string expectedAnswer;
    private GameManager gameManager;
    private int userLevel; // Default user level
    void Start()
    {
        firebaseManager = FindObjectOfType<FirebaseManager>();
        if (firebaseManager == null)
        {
            Debug.LogError("FirebaseManager not found in the scene!");
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        if (dialogueAudioClips == null)
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
        // Check if UserManager.Instance exists and has CurrentUser data
        this.userLevel = UserManager.Instance.CurrentUser.levelEn;
        if (other.CompareTag("Player") && !passedAlready)
        {
            GameManager.IsGamePaused = true;
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
        // Select audio clip based on user level
        if (this.userLevel == 1)
        {
            yield return StartCoroutine(firebaseManager.GetQuestionData("question_5", OnQuestionDataReceived));
        }
        else if (this.userLevel == 2)
        {
            yield return StartCoroutine(firebaseManager.GetQuestionData("question_5_level_2", OnQuestionDataReceived));
        }
    }

    private void OnQuestionDataReceived(QuestionData questionData)
    {
        if (questionData != null)
        {
            dialogueText.text = questionData.question + "\n\nSay:\n" + questionData.answer;
            dialogueText.fontSize = 30;
            expectedAnswer = questionData.answer;

            dialogManager.ShowDialog();

            // Select audio clip based on user level
            if (audioSource != null)
            {
                audioSource.clip = dialogueAudioClips[this.userLevel - 1];
                audioSource.Play();
                StartCoroutine(StartListeningAfterAudio());
            }
            else
            {
                Debug.LogError("Appropriate audio clip or audio source is missing for the current level!");
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
        if (dialogueText != null && percentAccuracyInt >= 80)
        {
            Debug.Log("Correct speech recognized.");
            passedAlready = true;
            dialogueText.text = "You said it perfectly!";
            dialogueText.color = Color.green;
            pointCounter.UpdateCoin(5);

            // Select response audio clip based on user level
            if (this.userLevel <= responseAudioClips.Length && audioSource != null)
            {
                Debug.Log("Playing response audio clip.");
                audioSource.clip = responseAudioClips[this.userLevel - 1];
                audioSource.Play();
                GameManager.IsGamePaused = false; // Resume the game
                StartCoroutine(HideDialogAfterAudio());

            }
            else
            {
                Debug.LogError("Response audio clip or audio source is missing for the current level!");
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