using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StarManager : Singleton<StarManager>
{
    private int currentStarsInLevel = 0; // Số sao trong level hiện tại
    [SerializeField] private TextMeshProUGUI starText; // Text UI để hiển thị số sao

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
        // Reset số sao khi bắt đầu level mới
        ResetLevelStars();
        // Làm mới tham chiếu starText
        RefreshStarText();
        UpdateStarUI();
        Debug.Log("StarManager refreshed in scene: " + scene.name + " - starText: " + (starText != null ? "Found" : "Null"));
    }

    public override void Start()
    {
        base.Start();
        RefreshStarText();
        UpdateStarUI(); // Cập nhật UI khi bắt đầu
    }

    private void RefreshStarText()
    {
        if (starText == null)
        {
            // Thử tìm lại starText với nhiều cách
            starText = GameObject.Find("StarText")?.GetComponent<TextMeshProUGUI>();
            if (starText == null)
            {
                // Thử tìm trong Canvas
                Canvas[] canvases = FindObjectsOfType<Canvas>();
                foreach (Canvas canvas in canvases)
                {
                    starText = canvas.transform.Find("StarText")?.GetComponent<TextMeshProUGUI>();
                    if (starText != null) break;
                }
            }
            if (starText == null)
            {
                Debug.LogWarning("starText not found in scene: " + SceneManager.GetActiveScene().name + ". Please ensure a TextMeshProUGUI named 'StarText' exists.");
            }
        }
    }

    public void AddStars(int amount)
    {
        currentStarsInLevel += amount;
        UpdateStarUI();
    }

    public void SaveTotalStars()
    {
        Pref.Star = Pref.Star + currentStarsInLevel; // Cập nhật tổng số sao
        PlayerPrefs.Save(); // Lưu thủ công vào PlayerPrefs
        ResetLevelStars(); // Reset sau khi lưu
    }

    public void ResetLevelStars()
    {
        currentStarsInLevel = 0;
        UpdateStarUI();
    }

    private void UpdateStarUI()
    {
        if (starText != null)
        {
            starText.text = currentStarsInLevel.ToString();
        }
        else
        {
            Debug.LogError("starText is null or destroyed in scene: " + SceneManager.GetActiveScene().name + ". Attempting to refresh...");
            RefreshStarText(); // Thử tìm lại khi null
            if (starText != null)
            {
                starText.text = currentStarsInLevel.ToString();
                Debug.Log("starText refreshed successfully.");
            }
        }
    }

    public int GetCurrentStars()
    {
        return currentStarsInLevel;
    }

    public int GetTotalStars()
    {
        return Pref.Star;
    }
}