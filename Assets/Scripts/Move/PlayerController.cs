using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float torqueAmount = 1f;
    [SerializeField] private float maximumBoostSpeed = 40f;
    [SerializeField] float boostAmount = 0.5f;
    [SerializeField] private float baseSpeed = 20f;
    [SerializeField] private SurfaceEffector2D surfaceEffector2D;

    // Power-up Variables
    private bool isInvincible = false;
    [SerializeField] private float powerUpDuration = 5f;
    [SerializeField] private float maxInvincibleSpeed = 50f;
    [SerializeField] private Color invincibleColor = Color.yellow;

    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;
    private bool canMove = true;

    public bool IsInvincible
    {
        get { return isInvincible; }
    }


    public float CurrentSpeed
    {
        get
        {
            if (surfaceEffector2D != null)
            {
                return surfaceEffector2D.speed;
            }
            return 0f;
        }
    }

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        // Sử dụng GetComponentInChildren để tìm SpriteRenderer, ngay cả khi nó nằm trên đối tượng con
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogWarning("PlayerController: No SpriteRenderer found. Invincibility visual effect will be skipped.");
        }
    }

    private void Update()
    {
        if (canMove)
        {
            RotatePlayer();
            RespondToBoost();
        }
    }

    public void DisableControls()
    {
        canMove = false;
    }

    private void RespondToBoost()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (surfaceEffector2D != null && surfaceEffector2D.speed < maximumBoostSpeed)
            {
                surfaceEffector2D.speed += boostAmount;
            }
        }
        else
        {
            // ĐÃ SỬA: Loại bỏ logic if/else bị dư thừa/buggy. 
            // Khi không boost, tốc độ trở về baseSpeed hiện tại (baseSpeed được Coroutine SpeedBoostPowerUp điều chỉnh).
            if (surfaceEffector2D != null)
            {
                surfaceEffector2D.speed = baseSpeed;
            }
        }
    }

    private void RotatePlayer()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb2d.AddTorque(torqueAmount);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rb2d.AddTorque(-torqueAmount);
        }
    }

    public void ActivatePowerUp(string type)
    {
        switch (type)
        {
            case "Invincibility":
                // Đảm bảo dừng mọi coroutine Invincibility đang chạy để ngăn chặn lỗi xếp chồng
                StopCoroutine(InvincibilityPowerUp());
                StartCoroutine(InvincibilityPowerUp());
                break;
            case "SpeedBoost":
                // Đảm bảo dừng mọi coroutine SpeedBoost đang chạy
                StopCoroutine(SpeedBoostPowerUp());
                StartCoroutine(SpeedBoostPowerUp());
                break;
        }
    }

    // THÊM: Phương thức này được Star/Snowflake gọi để thu thập (thường là cộng điểm)
    public void CollectStar()
    {
        // Thêm logic cập nhật điểm số ở đây. Ví dụ: ScoreManager.Ins.AddScore(1);
        Debug.Log("Star Collected! Only score added, no speed change.");
    }

    IEnumerator InvincibilityPowerUp()
    {
        // Sử dụng StopCoroutine/StartCoroutine trong ActivatePowerUp() sẽ ngăn chặn việc chạy lại.
        isInvincible = true;
        Color originalColor = Color.white;

        // ĐÃ SỬA: Thêm kiểm tra null để tránh lỗi MissingComponentException
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
            spriteRenderer.color = invincibleColor;
        }

        Debug.Log("Invincibility Activated! (Implement isInvincible check in CrashDetector.cs)");

        yield return new WaitForSeconds(powerUpDuration);

        // ĐÃ SỬA: Thêm kiểm tra null
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }

        isInvincible = false;
        Debug.Log("Invincibility Ended.");
    }

    IEnumerator SpeedBoostPowerUp()
    {
        // Lưu trữ tốc độ hiện tại để có thể hoàn nguyên chính xác
        float originalBaseSpeed = baseSpeed;
        float originalMaximumBoostSpeed = maximumBoostSpeed;

        // Thiết lập tốc độ mới
        baseSpeed = maxInvincibleSpeed * 0.8f;
        maximumBoostSpeed = maxInvincibleSpeed;

        // Ngay lập tức áp dụng tốc độ mới nếu tốc độ hiện tại thấp hơn tốc độ cơ bản mới
        if (surfaceEffector2D.speed < baseSpeed)
        {
            surfaceEffector2D.speed = baseSpeed;
        }

        Debug.Log("SpeedBoost Activated! New Base Speed: " + baseSpeed);

        yield return new WaitForSeconds(powerUpDuration);

        // Hết hiệu ứng: Đặt lại tốc độ
        baseSpeed = originalBaseSpeed;
        maximumBoostSpeed = originalMaximumBoostSpeed;

        // Đặt lại tốc độ di chuyển hiện tại về tốc độ cơ bản ban đầu
        if (surfaceEffector2D.speed > baseSpeed)
        {
            surfaceEffector2D.speed = baseSpeed;
        }

        Debug.Log("SpeedBoost Ended. Base Speed Reset to: " + originalBaseSpeed);
    }
}