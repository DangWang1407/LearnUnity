using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float moveForce = 10f;

    [SerializeField]
    private float jumpForce = 80f;


    [SerializeField]
    private ForceMode2D jumpForceMode = ForceMode2D.Impulse;

    private float movementX;

    private Rigidbody2D myBody;

    private SpriteRenderer sr;

    private Animator anim;

    private string WALK_ANIMATION = "Walk";

    private bool isGrounded;

    private bool jumpRequested = false;

    private string GROUND_TAG = "Ground";

    private float jumpStartY;
    private float maxJumpHeight;
    private bool isJumping;

    private float jumpStartTime;
    private float jumpDuration;

    private void Awake()
    {

        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        sr = GetComponent<SpriteRenderer>();

    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(myBody.bodyType);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMoveKeyboard();
        AnimatePlayer();

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            jumpRequested = true;
        }

        //Debug.Log("isGrounded: " + isGrounded);
    }

    private void FixedUpdate()
    {
        if (jumpRequested)
        {
            PlayerJump();
            jumpRequested = false;
        }
        //Debug.Log("FixedUpdate called");

        if (isJumping)
        {
            if (transform.position.y > maxJumpHeight)
                maxJumpHeight = transform.position.y;

            if (myBody.velocity.y < 0 && isGrounded)
            {
                isJumping = false;
                float jumpHeight = maxJumpHeight - jumpStartY;

                jumpDuration = Time.time - jumpStartTime; 

                //Debug.Log($"Mode: {jumpForceMode}, Jump Height: {jumpHeight:F2}, Duration: {jumpDuration:F2} sec");
            }
        }

    }

    void PlayerMoveKeyboard()
    {

        movementX = Input.GetAxisRaw("Horizontal");

        transform.position += new Vector3(movementX, 0f, 0f) * moveForce * Time.deltaTime;

    }

    void AnimatePlayer()
    {

        if (movementX > 0)
        {
            anim.SetBool(WALK_ANIMATION, true);
            sr.flipX = false;
        }
        else if (movementX < 0)
        {
            anim.SetBool(WALK_ANIMATION, true);
            sr.flipX = true;
        }
        else
        {
            anim.SetBool(WALK_ANIMATION, false);
        }

    }

    void PlayerJump()
    {
        isGrounded = false;
        jumpRequested = false;
        isJumping = true;

        jumpStartY = transform.position.y;
        maxJumpHeight = transform.position.y;

        jumpStartTime = Time.time; 

        myBody.velocity = new Vector2(myBody.velocity.x, 0f); 

        myBody.AddForce(new Vector2(0f, jumpForce), jumpForceMode);

        //Debug.Log($"Jump mode: {jumpForceMode}, force: {jumpForce}");

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GROUND_TAG))
            isGrounded = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GROUND_TAG))
            isGrounded = true;
        //Debug.Log(collision.gameObject.name + " isGrounded: " + isGrounded);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GROUND_TAG))
            isGrounded = false;
    }
}


