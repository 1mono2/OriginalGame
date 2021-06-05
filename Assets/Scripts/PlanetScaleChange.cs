using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetScaleChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "escapee")
        {
            other.gameObject.SendMessage("PlanetScaleUp");
            Destroy(this.gameObject);
        }

        if (other.gameObject.tag == "seeker")
        {
            other.gameObject.SendMessage("PlanetScaleDown");
            Destroy(this.gameObject);
        }
    }
}
