using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LazerBehavior : MonoBehaviour
{
    public float offset;
    public float movementSpeed;
    public float damage = 10;

    public bool isHorizontal = false;

    Sequence lazerSequence;

    void Start()
    {
        StartMovement();
    }

    void StartMovement()
    {
        if (!isHorizontal)
        {
            lazerSequence = DOTween.Sequence();
            float y = transform.position.y;
            float lowerY = y - offset;
            lazerSequence.Append(transform.DOMoveY(lowerY, movementSpeed)).Append(transform.DOMoveY(y, movementSpeed)).SetLoops(-1);
        }
        else
        {
            lazerSequence = DOTween.Sequence();
            float x = transform.position.x;
            float rightMostX = x + offset;
            lazerSequence.Append(transform.DOMoveX(rightMostX, movementSpeed)).Append(transform.DOMoveX(x, movementSpeed)).SetLoops(-1);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - offset, transform.position.z));

        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - offset, transform.position.z), 0.5f);
    }

    void OnTriggerEnter(Collider other)
    {
        FlightModel otherFM = other.gameObject.GetComponent<FlightModel>();

        if (otherFM != null)
        {
            print("Hit Lazer!");
            otherFM.health.TakeDamage(damage);
        }
    }
}
