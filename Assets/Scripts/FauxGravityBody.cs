using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxGravityBody : MonoBehaviour
{
    GameObject planet;
    public FauxGravityAttractor attractor;
    private Transform myTransform;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        planet = GameObject.Find("Planet");
        attractor = planet.GetComponent<FauxGravityAttractor>();

        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;
        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        attractor.Attract(myTransform);
    }
}
