using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LazerBehavior : MonoBehaviour
{
    public float yOffset;
    public float movementSpeed;

    Sequence lazerSequence;

    void Start()
    {
        lazerSequence = DOTween.Sequence();
        float y = transform.position.y;
        float lowerY = y - yOffset;
        lazerSequence.Append(transform.DOMoveY(lowerY, movementSpeed)).Append(transform.DOMoveY(y, movementSpeed)).SetLoops(-1);
    }

    void OnTriggerEnter(Collider other)
    {
        FlightModel otherFM = other.gameObject.GetComponent<FlightModel>();

        if (otherFM != null)
        {
            print("Ouch! Missed the Hole / Wrong shape!");
        }
    }
}
