using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject homePanel;          // Panel chứa các nút Home
    [SerializeField] private GameObject menuLevelSelectPanel; // Panel chọn level
    [SerializeField] private GameObject settingPanel;        // Panel setting
    [SerializeField] private GameObject infoPanel;          // Panel info
    [SerializeField] private Button playButton;             // Nút Play
    [SerializeField] private Button settingButton;          // Nút Setting
    [SerializeField] private Button infoButton;             // Nút Info
    [SerializeField] private Button[] levelButtons;         // Mảng các nút level
    [SerializeField] private Button returnButtonSetting;    // Nút Return trong Setting
    [SerializeField] private Button returnButtonInfo;       // Nút Return trong Info
    [SerializeField] private TextMeshProUGUI totalStarText; // Text để hiển thị tổng số sao
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private Sprite lockedSprite;
    [SerializeField] private Color lockedColor;

    public override void Awake()
    {
        MakeSingleton(false);
    }

    public override void Start()
    {
        // Gán sự kiện cho các nút
        playButton.onClick.AddListener(ShowLevelSelect);
        settingButton.onClick.AddListener(ShowSettingPanel);
        infoButton.onClick.AddListener(ShowInfoPanel);

        returnButtonSetting.onClick.AddListener(HideAllPanelsAndShowHome);
        returnButtonInfo.onClick.AddListener(HideAllPanelsAndShowHome);

        // Gán sự kiện cho các nút level
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i + 1; // Index level bắt đầu từ 1
            levelButtons[i].onClick.AddListener(() => LoadLevel(levelIndex));
        }

        // Ẩn tất cả panel khi bắt đầu (trừ home)
        HideAllPanels();
        homePanel.SetActive(true); // Đảm bảo HomePanel hiển thị ban đầu
        UpdateTotalStarText(); // Cập nhật tổng số sao khi bắt đầu
        RefreshLevelButtons(); // Làm mới trạng thái button khi mở menu
    }

    private void ShowLevelSelect()
    {
        HideAllPanels();
        homePanel.SetActive(false); // Ẩn HomePanel
        menuLevelSelectPanel.SetActive(true);
        PlayButtonClickSound();
    }

    private void ShowSettingPanel()
    {
        HideAllPanels();
        homePanel.SetActive(false); // Ẩn HomePanel
        settingPanel.SetActive(true);
        PlayButtonClickSound();
    }

    private void ShowInfoPanel()
    {
        HideAllPanels();
        homePanel.SetActive(false); // Ẩn HomePanel
        infoPanel.SetActive(true);
        UpdateInfoText();
        PlayButtonClickSound();
    }

    private void HideAllPanels()
    {
        menuLevelSelectPanel.SetActive(false);
        settingPanel.SetActive(false);
        infoPanel.SetActive(false);
    }

    private void HideAllPanelsAndShowHome()
    {
        HideAllPanels();
        homePanel.SetActive(true); // Hiển thị lại HomePanel khi nhấn Return
        PlayButtonClickSound();
    }

    private void LoadLevel(int levelIndex)
    {
        if (IsLevelUnlocked(levelIndex) && levelIndex <= SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene("Level" + levelIndex);
            PlayButtonClickSound(); // Thêm âm thanh khi nhấn
        }
        else
        {
            Debug.LogWarning("Level " + levelIndex + " is locked or not found in Build Settings!");
        }
    }

    private bool IsLevelUnlocked(int levelIndex)
    {
        // Level 1 luôn mở khóa
        if (levelIndex == 1) return true;
        // Kiểm tra trạng thái mở khóa từ PlayerPrefs
        return PlayerPrefs.GetInt("LevelUnlocked_" + levelIndex, 0) == 1;
    }

    private void UpdateLevelButtonState(int levelIndex, Button button)
    {
        if (!IsLevelUnlocked(levelIndex))
        {
            button.interactable = false; // Vô hiệu hóa button
            if (lockedSprite != null) button.GetComponent<Image>().sprite = lockedSprite; // Thêm hình khóa
            button.GetComponent<Image>().color = lockedColor; // Màu mờ
        }
        else
        {
            button.interactable = true; // Kích hoạt button
            if (lockedSprite != null) button.GetComponent<Image>().sprite = null; // Xóa hình khóa
            button.GetComponent<Image>().color = Color.white; // Màu bình thường
        }
    }

    private void RefreshLevelButtons()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i + 1;
            UpdateLevelButtonState(levelIndex, levelButtons[i]);
        }
    }

    private void UpdateTotalStarText()
    {
        if (totalStarText != null)
        {
            totalStarText.text = Pref.Star.ToString(); // Hiển thị tổng số sao từ Pref
        }
        else
        {
            Debug.LogError("totalStarText is not assigned in Inspector!");
        }
    }

    private void UpdateInfoText()
    {
        if (infoText != null)
        {
            infoText.text = "=== Welcome to Snow Boarding ===\n" +
                           "Giới thiệu: Star Jumper là một game phiêu lưu nơi bạn điều khiển nhân vật vượt qua các cấp độ để thu thập sao và mở khóa cấp độ mới!\n" +
                           "Cách chơi:\n" +
                           "- Sử dụng phím mũi tên hoặc A/D để di chuyển.\n" +
                           "- Thu thập sao để tăng điểm số (hiển thị trên màn hình).\n" +
                           "- Nhặt powerup tăng tốc và bất tử để tăng khả năng qua màn." +
                           "- Tránh va chạm với mặt đất để không thua!\n" +
                           "- Hoàn thành cấp độ để mở khóa cấp độ tiếp theo.\n" +
                           "Cài đặt: Vào menu Setting để điều chỉnh âm thanh và nhạc nền.\n" +
                           "Tạm dừng: Nhấn Esc để tạm dừng hoặc tiếp tục game.\n" +
                           "Chúc bạn chơi game vui vẻ!";
        }
        else
        {
            Debug.LogError("infoText is not assigned in Inspector!");
        }
    }

    private void PlayButtonClickSound()
    {
        if (AudioController.Ins != null)
        {
            AudioController.Ins.PlayButtonClickSound();
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}