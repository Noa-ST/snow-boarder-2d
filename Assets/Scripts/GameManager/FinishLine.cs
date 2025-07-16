using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private ParticleSystem finishEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Utils.PlayerTag))
        {
            finishEffect.Play();
            if (StarManager.Ins != null)
            {
                StarManager.Ins.SaveTotalStars();
            }
            LevelUIManager.Ins?.ShowGameWinPanel();
            int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
            int nextLevelIndex = currentLevelIndex + 1;
            if (nextLevelIndex <= 5) // Giới hạn ở Level 5
            {
                PlayerPrefs.SetInt("LevelUnlocked_" + nextLevelIndex, 1);
                PlayerPrefs.Save(); // Đảm bảo lưu ngay lập tức
                Debug.Log("Level " + nextLevelIndex + " unlocked!");
            }
        }
    }
}