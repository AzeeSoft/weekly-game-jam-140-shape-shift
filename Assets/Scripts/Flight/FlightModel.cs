using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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
    public PostProcessVolume damageEffect;
    public float damageEffectTransitionDuration = 0.3f;
    public CameraShakeProps damageCameraShakeProps;

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
        health.OnDamageTaken.AddListener(() =>
        {
            damageEffect.DOKill(true);
            damageEffect.DOWeight(1f, damageEffectTransitionDuration)
                .OnComplete(() => { damageEffect.DOWeight(0f, damageEffectTransitionDuration).Play(); }).Play();

            CameraRig.Instance.ShakeCamera(damageCameraShakeProps);
        });
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