using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player2Controller : MonoBehaviour
{
    private float moveSpeed = 5;
    private Vector3 moveDir;
    Rigidbody rigidbody;

    [SceneName]
    public string resultScene;

    private void Start()
    {
        
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = new Vector3(Input.GetAxisRaw("Player2Horizontal"), 0, Input.GetAxisRaw("Player2Vertical")).normalized;
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "escapee")
        {
            StartCoroutine( LoadResult());
        }
    }

    private IEnumerator LoadResult()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(resultScene);
    }
}
