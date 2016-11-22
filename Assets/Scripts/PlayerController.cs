using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;

    GameObject playerLane;

    Vector3 newPos;

    public float moveSpeed;
    public float jumpForce;
    public float dashSpeed;
    public float dashDist;
    public float diveSpeed;
    public int currentLane;

    bool grounded;
    bool jumpDashDown;
    bool diveDown;
    bool dashing;
    bool diving;

    // Fixed Update is called at a fixed amount of time
    void FixedUpdate ()
    {
        // Call the jump function when the jump key is pressed
        if (Input.GetButton("Jump") && grounded)
        {
            Jump();
        }

        if (Input.GetButton("Slide") && !grounded && !dashing && !diveDown)
        {
            diveDown = true;
            rb.AddForce(Vector3.down * diveSpeed, ForceMode.Impulse);
        }
    }

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();

        playerLane = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Move the player forwards at a constant rate
        playerLane.transform.position += Vector3.forward * moveSpeed * Time.deltaTime;

        if (Input.GetButtonDown("Horizontal") && !dashing && !jumpDashDown && !diving)
        {
            bool canDash = true;

            // If the player pressed left
            if (Input.GetAxis("Horizontal") < 0)
            {
                if (currentLane == 1)
                {
                    canDash = false;
                }
                else
                {
                    currentLane--;
                }
            }
            // If the player pressed right
            else if (Input.GetAxis("Horizontal") > 0)
            {
                if (currentLane == 3)
                {
                    canDash = false;
                }
                else
                {
                    currentLane++;
                }
            }

            // If the player can dash calculate the new position
            if (canDash)
            {
                newPos = transform.localPosition;
                newPos.x += Input.GetAxis("Horizontal") * dashDist;

                // If the player is in air
                if (!grounded)
                {
                    jumpDashDown = true;
                    newPos.y -= 0.5f;
                    rb.velocity = Vector3.zero;
                }

                dashing = true;
            }
        }

        if (Input.GetButtonDown("Slide") && !grounded && !dashing)
        {
            /*newPos = transform.localPosition;
            newPos.y = 0;

            diving = true;*/
        }

        // While dash is active, call the dash function
        if (dashing)
        {
            MoveTo(dashSpeed);

            // When the player reaches the new position, disable the dash
            if (transform.localPosition.x == newPos.x)
            {
                dashing = false;
            }
        }

        if (diving)
        {
            MoveTo(diveSpeed);

            if (transform.localPosition.y == newPos.y)
            {
                diving = false;
            }
        }
	}

    // Jump function that adds upwards force to the player
    void Jump ()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    // Function that moves the player towards the new position
    void MoveTo (float speed)
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, newPos, speed);
    }

    void WallRun ()
    {

    }

    // Function that is called when the player starts colliding with another collider
    void OnCollisionEnter(Collision otherObject)
    {
        // If the player collides with the ground and is not grounded, then set the player to grounded
        if (otherObject.collider.CompareTag("Ground") && !grounded)
        {
            grounded = true;

            if (jumpDashDown)
            {
                jumpDashDown = false;
            }

            if (diveDown)
            {
                diveDown = false;
            }
        }

        if (otherObject.collider.CompareTag("Obstacle"))
        {
            Debug.Log("You lose!");
        }
    }

    // Function that is called when the player stops colliding with another collider
    void OnCollisionExit (Collision otherObject)
    {
        // If the player leaves the ground and is grounded, set the player to not grounded
        if (otherObject.collider.CompareTag("Ground") && grounded)
        {
            grounded = false;
        }
    }
}
