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
        StartCoroutine(waitForLoading());

    }

    // Update is called once per frame
    public void LoadNextScene()
    {
        this.gameObject.SetActive(true);
        animator.SetTrigger("Start");
    }

    public void SetNotActive()
    {
        this.gameObject.SetActive(false);
    }

    IEnumerator waitForLoading(){
        yield return new WaitForSeconds(0.5f);
        SetNotActive();
    }

}
