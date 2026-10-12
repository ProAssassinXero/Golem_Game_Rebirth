using System.Collections;
using UnityEngine;

public class NPC_2 : MonoBehaviour
{
    public GameObject player;
    public GameObject projectilePrefab;
    public PlayerMovement playerMovement;
    public TimeSystem timeSystem;
    public Animator animator;
    public bool inRange;
    public float speed = 3;
    public float stopDistance = 0f;
    public int range = 8;
    public int shootingRange = 12;
    public bool isShooting;
    public bool movingY;
    public bool movingX;
    public bool moving;
    public bool touchingPlayer;
    public bool slashing;
    public bool atStartingPosition;
    public Vector2 startingPosition;
    public Vector2 lastDirection;
    public Vector2 distanceToPlayer;
    public Vector2 targetPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastDirection = Vector2.down;
        targetPosition = transform.position;
        animator = GetComponent<Animator>();
        startingPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        timeSystem = GameObject.FindGameObjectWithTag("TimeSystem").GetComponent<TimeSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 previousPosition = transform.position;

        // Calculate the distance to the player
        distanceToPlayer = new Vector2(Mathf.Abs(player.transform.position.x - gameObject.transform.position.x), Mathf.Abs(player.transform.position.y - gameObject.transform.position.y));

        // Check if the player is within range
        if (distanceToPlayer.magnitude <= range && !isShooting && !playerMovement.inCircle)
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }

        if (touchingPlayer)
        {
            animator.SetBool("Slashing", true);
        }
        else
        {
            animator.SetBool("Slashing", false);
        }

        // Move towards the player if in range and not stopped
        if (inRange && !isShooting && !touchingPlayer && !playerMovement.inCircle)
        {
            MoveTowardsPlayer();
            atStartingPosition = false;
        }
        // Move back to the starting position if not in range and not shooting
        if (!inRange && !isShooting)
        {
            MoveTowardsStartingPosition();
        }

        // Shoot a projectile if the player is within shooting range and not too close
        if (distanceToPlayer.magnitude <= shootingRange && distanceToPlayer.magnitude >= range && !isShooting)
        {
            StartCoroutine(ShootProjectile());
            isShooting = true;
            animator.SetBool("Shooting", true);
        }

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Slash") && stateInfo.normalizedTime >= 1.0f && slashing)
        {
            animator.SetBool("Slashing", false);
        }

        // Update the moving state based on the change in position
        Vector2 delta = (Vector2)transform.position - previousPosition;
        moving = delta.sqrMagnitude > 0.00001f;
        if (moving) lastDirection = delta;
        UpdateAnimation();
    }
    void UpdateAnimation()
    {
        bool horizontal = Mathf.Abs(lastDirection.x) > Mathf.Abs(lastDirection.y);

        bool right = horizontal && lastDirection.x > 0;
        bool left = horizontal && lastDirection.x < 0;
        bool up = !horizontal && lastDirection.y > 0;
        bool down = !horizontal && lastDirection.y < 0;
        slashing = animator.GetBool("Slashing");

        animator.SetBool("WalkRight", !slashing && !atStartingPosition && moving && right);
        animator.SetBool("WalkLeft", !slashing && !atStartingPosition && moving && left);
        animator.SetBool("WalkUp", !slashing && !atStartingPosition && moving && up);
        animator.SetBool("WalkDown", !slashing && !atStartingPosition && moving && down);

        animator.SetBool("IdleRight", !slashing && !atStartingPosition && !isShooting && !moving && right);
        animator.SetBool("IdleLeft", !slashing && !atStartingPosition && !isShooting && !moving && left);
        animator.SetBool("IdleUp", !slashing && !atStartingPosition && !isShooting && !moving && up);
        animator.SetBool("IdleDown", !slashing && !atStartingPosition && !isShooting && !moving && down);
    }
    public IEnumerator ShootProjectile()
    {
        if (!slashing)
        {
            Instantiate(projectilePrefab, gameObject.transform.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(1f);
        isShooting = false;
        animator.SetBool("Shooting", false);
    }


    void MoveTowardsStartingPosition()
    {
        Vector2 distanceToStartingPosition = new Vector2(Mathf.Abs(startingPosition.x - gameObject.transform.position.x), Mathf.Abs(startingPosition.y - gameObject.transform.position.y));
        // Stop when close enough so it doesn't push against the starting position
        if ((Vector2)gameObject.transform.position == startingPosition)
        {
            animator.SetBool("IdleDown", true);
            animator.SetBool("IdleUp", false);
            animator.SetBool("IdleLeft", false);
            animator.SetBool("IdleRight", false);
            atStartingPosition = true;
        }
        // Stop when close enough so it doesn't push against the starting position
        if (distanceToStartingPosition.magnitude < 0.01f)
        {
            gameObject.transform.position = startingPosition;
            movingX = false;
            movingY = false;
        }

        // Release the axis lock when we reach the target OR are lined up with the starting position on that axis
        if (Vector2.Distance(gameObject.transform.position, targetPosition) < 0.01f || (movingX && distanceToStartingPosition.x < 0.05f) || (movingY && distanceToStartingPosition.y < 0.05f))
        {
            movingX = false;
            movingY = false;
        }

        // Move towards the starting position on the axis with the greater distance, unless that axis is locked
        if (distanceToStartingPosition.x > distanceToStartingPosition.y && !movingY)
        {
            targetPosition = new Vector2(startingPosition.x, gameObject.transform.position.y);
            movingX = true;
            movingY = false;
        }
        else if (distanceToStartingPosition.y > distanceToStartingPosition.x && !movingX)
        {
            targetPosition = new Vector2(gameObject.transform.position.x, startingPosition.y);
            movingY = true;
            movingX = false;
        }
        else if (!movingX && !movingY)
        {
            targetPosition = new Vector2(startingPosition.x, gameObject.transform.position.y);
            movingX = true;
        }

        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, targetPosition, speed * Time.deltaTime);
    }
    // Move the NPC towards the player
    void MoveTowardsPlayer()
    {
        // Stop when close enough so it doesn't push against the player
        if (distanceToPlayer.magnitude < stopDistance)
        {
            movingX = false;
            movingY = false;
            return;
        }

        // Release the axis lock when we reach the target OR are lined up with the player on that axis
        if (Vector2.Distance(gameObject.transform.position, targetPosition) < 0.01f || (movingX && distanceToPlayer.x < 0.05f) || (movingY && distanceToPlayer.y < 0.05f))
        {
            movingX = false;
            movingY = false;
        }
        // Move towards the player on the axis with the greater distance, unless that axis is locked
        if (distanceToPlayer.x > distanceToPlayer.y && !movingY)
        {
            targetPosition = new Vector2(player.transform.position.x, gameObject.transform.position.y);
            movingX = true;
            movingY = false;
        }
        else if (distanceToPlayer.y > distanceToPlayer.x && !movingX)
        {
            targetPosition = new Vector2(gameObject.transform.position.x, player.transform.position.y);
            movingY = true;
            movingX = false;
        }
        else if (Mathf.Approximately(distanceToPlayer.x, distanceToPlayer.y) && distanceToPlayer.x > 0)
        {
            if (!movingY)
            {
                targetPosition = new Vector2(player.transform.position.x, gameObject.transform.position.y);
                movingX = true;
                movingY = false;
            }
            else if (!movingX)
            {
                targetPosition = new Vector2(gameObject.transform.position.x, player.transform.position.y);
                movingY = true;
                movingX = false;
            }
        }

        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, targetPosition, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchingPlayer = true;
            animator.SetBool("Slashing", true);
            playerMovement.knockback = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchingPlayer = true;
            animator.SetBool("Slashing", true);
            Vector2 directionToPlayer = player.transform.position - transform.position;
            player.GetComponent<Rigidbody2D>().AddForce(directionToPlayer * 30f, ForceMode2D.Impulse);
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerMovement.StartCoroutine(playerMovement.KnockBackEnd());
            touchingPlayer = false;
            animator.SetBool("Slashing", false);
        }
    }
}