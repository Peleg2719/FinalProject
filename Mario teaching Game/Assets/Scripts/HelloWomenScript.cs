using UnityEngine;
using UnityEngine.UI;

public class HelloWomenScript : MonoBehaviour
{
    public GameObject canvas;
    public Text sentenceText;
    private DialogManager dialogManager;
    public string sentence = "The man touched the woman!";

    void Start()
    {
        dialogManager = FindObjectOfType<DialogManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialogManager.ShowDialog();
        }
    }
    

    // void OnTriggerExit2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         dialogManager.HideDialogPanel();
    //     }
    // }
}