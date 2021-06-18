using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultController : MonoBehaviour
{
    [SceneName]
    public string startScene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            StartCoroutine(LoadStart());
        }
    }

    private IEnumerator LoadStart()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(startScene);
    }
}
