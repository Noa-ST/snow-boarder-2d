using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelUIManager : Singleton<LevelUIManager>
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameWinPanel;
    [SerializeField] private GameObject gamePausePanel;

    [SerializeField] private Button againButtonGameOver;
    [SerializeField] private Button homeButtonGameOver;
    [SerializeField] private Button againButtonGameWin;
    [SerializeField] private Button homeButtonGameWin;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button againButtonPause;
    [SerializeField] private Button homeButtonPause;

    private bool isPaused = false;
    private int currentLevelIndex;

    public override void Awake()
    {
        MakeSingleton(false);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentLevelIndex = scene.buildIndex;
        RefreshReferences();
        InitializeUI();
        Debug.Log("Scene loaded: " + scene.name + ", gameOverPanel: " + (gameOverPanel != null ? "Found" : "Null"));
    }

    public override void Start()
    {
        base.Start();
        // Chỉ gọi InitializeUI nếu chưa được gọi trong OnSceneLoaded
        if (!gameOverPanel.activeSelf && !gameWinPanel.activeSelf && !gamePausePanel.activeSelf)
        {
            InitializeUI();
        }
    }

    private void RefreshReferences()
    {
        if (gameOverPanel == null)
        {
            gameOverPanel = GameObject.Find("GameOverPanel");
            if (gameOverPanel == null)
            {
                Canvas[] canvases = FindObjectsOfType<Canvas>();
                foreach (Canvas canvas in canvases)
                {
                    gameOverPanel = canvas.transform.Find("GameOverPanel")?.gameObject;
                    if (gameOverPanel != null) break;
                }
            }
        }
        if (gameWinPanel == null)
        {
            gameWinPanel = GameObject.Find("GameWinPanel");
            if (gameWinPanel == null)
            {
                Canvas[] canvases = FindObjectsOfType<Canvas>();
                foreach (Canvas canvas in canvases)
                {
                    gameWinPanel = canvas.transform.Find("GameWinPanel")?.gameObject;
                    if (gameWinPanel != null) break;
                }
            }
        }
        if (gamePausePanel == null)
        {
            gamePausePanel = GameObject.Find("GamePausePanel");
            if (gamePausePanel == null)
            {
                Canvas[] canvases = FindObjectsOfType<Canvas>();
                foreach (Canvas canvas in canvases)
                {
                    gamePausePanel = canvas.transform.Find("GamePausePanel")?.gameObject;
                    if (gamePausePanel != null) break;
                }
            }
        }

        if (againButtonGameOver == null) againButtonGameOver = GameObject.Find("AgainButtonGameOver")?.GetComponent<Button>();
        if (homeButtonGameOver == null) homeButtonGameOver = GameObject.Find("HomeButtonGameOver")?.GetComponent<Button>();
        if (againButtonGameWin == null) againButtonGameWin = GameObject.Find("AgainButtonGameWin")?.GetComponent<Button>();
        if (homeButtonGameWin == null) homeButtonGameWin = GameObject.Find("HomeButtonGameWin")?.GetComponent<Button>();
        if (nextLevelButton == null) nextLevelButton = GameObject.Find("NextLevelButton")?.GetComponent<Button>();
        if (againButtonPause == null) againButtonPause = GameObject.Find("AgainButtonPause")?.GetComponent<Button>();
        if (homeButtonPause == null) homeButtonPause = GameObject.Find("HomeButtonPause")?.GetComponent<Button>();

        Debug.Log("Refreshed references - gameOverPanel: " + (gameOverPanel != null ? "Found" : "Null") +
                  ", againButtonGameOver: " + (againButtonGameOver != null ? "Found" : "Null") +
                  ", homeButtonGameOver: " + (homeButtonGameOver != null ? "Found" : "Null") +
                  ", againButtonGameWin: " + (againButtonGameWin != null ? "Found" : "Null") +
                  ", homeButtonGameWin: " + (homeButtonGameWin != null ? "Found" : "Null") +
                  ", nextLevelButton: " + (nextLevelButton != null ? "Found" : "Null") +
                  ", againButtonPause: " + (againButtonPause != null ? "Found" : "Null") +
                  ", homeButtonPause: " + (homeButtonPause != null ? "Found" : "Null"));
    }

    private void InitializeUI()
    {
        HideAllPanels();

        if (againButtonGameOver != null)
        {
            againButtonGameOver.onClick.RemoveAllListeners();
            againButtonGameOver.onClick.AddListener(() =>
            {
                AudioController.Ins.PlayButtonClickSound();
                ReloadCurrentLevel();
            });
            Debug.Log("Attached ReloadCurrentLevel to againButtonGameOver.");
        }
        else
        {
            Debug.LogError("againButtonGameOver is null!");
        }
        if (homeButtonGameOver != null)
        {
            homeButtonGameOver.onClick.RemoveAllListeners();
            homeButtonGameOver.onClick.AddListener(() =>
            {
                AudioController.Ins.PlayButtonClickSound();
                LoadMainMenu();
            });
            Debug.Log("Attached LoadMainMenu to homeButtonGameOver.");
        }
        else
        {
            Debug.LogError("homeButtonGameOver is null!");
        }
        if (againButtonGameWin != null)
        {
            againButtonGameWin.onClick.RemoveAllListeners();
            againButtonGameWin.onClick.AddListener(() =>
            {
                AudioController.Ins.PlayButtonClickSound();
                ReloadCurrentLevel();
            });
            Debug.Log("Attached ReloadCurrentLevel to againButtonGameWin.");
        }
        else
        {
            Debug.LogError("againButtonGameWin is null!");
        }
        if (homeButtonGameWin != null)
        {
            homeButtonGameWin.onClick.RemoveAllListeners();
            homeButtonGameWin.onClick.AddListener(() =>
            {
                AudioController.Ins.PlayButtonClickSound();
                LoadMainMenu();
            });
            Debug.Log("Attached LoadMainMenu to homeButtonGameWin.");
        }
        else
        {
            Debug.LogError("homeButtonGameWin is null!");
        }
        if (nextLevelButton != null && currentLevelIndex < 5)
        {
            nextLevelButton.onClick.RemoveAllListeners();
            nextLevelButton.onClick.AddListener(() =>
            {
                AudioController.Ins.PlayButtonClickSound();
                LoadNextLevel();
            });
            Debug.Log("Attached LoadNextLevel to nextLevelButton.");
        }
        else if (nextLevelButton == null)
        {
            Debug.LogError("nextLevelButton is null!");
        }
        if (againButtonPause != null)
        {
            againButtonPause.onClick.RemoveAllListeners();
            againButtonPause.onClick.AddListener(() =>
            {
                AudioController.Ins.PlayButtonClickSound();
                ResumeGame();
            });
            Debug.Log("Attached ResumeGame to againButtonPause.");
        }
        else
        {
            Debug.LogError("againButtonPause is null!");
        }
        if (homeButtonPause != null)
        {
            homeButtonPause.onClick.RemoveAllListeners();
            homeButtonPause.onClick.AddListener(() =>
            {
                AudioController.Ins.PlayButtonClickSound();
                LoadMainMenu();
            });
            Debug.Log("Attached LoadMainMenu to homeButtonPause.");
        }
        else
        {
            Debug.LogError("homeButtonPause is null!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void ShowGameOverPanel()
    {
        if (gameOverPanel != null)
        {
            HideAllPanels();
            gameOverPanel.SetActive(true);
            AudioController.Ins.PlayGameOverSound();
            Time.timeScale = 0f;
            Debug.Log("GameOverPanel displayed successfully.");
        }
        else
        {
            Debug.LogError("gameOverPanel is null or destroyed in scene: " + SceneManager.GetActiveScene().name);
        }
    }

    public void ShowGameWinPanel()
    {
        if (gameWinPanel != null)
        {
            HideAllPanels();
            gameWinPanel.SetActive(true);
            AudioController.Ins.PlayGameWinSound();
            Time.timeScale = 0f;
            Debug.Log("GameWinPanel displayed successfully.");
        }
        else
        {
            Debug.LogError("gameWinPanel is null or destroyed!");
        }
    }

    private void TogglePause()
    {
        if (gamePausePanel != null)
        {
            isPaused = !isPaused;
            gamePausePanel.SetActive(isPaused);
            if (isPaused)
            {
                AudioController.Ins.PlayPauseSound();
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
            Debug.Log("Toggled pause, isPaused: " + isPaused + ", Time.timeScale: " + Time.timeScale);
        }
        else
        {
            Debug.LogError("gamePausePanel is null or destroyed!");
        }
    }

    private void ResumeGame()
    {
        TogglePause();
    }

    private void HideAllPanels()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (gameWinPanel != null) gameWinPanel.SetActive(false);
        if (gamePausePanel != null) gamePausePanel.SetActive(false);
    }

    private void ReloadCurrentLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(currentLevelIndex);
        Debug.Log("Reloading level: " + currentLevelIndex);
    }

    private void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Loading MainMenu");
    }

    private void LoadNextLevel()
    {
        int nextLevelIndex = currentLevelIndex + 1;
        if (nextLevelIndex <= 5)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Level" + nextLevelIndex);
            Debug.Log("Loading next level: Level" + nextLevelIndex);
        }
        else
        {
            Debug.LogWarning("No next level available!");
            LoadMainMenu();
        }
    }
}