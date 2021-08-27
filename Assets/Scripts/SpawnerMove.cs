using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMove : MonoBehaviour
{
   
    private GameObject gameControllerObj;
    private GameController gameController;
    Rigidbody rigidbody;
    private GameObject planet;

    public float timeleft = 0.0f;
    public float defaltTimeLeft = 3.0f;
    

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        gameControllerObj = GameObject.Find("GameController");
        gameController = gameControllerObj.GetComponent<GameController>();

        planet = GameObject.Find("Planet");

        StartCoroutine(Warp());
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.isBattling == true)
        {
            timeleft -= Time.deltaTime;
            if (timeleft <= 0.0)
            { 
                //Warp();
                timeleft = defaltTimeLeft;
            }
            
            
        }
        
    }


    public IEnumerator Warp()
    {
        while (true) {
            yield return new WaitForSeconds(5.0f);
        SphereCollider planetCollider = planet.GetComponent<SphereCollider>();
        Vector3 randomPos = GetPosition(Random.Range(0, 360), Random.Range(0, 360), planetCollider.radius * (planet.transform.localScale.x - 1));
        Debug.Log(randomPos);
        rigidbody.MovePosition(randomPos);
        }
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
