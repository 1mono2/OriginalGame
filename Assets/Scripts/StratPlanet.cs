using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StratPlanet : MonoBehaviour
{

    [SerializeField]
    private float planetRotateSpeed = 0.6f; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(planetRotateSpeed, 0, 0);
    }
}
