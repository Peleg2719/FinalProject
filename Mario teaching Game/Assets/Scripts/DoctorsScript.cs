using UnityEngine;
using UnityEngine.UI;
using GoogleCloudStreamingSpeechToText;
using TMPro;
using System.Collections;

public class DoctorsScript : MonoBehaviour
{
    public TMP_Text dialogueText;
    public DialogManagerDoctor dialogManager;
    public PointCounter pointCounter;
    private StreamingRecognizer recognizer;
    private StreamingRecognizerSpanish spanishRecognizer;
    public AudioClip[] dialogueAudioClips;
    public AudioClip[] responseAudioClips;
    public AudioClip notSuccessResponseAudioClipDoctor;
    public AudioClip notSuccessResponseAudioClipSpanish;
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

        spanishRecognizer = FindObjectOfType<StreamingRecognizerSpanish>();
        if (recognizer == null)
        {
            Debug.LogError("StreamingRecognizerSpanish component not found!");
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
        if (GameManager.Language == "en")
        {
            this.userLevel = UserManager.Instance.CurrentUser.levelEn;
        }
        else
        {
            this.userLevel = UserManager.Instance.CurrentUser.levelEs;
        }
        if (other.CompareTag("Player") && this.passedAlready == false)
        {
            GameManager.IsGamePaused = true;
            GameManager.StopGame();
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
            if (GameManager.Language == "en")
            {
                yield return StartCoroutine(firebaseManager.GetQuestionData("question_3", OnQuestionDataReceived));
            }
            else
            {
                yield return StartCoroutine(firebaseManager.GetQuestionData("question_3_es", OnQuestionDataReceived));
            }
        }
        else if (this.userLevel == 2)
        {
            if (GameManager.Language == "en")
            {
                yield return StartCoroutine(firebaseManager.GetQuestionData("question_3_level_2", OnQuestionDataReceived));
            }
            else
            {
                yield return StartCoroutine(firebaseManager.GetQuestionData("question_3_level_2_es", OnQuestionDataReceived));
            }
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
                if (GameManager.Language == "en")
                {
                    if (this.userLevel == 1)
                    {
                        audioSource.clip = dialogueAudioClips[0];
                    }
                    else
                    {
                        audioSource.clip = dialogueAudioClips[1];
                    }
                }
                else
                {
                    if (this.userLevel == 1)
                    {
                        audioSource.clip = dialogueAudioClips[2];
                    }
                    else
                    {
                        audioSource.clip = dialogueAudioClips[3];
                    }
                }
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

        changeImage.ChangeImageSpriteToRecord();

        if (GameManager.Language == "en")
        {
            if (recognizer != null)
            {
                recognizer.onFinalResult.AddListener(OnSpeechRecognized);
                recognizer.StartListening();
            }
            else
            {
                Debug.LogError("English Recognizer is null when trying to start listening.");
            }
        }
        else if (GameManager.Language == "es")
        {
            if (spanishRecognizer != null)
            {
                spanishRecognizer.onFinalResult.AddListener(OnSpeechRecognized);
                spanishRecognizer.StartListening();
            }
            else
            {
                Debug.LogError("Spanish Recognizer is null when trying to start listening.");
            }
        }
    }

    void OnSpeechRecognized(string text)
    {
        Debug.Log("Speech Recognized: " + text);

        int percentAccuracyInt = LogicUtils.CalculateAccuracyPercentage(expectedAnswer, text);
        if (dialogueText != null && percentAccuracyInt >= 80)
        {
            this.passedAlready = true;
            if (GameManager.Language == "en")
            {
                dialogueText.text = "You said it perfectly!";
            }
            else if (GameManager.Language == "es")
            {
                dialogueText.text = "�Lo dijiste perfectamente!";
            }
            dialogueText.color = Color.green;
            pointCounter.UpdateCoin(5);

            // Select response audio clip based on user level
            if (audioSource != null)
            {
                Debug.Log("Playing response audio clip.");
                if (GameManager.Language == "en")
                {
                    if (this.userLevel == 1)
                    {
                        audioSource.clip = responseAudioClips[0];
                    }
                    else
                    {
                        audioSource.clip = responseAudioClips[1];
                    }
                }
                else
                {
                    audioSource.clip = responseAudioClips[2];
                }
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
            if (GameManager.Language == "en")
            {
                dialogueText.text = $"Your Score: {percentAccuracyInt}%";
            }
            else if (GameManager.Language == "es")
            {
                dialogueText.text = $"Tu Puntaje: {percentAccuracyInt}%";
            }

            if (notSuccessResponseAudioClipDoctor != null && audioSource != null)
            {
                if (GameManager.Language == "en")
                {
                    audioSource.clip = notSuccessResponseAudioClipDoctor;
                }
                else if (GameManager.Language == "es")
                {
                    audioSource.clip = notSuccessResponseAudioClipSpanish;
                }
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
        if (GameManager.Language == "en")
        {
            recognizer.StopListening();
        }
        else
        {
            spanishRecognizer.StopListening();
        }
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
        changeImage.ChangeImageSpriteToNotRecord();

        if (recognizer != null)
        {
            recognizer.StopListening();
            recognizer.onFinalResult.RemoveListener(OnSpeechRecognized);
        }

        if (spanishRecognizer != null)
        {
            spanishRecognizer.StopListening();
            spanishRecognizer.onFinalResult.RemoveListener(OnSpeechRecognized);
        }
    }
}