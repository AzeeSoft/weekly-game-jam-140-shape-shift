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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - yOffset, transform.position.z));

        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - yOffset, transform.position.z), 0.5f);
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
