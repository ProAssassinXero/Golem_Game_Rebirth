using System.Collections;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public TimeSystem timeSystem;
    public RoomScript roomScript;
    public ProjectileScript projectileScript;
    public NPC_2[] nPC_2s;
    public Camera camera;
    public float speed = 5f;
    public float dashSpeed = 20f;
    public float dashDuration = 0.1f;
    public bool inCircle;
    public bool moveUp = false;
    public bool moveDown = false;
    public bool moveLeft = false;
    public bool moveRight = false;

    public Rigidbody2D rb;
    public Vector2 moveDir;
    public Vector2 lastDirection;
    public Animator animation;
    public bool knockback;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
        roomScript = GameObject.FindGameObjectWithTag("RoomScript").GetComponent<RoomScript>();
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
        if (projectileScript == null && !knockback && !projectileScript.knockback)
        {
            rb.linearVelocity = moveDir * speed;
            return;
        }
        if (!projectileScript.knockback && !knockback)
        {
            rb.linearVelocity = moveDir * speed;
        }
        if (projectileScript.knockback)
        {
            StartCoroutine(KnockBackEnd());
        }
        else if (knockback)
        {
            StartCoroutine(ExplodeKnockBackEnd());
        }
    }

    // Coroutine to handle dashing
    public IEnumerator Dash()
    {
        speed = dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        speed = 5f;
        timeSystem.DashTimeLoss();
    }
    public IEnumerator KnockBackEnd()
    {
        yield return new WaitForSeconds(0.1f);
        projectileScript.knockback = false;
        knockback = false;

        for (int i = 0; i < nPC_2s.Length; i++)
        {
            nPC_2s[i].slashing = false;
            nPC_2s[i].touchingPlayer = false;
            nPC_2s[i].animator.SetBool("Slashing", false);
        }
    }
    public IEnumerator ExplodeKnockBackEnd()
    {
        yield return new WaitForSeconds(1.0f);
        projectileScript.knockback = false;
        knockback = false;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            projectileScript = collision.GetComponent<ProjectileScript>();
        }

        if (collision.gameObject.CompareTag("groundFloorStairs"))
        {
            transform.position = roomScript.spawnPoints[1].transform.position;
        }
        if (collision.gameObject.CompareTag("firstFloorStairs"))
        {
            transform.position = roomScript.spawnPoints[0].transform.position;
        }
        if (collision.gameObject.CompareTag("Circle"))
        {
            inCircle = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Circle"))
        {
            inCircle = false;
        }
    }
}