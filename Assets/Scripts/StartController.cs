using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour
{
    [SceneName]
    public string mainGame;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(LoadGame());
        }
    }

    private IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(mainGame);
    }
}
