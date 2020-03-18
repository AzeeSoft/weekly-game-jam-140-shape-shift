using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightPlayerInput : MonoBehaviour
{
    private FlightModel flightModel;

    void Awake()
    {
        flightModel = GetComponent<FlightModel>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var flightInput = flightModel.flightController.flightInput;

        flightInput.horizontal = Input.GetAxis("Horizontal");
        flightInput.vertical = Input.GetAxis("Vertical");
    }
}
