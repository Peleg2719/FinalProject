using UnityEngine;
using GoogleCloudStreamingSpeechToText;

public class VoiceMovement : MonoBehaviour
{
    private StreamingRecognizer recognizer;
    private Rigidbody2D rb;
    private Vector2 velocity;
    public bool jumping { get; private set; }
    public bool grounded { get; private set; }

    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2f, 2f);
    public float moveSpeed = 5f;

    void Start()
    {
        recognizer = GetComponent<StreamingRecognizer>();
        recognizer.onFinalResult.AddListener(OnSpeechRecognized);
        recognizer.StartListening();

        rb = GetComponent<Rigidbody2D>();
    }

    void OnDestroy()
    {
        recognizer.StopListening();
        recognizer.onFinalResult.RemoveListener(OnSpeechRecognized);
    }

    void OnSpeechRecognized(string text)
    {
        // Print the recognized speech to the console
        Debug.Log("Speech Recognized: " + text);

        // Check if the recognized text is "hello"
        if (text.ToLower() == "hola")
        {
            // Move the car backward
            MoveCarBackward();
        }
    }

    void MoveCarBackward()
    {
        // Move the car backward by changing its velocity or position
        // Example: decrease the car's x position
        transform.position -= Vector3.right * moveSpeed;
    }
}
