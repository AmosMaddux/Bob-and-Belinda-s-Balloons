using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    int currentSceneIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadPreviousScene()
    {
        SceneManager.LoadScene(currentSceneIndex - 1);
    }

    public void LoadStartScene()
    {
       // Debug.Log("LoadStartSceneCalled");
        SceneManager.LoadScene(0);
    }

    public void LoadSceneByName(GameObject button)
    {
        SceneManager.LoadScene(button.gameObject.name);
    }

    public void LoadGameOverScreen()
    {
        SceneManager.LoadScene("GameOver");
    }
}
