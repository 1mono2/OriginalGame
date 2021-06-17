using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateItems : MonoBehaviour
{

    public float rotateSpeedX = 0;
    public float rotateSpeedY = 0;
    public float rotateSpeedZ = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateSpeedX, rotateSpeedY, rotateSpeedZ);

    }
}
