using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashDetector : MonoBehaviour
{
    [SerializeField] private ParticleSystem crashEffect;
    private bool hasCrashed = false;

    // Khai báo và lấy PlayerController một lần duy nhất trong Start() là cách hiệu quả hơn
    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            // Đảm bảo có PlayerController để tránh lỗi Null Reference
            Debug.LogError("CrashDetector: Missing PlayerController on GameObject.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. LẤY PlayerController (nếu chưa lấy) và THỰC HIỆN KIỂM TRA BẤT TỬ
        if (playerController != null)
        {
            if (playerController.IsInvincible)
            {
                // Nếu đang bất tử, in thông báo và kết thúc hàm (ngăn không cho player chết)
                Debug.Log("Player is INVINCIBLE! Crash avoided.");
                return;
            }
        }

        // 2. Logic va chạm ban đầu chỉ được thực thi nếu KHÔNG bất tử
        if (collision.CompareTag(Utils.GroundTag) && !hasCrashed)
        {
            hasCrashed = true;

            // Sử dụng playerController đã lưu trong Start()
            if (playerController != null)
            {
                playerController.DisableControls();
            }

            crashEffect.Play();
            AudioController.Ins.PlayCrashSound();

            // Sử dụng Singleton để truy cập LevelUIManager
            if (LevelUIManager.Ins != null)
            {
                LevelUIManager.Ins.ShowGameOverPanel();
            }
            else
            {
                Debug.LogError("LevelUIManager Singleton not found in scene: " + SceneManager.GetActiveScene().name);
            }
        }
    }
}