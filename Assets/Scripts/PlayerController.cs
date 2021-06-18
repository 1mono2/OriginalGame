using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5;
    private Vector3 moveDir;
    Rigidbody rigidbody;
    public GameObject planet;
    public GameObject gameobjController;
    GameController gameController;

    Animator animator;
    GameObject cat; 


    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        gameController = gameobjController.GetComponent<GameController>();

         cat = GameObject.Find("Player/cat");
        animator = cat.GetComponent<Animator>();
 
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.isBattling == true)
        {
            float h = Input.GetAxisRaw("Player1Horizontal");
            float v = Input.GetAxisRaw("Player1Vertical");
            moveDir = new Vector3(h, 0, v).normalized;
            animator.SetInteger("Walking", 0);
            if (h != 0 | v != 0)
            {
                animator.SetInteger("Walking", 1);

            }
            if (v > 0)
            {
                cat.gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
                if (h < 0)
                {
                    cat.gameObject.transform.localEulerAngles = new Vector3(0, -45, 0);
                }
                else if (h > 0)
                {
                    cat.gameObject.transform.localEulerAngles = new Vector3(0, 45, 0);
                }

            }
            else if (v < 0)
            {
                cat.gameObject.transform.localEulerAngles = new Vector3(0, 180, 0);
                if (h < 0)
                {
                    cat.gameObject.transform.localEulerAngles = new Vector3(0, -135, 0);
                }
                else if (h > 0)
                {
                    cat.gameObject.transform.localEulerAngles = new Vector3(0, 135, 0);
                }
            }
            else if (h < 0)
            {
                cat.gameObject.transform.localEulerAngles = new Vector3(0, -90, 0);
            }
            else if (h > 0)
            {
                cat.gameObject.transform.localEulerAngles = new Vector3(0, 90, 0);
            }
        }
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);
    }

    public void SpeedUp()
    {
        moveSpeed = 10;
        gameController.FloatingParticle("P1");
        StartCoroutine(SetDefalutSpeed());
    }

    public IEnumerator SetDefalutSpeed()
    {
        yield return new WaitForSeconds(5.0f);
        moveSpeed = 5;
        gameController.FinishingParticle();
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

    public void PlanetScaleUp()
    {
        planet.transform.localScale = new Vector3(20, 20, 20);
        // SphereCollider planetCollider = planet.GetComponent<SphereCollider>();
        MoveUpDownPos(15.0f, 20.0f);
        gameController.P2MoveUpDownPos(15.0f, 20.0f);
        gameController.SpawnerMoveUpDownPos(15.0f, 20.0f);
        StartCoroutine(SetDefaltPlanetScale());
    }

    public IEnumerator SetDefaltPlanetScale()
    {
        yield return new WaitForSeconds(5.0f);
        planet.transform.localScale = new Vector3(15, 15, 15);
        MoveUpDownPos(20.0f, 15.0f);
        gameController.P2MoveUpDownPos(20.0f, 15.0f);
        gameController.SpawnerMoveUpDownPos(20.0f, 15.0f);
    }

    public void MoveUpDownPos(float beforeScale, float afterScale)
    {
        Vector3 scaleUpPos = transform.position * (afterScale / beforeScale);
        rigidbody.MovePosition(scaleUpPos);
    }

   
}
