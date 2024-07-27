using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static bool isGamePaused = true;
    public static bool IsGamePaused
    {
        get => isGamePaused;
        set
        {
            if (isGamePaused != value)
            {
                isGamePaused = value;
                OnChange();
            }
        }
    }

    public bool isOKButtonClicked = false;
    public int world { get; private set; }
    public int stage { get; private set; }
    public int lives { get; private set; }
    public int coins { get; private set; }
    public FirebaseManager firebaseManager;
    private static string _language = "en";
    public static string Language
    {
        get => _language;
        set
        {
            if (_language != value)
            {
                _language = value;
                OnLanguageChange();
            }
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        NewGame();

    }


    /*private void Update()
    {
        if (isGamePaused)
        {
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
        }
    }*/

    public static void OnChange()
    {
        if (isGamePaused)
        {
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
        }
    }
    private static void OnLanguageChange()
    {
        // Handle language change logic here
        Debug.Log($"Language changed to: {_language}");
    }

public static void StopGame()
    {

        Time.timeScale = 0f; // Pause the game

    }
    public static void StartGame()
    {
        Time.timeScale = 1f; // Pause the game
    }
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }


    public void NewGame()
    {
        lives = 3;
        coins = 0;
        LoadLevel(1, 1);
    }

    public void GameOver()
    {
        NewGame();
    }

    public void LoadLevel(int world, int stage)
    {
        this.world = world;
        this.stage = stage;
        SceneManager.LoadScene($"{world}-{stage}");
    }

    public void NextLevel()
    {
        LoadLevel(world, stage + 1);
    }

    public void ResetLevel(float delay)
    {
        Invoke(nameof(ResetLevel), delay);
    }

    public void ResetLevel()
    {
        lives--;

        if (lives > 0)
        {
            LoadLevel(world, stage);
        }
        else
        {
            GameOver();
        }
    }

    public void AddCoin()
    {
        coins++;

        if (coins == 100)
        {
            coins = 0;
            AddLife();
        }
    }

    public void AddLife()
    {
        lives++;
    }

    public void OnClickOk()
    {
        isOKButtonClicked = true;
        isGamePaused = false;
    }
}
