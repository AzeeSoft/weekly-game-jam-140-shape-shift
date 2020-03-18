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
    public FlightShape curFlightShape => flightController.curFlightShape;

    public new Rigidbody rigidbody { get; private set; }
    public FlightController flightController { get; private set; }
    public FlightPlayerInput flightPlayerInput { get; private set; }

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
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
}