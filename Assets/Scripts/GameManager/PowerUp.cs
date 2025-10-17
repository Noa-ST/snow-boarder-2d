using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { Invincibility, SpeedBoost }

    // Chọn loại Power-up trong Inspector
    public PowerUpType type;

    // Kéo và thả Particle System (ví dụ: hiệu ứng nhặt)
    [SerializeField] private GameObject collectEffect;

    // Đặt thời gian trước khi Power-up bị hủy
    [SerializeField] private float destroyDelay = 0.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Chỉ xử lý nếu va chạm với Player
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            // 1. Kích hoạt Power-up cho Player
            player.ActivatePowerUp(type.ToString());

            // 2. Kích hoạt hiệu ứng (nếu có)
            if (collectEffect != null)
            {
                // Instantiate hiệu ứng tại vị trí của Power-up
                Instantiate(collectEffect, transform.position, Quaternion.identity);
            }

            // 3. Ẩn Power-up và hủy nó sau một khoảng thời gian ngắn
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject, destroyDelay);

            // TẠM THỜI: Thêm âm thanh nếu bạn có AudioController.Ins
            // AudioController.Ins.PlayCollectPowerUpSound();
        }
    }
}