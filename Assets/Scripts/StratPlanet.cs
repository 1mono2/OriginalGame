using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StratPlanet : MonoBehaviour
{

    [SerializeField]
    private float planetRotateSpeed = 0.3f; 

    private void FixedUpdate()
    {
        transform.Rotate(planetRotateSpeed, 0, 0);
    }
}
