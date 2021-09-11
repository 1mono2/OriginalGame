using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour
{
    private float gravity = 8;
    public GameObject target1;
    Rigidbody rb;

    private Vector3 pos1;
    private float distance1;
    private Vector3 t1Angle;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        pos1 = target1.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        distance1 = Vector3.Distance(pos1, transform.position);
        t1Angle = target1.transform.position - transform.position;
        rb.AddForce(t1Angle.normalized * (gravity / Mathf.Pow(distance1, 2)));


    }
}
