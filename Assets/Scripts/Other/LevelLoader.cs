using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    private static LevelLoader instance;
    public static LevelLoader Instance => instance;
    
    [SerializeField] private Animator animator;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        
        
    }

    // Update is called once per frame
    public void LoadNextScene()
    {
        animator.SetTrigger("Start");
    }

    public void SetNotActive()
    {
        this.gameObject.SetActive(false);
    }
}
