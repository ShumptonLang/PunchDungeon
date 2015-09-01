using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    public float speed = 3.0f;
    public float jumpSpeed = 50.0f;
    private float playerY = 0;
    private Animator playerAnimator;
    private Rigidbody2D playerRigidbody;
    private SpriteRenderer playerRenderer;
    private Transform playerTransform;
    private float distToGround;
    public float leeway = 0.1f;
    public LayerMask mask = -1;
    public float downPull = 3f;
    private bool isJumpDown = false;
    private bool isShiftDown = false;
    public float maxFallSpeed = 5f;
    private float crouchSpeed;
    private float crouchColliderHeight;
    private BoxCollider2D playerBoundingBox;
    public float ceilingHeight = 1;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerTransform = GetComponent<Transform>();
        playerRenderer = GetComponent<SpriteRenderer>();
        playerBoundingBox = GetComponent<BoxCollider2D>();
        distToGround = playerRenderer.bounds.extents.y;
        crouchSpeed = speed / 2;
        crouchColliderHeight = distToGround / 2;
    }


    void Update()
    {



        playerY = playerRigidbody.velocity.y;
        playerRigidbody.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, playerY);

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumpDown = false;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumpDown = true;
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            isShiftDown = true;
        }
        else
        {
            isShiftDown = false;
        }

        playerAnimator.SetFloat("Walking", Mathf.Abs(Input.GetAxis("Horizontal")));
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }

        if (Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpSpeed);
        }
        if (isJumpDown == false && IsGrounded() == false)
        {
            playerRigidbody.velocity -= new Vector2(0f, downPull);
        }

        if (isShiftDown || IsCeiling())
        {
            speed = crouchSpeed;
            playerBoundingBox.size = new Vector2(0.9f, 0.9f);
        }
        else if (!isShiftDown && !IsCeiling())
        {
            speed = crouchSpeed * 2;
            playerBoundingBox.size = new Vector2(0.9f, 1.4f);
        }


    }

    bool IsGrounded()
    {
        Debug.DrawRay(playerTransform.position, -Vector2.up * (distToGround + leeway));
        return Physics2D.Raycast(playerTransform.position, -Vector3.up, distToGround + leeway, mask);
    }

    bool IsCeiling()
    {
        Debug.DrawRay(playerTransform.position, Vector2.up * (distToGround + ceilingHeight));
        return Physics2D.Raycast(playerTransform.position, Vector3.up, distToGround + leeway, mask);
    }

    void OnTriggerEnter(Collider other)
    {
        other.transform.localPosition.Set(0, 0, 0);
    }


}
