using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 5;
    private Vector3 moveDir;
    Rigidbody rigidbody;
    public GameObject planet;
    public GameObject gameobjController;
    GameController gameController;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        gameController = gameobjController.GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = new Vector3(Input.GetAxisRaw("Player1Horizontal"), 0, Input.GetAxisRaw("Player1Vertical")).normalized;
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);
    }

    public void SpeedUp()
    {
        moveSpeed = 10;
        StartCoroutine(SetDefalutSpeed());
    }

    public IEnumerator SetDefalutSpeed()
    {
        yield return new WaitForSeconds(5.0f);
        moveSpeed = 5;
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
        StartCoroutine(SetDefaltPlanetScale());
    }

    public IEnumerator SetDefaltPlanetScale()
    {
        yield return new WaitForSeconds(5.0f);
        planet.transform.localScale = new Vector3(15, 15, 15);
        MoveUpDownPos(20.0f, 15.0f);
        gameController.P2MoveUpDownPos(20.0f, 15.0f);
    }

    public void MoveUpDownPos(float beforeScale, float afterScale)
    {
        Vector3 scaleUpPos = transform.position * (afterScale / beforeScale);
        rigidbody.MovePosition(scaleUpPos);
    }

   
}
