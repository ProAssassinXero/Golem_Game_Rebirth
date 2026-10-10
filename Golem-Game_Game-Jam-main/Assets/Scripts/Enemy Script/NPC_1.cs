using System.Collections;
using UnityEngine;

public class NPC_1 : MonoBehaviour
{
    public GameObject player;
    public GameObject projectilePrefab;
    public Animator animator;
    public bool inRange;
    public float speed = 3;
    public float stopDistance = 1.5f;
    public int range = 8;
    public int shootingRange = 12;
    public bool isShooting;
    public bool movingY;
    public bool movingX;
    public bool moving;
    public bool touchingPlayer;
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
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 previousPosition = transform.position;

        // Calculate the distance to the player
        distanceToPlayer = new Vector2(Mathf.Abs(player.transform.position.x - gameObject.transform.position.x), Mathf.Abs(player.transform.position.y - gameObject.transform.position.y));

        // Check if the player is within range
        if (distanceToPlayer.magnitude <= range && !isShooting)
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }

        // Move towards the player if in range and not stopped
        if (inRange && !isShooting && !touchingPlayer)
        {
            MoveTowardsPlayer();
            atStartingPosition = false;
        }
        if (!inRange && !isShooting)
        {
            MoveTowardsStartingPosition();
        }

        if (distanceToPlayer.magnitude <= shootingRange && distanceToPlayer.magnitude >= range && !isShooting)
        {
            StartCoroutine(ShootProjectile());
            isShooting = true;
            animator.SetBool("Shooting", true);
        }

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

        animator.SetBool("WalkRight", !atStartingPosition && moving && right);
        animator.SetBool("WalkLeft", !atStartingPosition && moving && left);
        animator.SetBool("WalkUp", !atStartingPosition && moving && up);
        animator.SetBool("WalkDown", !atStartingPosition && moving && down);

        animator.SetBool("IdleRight", !atStartingPosition && !isShooting && !moving && right);
        animator.SetBool("IdleLeft", !atStartingPosition && !isShooting && !moving && left);
        animator.SetBool("IdleUp", !atStartingPosition && !isShooting && !moving && up);
        animator.SetBool("IdleDown", !atStartingPosition && !isShooting && !moving && down);
    }
    public IEnumerator ShootProjectile()
    {
        Instantiate(projectilePrefab, gameObject.transform.position, Quaternion.identity);
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
            if (collision.gameObject.CompareTag("Player")) touchingPlayer = true;

            Vector2 directionToPlayer = player.transform.position - transform.position;
            if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.y))
            {
                if (directionToPlayer.x < 0)
                {
                    Debug.Log("Player is to the left of NPC");
                }
                else
                {
                    Debug.Log("Player is to the right of NPC");
                }
            }
            else
            {
                if (directionToPlayer.y < 0)
                {
                    Debug.Log("Player is below NPC");
                }
                else
                {
                    Debug.Log("Player is above NPC");
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.CompareTag("Player")) touchingPlayer = false;
        }
    }
}