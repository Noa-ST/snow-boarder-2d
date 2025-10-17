using TMPro;
using UnityEngine;
using UnityEngine.UI; // Cần thiết để sử dụng component Text

public class SpeedDisplay : MonoBehaviour
{
    // 1. Kéo và thả Text UI Component vào đây trong Inspector
    [SerializeField] private TextMeshProUGUI speedText;

    // 2. Tham chiếu đến PlayerController trong Scene
    private PlayerController playerController;

    private void Start() 
    {
        // TÌM: Tìm instance của PlayerController trong Scene
        playerController = FindObjectOfType<PlayerController>();

        if (speedText == null)
        {
            Debug.LogError("Speed Text UI is not assigned. Please drag the Text component onto the 'Speed Text' field in the Inspector.");
        }

        if (playerController == null)
        {
            Debug.LogError("PlayerController not found in the scene.");
        }
    }

    private void Update()
    {
        if (playerController != null && speedText != null)
        {
            // Lấy tốc độ hiện tại từ PlayerController
            float currentSpeed = playerController.CurrentSpeed;

            // Hiển thị tốc độ lên UI. Sử dụng "F1" để làm tròn đến 1 chữ số thập phân
            // Bạn có thể tùy chỉnh chuỗi hiển thị theo ý muốn
            speedText.text = "SPEED: " + currentSpeed.ToString("F1") + " km/h";
        }
    }
}