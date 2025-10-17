using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float torqueAmount = 1f;
    [SerializeField] private float maximumBoostSpeed = 40f;
    [SerializeField] float boostAmount = 0.5f;
    [SerializeField] private float baseSpeed = 20f;
    [SerializeField] private SurfaceEffector2D surfaceEffector2D;
    private bool isInvincible = false;
    [SerializeField] private float powerUpDuration = 5f; 
    [SerializeField] private float maxInvincibleSpeed = 50f;
    [SerializeField] private Color invincibleColor = Color.yellow; // Màu khiên
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;
    private bool canMove = true;

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
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
            if (surfaceEffector2D.speed < maximumBoostSpeed)
            {
                surfaceEffector2D.speed += boostAmount;
            }
        }
        else
        {
            surfaceEffector2D.speed = baseSpeed;
            if (!isInvincible)
            {
                surfaceEffector2D.speed = baseSpeed;
            }
        }
    }

    public void ActivatePowerUp(string type)
    {
        switch (type)
        {
            case "Invincibility":
                StartCoroutine(InvincibilityPowerUp());
                break;
            case "SpeedBoost":
                StartCoroutine(SpeedBoostPowerUp());
                break;
        }
    }

    IEnumerator InvincibilityPowerUp()
    {
        if (isInvincible) yield break; // Ngăn chặn Coroutine chạy lại nếu đã có

        isInvincible = true;
        Color originalColor = spriteRenderer.color; // Lưu màu gốc

        // Hiệu ứng: Đổi màu người chơi và tăng tốc độ tối đa
        spriteRenderer.color = invincibleColor;

        // Vô hiệu hóa CrashDetector va chạm (Chắc chắn CrashDetector có biến 'isInvincible' để kiểm tra)
        // Hiện tại, tôi sẽ dùng cách này để người chơi không bị crash khi đang có khiên.
        // CẦN CẬP NHẬT CrashDetector.cs:
        // isInvincible = true; // Dùng cờ này trong CrashDetector.cs để bỏ qua va chạm với bom/rock.

        Debug.Log("Invincibility Activated!");

        yield return new WaitForSeconds(powerUpDuration);

        // Hết hiệu ứng
        // isInvincible = false; // Đặt lại cờ này trong CrashDetector.cs
        spriteRenderer.color = originalColor;

        Debug.Log("Invincibility Ended.");
    }

    // THÊM: Coroutine cho Power-up Tăng Tốc Độ
    IEnumerator SpeedBoostPowerUp()
    {
        // Tăng tạm thời baseSpeed và maximumBoostSpeed
        float originalBaseSpeed = baseSpeed;
        float originalMaximumBoostSpeed = maximumBoostSpeed;

        baseSpeed = maxInvincibleSpeed * 0.8f; // Tăng base speed
        maximumBoostSpeed = maxInvincibleSpeed; // Tăng max speed

        Debug.Log("SpeedBoost Activated! Max Speed: " + maximumBoostSpeed);

        yield return new WaitForSeconds(powerUpDuration);

        // Hết hiệu ứng: Đặt lại tốc độ
        baseSpeed = originalBaseSpeed;
        maximumBoostSpeed = originalMaximumBoostSpeed;
        surfaceEffector2D.speed = baseSpeed; // Đặt lại speed

        Debug.Log("SpeedBoost Ended. Max Speed: " + originalMaximumBoostSpeed);
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
}