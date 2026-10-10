using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public TimeSystem timeSystem;
    public Camera camera;
    public float speed = 5f;
    public float dashSpeed = 20f;
    public float dashDuration = 0.1f;
    public bool moveUp = false;
    public bool moveDown = false;
    public bool moveLeft = false;
    public bool moveRight = false;

    public Rigidbody2D rb;
    public Vector2 moveDir;
    public Vector2 lastDirection;
    public Animator animation;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        camera.transform.position = new Vector3(transform.position.x, transform.position.y, camera.transform.position.z);

        moveDir = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.LeftShift) && timeSystem.time >= 10f)
        {
            StartCoroutine(Dash());
        }
        // Get the input axes for movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 currentInput = new Vector2(moveX, moveY).normalized;

        // Update the last direction if there is any input
        if (currentInput != Vector2.zero)
        {
            lastDirection = currentInput;
        }
        // Down Idle Animation Logic
        if (lastDirection.y < 0 && !moveUp && !moveLeft && !moveRight && !moveDown)
        {
            animation.SetBool("IdleDown", true);
        }
        else
        {
            animation.SetBool("IdleDown", false);
        }

        // Right Idle Animation Logic
        if (lastDirection.x > 0 && !moveUp && !moveLeft && !moveRight && !moveDown)
        {
            animation.SetBool("IdleRight", true);
        }
        else
        {
            animation.SetBool("IdleRight", false);
        }
        // Up Idle Animation Logic
        if (lastDirection.y > 0 && !moveUp && !moveLeft && !moveRight && !moveDown)
        {
            animation.SetBool("IdleUp", true);
        }
        else
        {
            animation.SetBool("IdleUp", false);
        }
        // Left Idle Animation Logic
        if (lastDirection.x < 0 && !moveUp && !moveLeft && !moveRight && !moveDown)
        {
            animation.SetBool("IdleLeft", true);
        }
        else
        {
            animation.SetBool("IdleLeft", false);
        }

        // movement controls
        if (Input.GetKey(KeyCode.W) && !moveDown && !moveLeft && !moveRight)
        {
            moveDir = Vector2.up;
            moveUp = true;
            animation.SetBool("WalkUp", true);
        }
        else
        {
            moveUp = false;
            animation.SetBool("WalkUp", false);
        }

        // Down Movement Logic
        if (Input.GetKey(KeyCode.S) && !moveUp && !moveLeft && !moveRight)
        {

            moveDir = Vector2.down;
            moveDown = true;
            animation.SetBool("WalkDown", true);
        }
        else
        {
            moveDown = false;
            animation.SetBool("WalkDown", false);
        }

        // Left Movement Logic
        if (Input.GetKey(KeyCode.A) && !moveUp && !moveDown && !moveRight)
        {
            moveDir = Vector2.left;
            moveLeft = true;
            animation.SetBool("WalkLeft", true);
        }
        else
        {
            moveLeft = false;
            animation.SetBool("WalkLeft", false);
        }

        // Right Movement Logic
        if (Input.GetKey(KeyCode.D) && !moveUp && !moveDown && !moveLeft)
        {
            moveDir = Vector2.right;
            moveRight = true;
            animation.SetBool("WalkRight", true);
        }
        else
        {
            moveRight = false;
            animation.SetBool("WalkRight", false);
        }
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = moveDir * speed;
    }

    // Coroutine to handle dashing
    public IEnumerator Dash()
    {
        speed = dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        speed = 5f;
        timeSystem.DashTimeLoss();
    }
}