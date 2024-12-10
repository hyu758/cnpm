using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Subjects
{
    private static bool isWin;
    public static bool IsWin => isWin;
    private int enemiesRemaining;
    public int EnemiesRemaining => enemiesRemaining;
    private List<GameObject> enemyList;
    public new CircleCollider2D collider2D;
    public AnimatedSpriteRenderer spriteRenderer;

    private void Awake()
    {
        collider2D = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<AnimatedSpriteRenderer>();
        enemyList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        enemyList.Add(GameObject.FindGameObjectWithTag("Boss"));
        enemiesRemaining = enemyList.Count;
    }

    private void Start()
    {
        Debug.Log("SO QUAI :" + enemiesRemaining);
        InvokeRepeating(nameof(CheckState), 0f, 0.1f);
    }

    private void CheckState()
    {
        isWin = true;
        int enemiesCounter = 0;
        
        for (int i = enemyList.Count - 1; i >= 0; i--)
        {
            if (enemyList[i] == null || !enemyList[i].activeSelf)
            {
                enemyList.RemoveAt(i);
            }
            else
            {
                isWin = false;
                enemiesCounter++;
            }
        }
        
        enemiesRemaining = enemiesCounter;

        if (isWin)
        {
            NotifyObservers(PlayerAction.Win, 0);
            collider2D.enabled = true;
            spriteRenderer.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision) return;
        
        if (collision.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
            {
                SceneManager.LoadScene("HomeScene");
                return;
            }
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
