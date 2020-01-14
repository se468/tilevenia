using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // Config
    [SerializeField] float runSpeed = 8f;
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
        this.isAlive = true;
    }

    void Update()
    {
        if (!this.isAlive) { return; }

        float controlThrowHorizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float controlThrowVertical = CrossPlatformInputManager.GetAxis("Vertical");

        this.Run();
        this.ClimbLadder();
        this.Jump();
        this.FlipSprite();
        this.CheckTouchingHazards();
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

    private void CheckTouchingHazards()
    {
        if (!myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Hazards"))) { return; }

    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed) {
            transform.localScale = new Vector2 (Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    private void Die() {
        myAnimator.SetTrigger("Dying");
        myRigidBody.velocity = deathKick;
        this.isAlive = false;
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (LayerMask.LayerToName(col.gameObject.layer) == "Enemy" &&
            col.otherCollider.GetType() == typeof(UnityEngine.CapsuleCollider2D)) {
            GameObject enemy = col.gameObject;
            bool enemyIsAlive = enemy.GetComponent<EnemyMovement>().isAlive;
            if (enemyIsAlive) {
                this.Die();
            }
        }
    }
}
