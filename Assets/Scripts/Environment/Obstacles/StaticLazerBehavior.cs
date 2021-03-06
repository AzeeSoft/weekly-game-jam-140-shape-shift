﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticLazerBehavior : MonoBehaviour
{
    public float damage = 10;
    public AudioClip hitSound;

    void OnTriggerEnter(Collider other)
    {
        FlightModel otherFM = other.gameObject.GetComponent<FlightModel>();

        if (otherFM != null)
        {
            print("Hit Lazer!");
            otherFM.health.TakeDamage(damage);
            SoundEffectsManager.Instance.Play(hitSound);
        }
    }
}