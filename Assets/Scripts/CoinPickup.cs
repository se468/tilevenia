using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    private void OnTriggerEnter2D(Collider2D collider) {
        if(LayerMask.LayerToName(collider.gameObject.layer) == "Player" &&
            collider.GetType() == typeof(UnityEngine.CapsuleCollider2D))
        {
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
            Destroy(this.gameObject);

            FindObjectOfType<GameSession>().AddToScore(100);
        }
    }
}
