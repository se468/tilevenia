using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] float springSpeed = 100f;
    private void OnCollisionEnter2D(Collision2D col) {
        Debug.Log("SPRING!");

        if (LayerMask.LayerToName(col.gameObject.layer) == "Player" &&
            col.collider.GetType() == typeof(UnityEngine.BoxCollider2D)) {
            Debug.Log("SPRING!");
            Rigidbody2D playerBody = col.gameObject.GetComponent<Rigidbody2D>();
            Vector2 playerVelocity = new Vector2(playerBody.velocity.x, springSpeed);
            playerBody.velocity = playerVelocity;
        }
    }
}
