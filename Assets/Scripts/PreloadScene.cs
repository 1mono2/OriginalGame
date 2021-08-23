using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreloadScene : MonoBehaviour
{
    [SceneName]
    public string startScene;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene(startScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
