﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [SerializeField] AudioClip heartPickupSFX;
    private void OnTriggerEnter2D(Collider2D collider) {
        AudioSource.PlayClipAtPoint(heartPickupSFX, Camera.main.transform.position);
        Destroy(this.gameObject);

        FindObjectOfType<GameSession>().AddToLives(1);
    }
}
