﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    private void OnTriggerEnter2D(Collider2D collider) {
        AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
        Destroy(this.gameObject);

        FindObjectOfType<GameSession>().AddToScore(100);
    }
}
