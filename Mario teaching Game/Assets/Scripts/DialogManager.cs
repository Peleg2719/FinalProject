using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public Text dialogueText;
    public GameObject dialoguePanel;
    public Button continueButton;

    private bool isDialogActive = false;

    void Start()
    {
        continueButton.onClick.AddListener(ContinueGame);
        HideDialogPanel();
    }

    public void ShowDialog()
    {
        isDialogActive = true;
        dialoguePanel.SetActive(true);
        GameManager.isGamePaused = true; // Pause the game
    }

    public void HideDialogPanel()
    {
        isDialogActive = false;
        GameManager.isGamePaused = false; // Resume the game
        dialoguePanel.SetActive(false);
    }

    public void ContinueGame()
    {
        HideDialogPanel();
    }
}