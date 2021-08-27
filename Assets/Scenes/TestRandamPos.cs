using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRandamPos : MonoBehaviour
{
    Rigidbody rigidbody;
    [SerializeField]
    GameObject planet;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        StartCoroutine(Warp());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Warp()
    {
        
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            SphereCollider planetCollider = planet.GetComponent<SphereCollider>();
            Vector3 randomPos = GetPosition(Random.Range(0, 360), Random.Range(0, 360),
                planetCollider.radius * (planet.transform.localScale.x - 1));
            Debug.Log(randomPos);
            rigidbody.MovePosition(randomPos);
            // Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), randomPos + planet.transform.position, Quaternion.identity);
            
        }
    }

    public Vector3 GetPosition(float angle1, float angle2, float radius)
    {
        float x = radius * Mathf.Sin(angle1 * Mathf.Deg2Rad) * Mathf.Cos(angle2 * Mathf.Deg2Rad);
        float y = radius * Mathf.Sin(angle1 * Mathf.Deg2Rad) * Mathf.Sin(angle2 * Mathf.Deg2Rad);
        float z = radius * Mathf.Cos(angle1 * Mathf.Deg2Rad);
        return new Vector3(x, y, z);
    }

}
