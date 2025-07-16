using UnityEngine;

public class Snowflake : MonoBehaviour
{
    [SerializeField] private int points = 1; // Số sao khi nhặt

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Utils.PlayerTag))
        {
            if (StarManager.Ins != null)
            {
                StarManager.Ins.AddStars(points); // Tăng số sao tạm thời
            }
            AudioController.Ins.PlayCollectableSound();
            Destroy(gameObject); // Xóa star sau khi nhặt
        }
    }
}