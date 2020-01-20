using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public bool isAlive = true;
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    SpriteRenderer mySpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        this.isAlive = true;
        myAnimator.SetBool("isAlive", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.isAlive == false) {
            myRigidBody.velocity = new Vector2(0f, 0f);
            return;
        }
        if (IsFacingRight()) {
            myRigidBody.velocity = new Vector2(moveSpeed, 0f);
        }
        else {
            myRigidBody.velocity = new Vector2(-moveSpeed, 0f);
        }

    }

    public void Die() {
        this.isAlive = false;
        myAnimator.SetBool("isAlive", false);
        myBodyCollider.enabled = false;
        mySpriteRenderer.color = new Color(1, 1, 1, 0.4f);
        Invoke("Deactivate", 2);
    }

    void Deactivate() {
        this.gameObject.SetActive(false);
    }

    private bool IsFacingRight() {
        return transform.localScale.x > 0;
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (LayerMask.LayerToName(col.gameObject.layer) == "Player" &&
            col.collider.GetType() == typeof(UnityEngine.BoxCollider2D)) {

            Rigidbody2D playerBody = col.gameObject.GetComponent<Rigidbody2D>();
            playerBody.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
            this.Die();
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (!this.isAlive) { return; }
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidBody.velocity.x)), 1f);
    }
}
