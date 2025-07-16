using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float torqueAmount = 1f;
    [SerializeField] private float maximumBoostSpeed = 40f;
    [SerializeField] float boostAmount = 0.5f;
    [SerializeField] private float baseSpeed = 20f;
    [SerializeField] private SurfaceEffector2D surfaceEffector2D;
    private Rigidbody2D rb2d;
    private bool canMove = true;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
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
}