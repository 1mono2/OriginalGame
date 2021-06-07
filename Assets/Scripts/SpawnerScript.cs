using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject[] items;
    // public GameObject gameObjController;
    // public GameController gameController;
    Rigidbody rigidbody;
    public GameObject planet;

    public float timeleft = 0.0f;
    public float defaltTimeLeft = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        // gameController = gameObjController.GetComponent<GameController>();
        StartCoroutine(ExistCheckAndGenerate());

    }

    // Update is called once per frame
    void Update()
    {
        timeleft -= Time.deltaTime;
        if(timeleft <= 0.0)
        {
            Warp();
            
            timeleft = defaltTimeLeft;
        }

   
       
        
    }

    IEnumerator ExistCheckAndGenerate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            GameObject item = GameObject.FindGameObjectWithTag("Item");
           if(item == null)
            {
                yield return new WaitForSeconds(10);
                Instantiate(items[Random.Range(0, items.Length)], this.transform.position, Quaternion.identity);
            }
           
        }
        
    }

    public void Warp()
    {
        SphereCollider planetCollider = planet.GetComponent<SphereCollider>();
        Vector3 randomPos = GetPosition(Random.Range(0, 360), Random.Range(0, 360),
            planetCollider.radius * (planet.transform.localScale.x - 1));
        Debug.Log(randomPos);
        rigidbody.MovePosition(randomPos);
    }
    public Vector3 GetPosition(float angle1, float angle2, float radius)
    {
        float x = radius * Mathf.Sin(angle1 * Mathf.Deg2Rad) * Mathf.Cos(angle2 * Mathf.Deg2Rad);
        float y = radius * Mathf.Sin(angle1 * Mathf.Deg2Rad) * Mathf.Sin(angle2 * Mathf.Deg2Rad);
        float z = radius * Mathf.Cos(angle1 * Mathf.Deg2Rad);
        return new Vector3(x, y, z);
    }

    public void MoveUpDownPos(float beforeScale, float afterScale)
    {
        Vector3 scaleUpPos = transform.position * (afterScale / beforeScale);
        rigidbody.MovePosition(scaleUpPos);
    }
}
