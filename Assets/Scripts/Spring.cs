using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] float springSpeed = 100f;
    [SerializeField] AudioClip springSFX;
    private void OnCollisionEnter2D(Collision2D col) {

        if (LayerMask.LayerToName(col.gameObject.layer) == "Player" &&
            col.collider.GetType() == typeof(UnityEngine.BoxCollider2D)) {
            AudioSource.PlayClipAtPoint(springSFX, Camera.main.transform.position);
            Rigidbody2D playerBody = col.gameObject.GetComponent<Rigidbody2D>();
            Vector2 playerVelocity = new Vector2(playerBody.velocity.x, springSpeed);
            playerBody.velocity = playerVelocity;
        }
    }
}
