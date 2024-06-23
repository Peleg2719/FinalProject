using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Camera mainCamera;
    public Vector3 viewportPosition = new Vector3(456f, 195f, 0); // Set to top middle by default

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // Set the initial position of the microphone to the top middle of the screen
        SetPositionAtTopMiddle();
    }

    void LateUpdate()
    {
        // Keep the microphone at the top middle of the screen as the camera moves
        SetPositionAtTopMiddle();
    }

    void SetPositionAtTopMiddle()
    {
        // Convert viewport position to world position
        Vector3 worldPoint = mainCamera.ViewportToWorldPoint(viewportPosition);
        //transform.position = new Vector3(worldPoint.x, worldPoint.y, transform.position.z);
       transform.position= viewportPosition;
    }
}
