using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // Config
    [SerializeField] float runSpeed = 12f;
    [SerializeField] float jumpSpeed = 30f;
    [SerializeField] Vector2 deathKick = new Vector2(0f, 100f);
    // State
    bool isAlive = true;

    // Cached component references
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float myGravity = 0f;

    // Messages and Methods
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        myGravity = myRigidBody.gravityScale;
    }

    void Update()
    {
        if (!this.isAlive) { return; }
        this.Run();
        this.ClimbLadder();
        this.Jump();
        this.FlipSprite();
        this.Die();
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed) {
            myAnimator.SetBool("Running", true);
        }
        else {
            myAnimator.SetBool("Running", false);
        }
    }

    private void ClimbLadder()
    {
        if (!myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"))) {
            myAnimator.SetBool("Climbing", false);
            myRigidBody.gravityScale = myGravity;
            return;
        }
        myRigidBody.gravityScale = 0;
        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 playerVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * runSpeed);
        myRigidBody.velocity = playerVelocity;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", playerHasVerticalSpeed);
    }

    private void Jump()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
    }

    private void Die()
    {
        if (!myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards"))) { return; }
        myAnimator.SetTrigger("Dying");
        //myRigidBody.velocity = deathKick;
        //this.isAlive = false;
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed) {
            transform.localScale = new Vector2 (Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

}
