using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralLevelUnit : MonoBehaviour
{
    public Transform prevConnector;
    public Transform nextConnector;

    public event Action onDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        onDestroyed?.Invoke();
    }
}
