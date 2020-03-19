using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlightShape
{
    Circle,
    Triangle,
    Square,
    Star
}

public class FlightModel : MonoBehaviour
{
    public bool switchCurveTargetOnBarrierTrigger = true;

    public FlightShape curFlightShape => flightController.curFlightShape;

    public new Rigidbody rigidbody { get; private set; }
    public Health health { get; private set; }
    public FlightController flightController { get; private set; }
    public FlightPlayerInput flightPlayerInput { get; private set; }

    public event Action<BarrierBehavior, bool> onPassedThroughBarrier;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
        flightController = GetComponent<FlightController>();
        flightPlayerInput = GetComponent<FlightPlayerInput>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PassedThroughBarrier(BarrierBehavior barrierBehavior, bool success)
    {
        if (switchCurveTargetOnBarrierTrigger)
        {
            LevelManager.Instance.proceduralLevelGenerator.CheckAndSwitchCurvetarget();
        }

        onPassedThroughBarrier?.Invoke(barrierBehavior, success);
    }
}