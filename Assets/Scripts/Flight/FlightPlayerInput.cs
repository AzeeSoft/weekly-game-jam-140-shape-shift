using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlightPlayerInput : MonoBehaviour
{
    private FlightModel flightModel;
    private FlightInput flightInput => flightModel.flightController.flightInput;

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

    }

    [UsedImplicitly]
    void OnMove(InputValue value)
    {
        var move = value.Get<Vector2>();

        flightInput.horizontal = move.x;
        flightInput.vertical = move.y * (GameManager.Instance.gameData.playerSettings.shouldInvertY ? -1 : 1);
    }

    [UsedImplicitly]
    void OnShapeShiftSquare(InputValue value)
    {
        flightModel.flightController.ShapeShiftTo(FlightShape.Square);
    }

    [UsedImplicitly]
    void OnShapeShiftTriangle(InputValue value)
    {
        flightModel.flightController.ShapeShiftTo(FlightShape.Triangle);
    }

    [UsedImplicitly]
    void OnShapeShiftCircle(InputValue value)
    {
        flightModel.flightController.ShapeShiftTo(FlightShape.Circle);
    }

    [UsedImplicitly]
    void OnShapeShiftStar(InputValue value)
    {
        flightModel.flightController.ShapeShiftTo(FlightShape.Star);
    }
}
