using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public GameObject player1;
    public GameObject player2;

    [SceneName]
    public string resultScene;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator LoadResult ()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(resultScene);
    }
}
