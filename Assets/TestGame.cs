using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestGame : MonoBehaviour
{
    public BossController bossController;
    
    void Update()
    {
        PassScene();
        DamageObject();
    }

    private void DamageObject()
    {
        if(Input.GetKeyDown(KeyCode.O)) 
        {
            bossController?.HandleHurt(10);
        }
    }

    private void PassScene()
    {
        if(Input.GetKeyDown(KeyCode.P)) 
        {
            if(SceneManager.GetActiveScene().buildIndex + 1< SceneManager.sceneCountInBuildSettings)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
