using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashDetector : MonoBehaviour
{
    [SerializeField] private ParticleSystem crashEffect;
    private bool hasCrashed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Utils.GroundTag) && !hasCrashed)
        {
            hasCrashed = true;
            GetComponent<PlayerController>().DisableControls();
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