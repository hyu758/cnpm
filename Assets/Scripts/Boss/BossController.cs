using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill
{
    public int damage;
    public float time;
    public GameObject skillObject;
}

public class BossController : Subjects
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected PolygonCollider2D normalCollider;
    [SerializeField] protected PolygonCollider2D skill2Collider;

    public List<Skill> skills;

    [SerializeField] protected float restTime = 2f;
    [SerializeField] protected int currentAction = 0;

    [SerializeField] public int HP = 100;
    [SerializeField] public int maxHP = 100;


    void Awake()
    {
        animator = GetComponent<Animator>();

    }

    void Start()
    {
        SetCollider();
        HP = maxHP;
        for (int i = 0; i < skills.Count; i++)
        {
            skills[i].skillObject.SetActive(false);
        }
        StartCoroutine(UseSkills());
    }

    void Update()
    {
        if (HP <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    void SetCollider()
    {
        if (normalCollider == null || skill2Collider == null)
        {
            PolygonCollider2D[] colliders = GetComponents<PolygonCollider2D>();
            if (colliders.Length >= 2)
            {
                normalCollider = colliders[0];
                skill2Collider = colliders[1];
            }
        }

        normalCollider.enabled = true;
        skill2Collider.enabled = false;
    }

    private IEnumerator UseSkills()
    {
        while (true)
        {
            // Resting
            yield return new WaitForSeconds(restTime);

            if(currentAction == 1)
            {
                StartCoroutine(ChangeCollider());
            }

            //Using skill
            string isUsingSkill = "isUsingSkill" + (currentAction + 1);
            animator.SetBool(isUsingSkill, true);
            // Waiting for pre-skill
            yield return new WaitForSeconds(1f);

            skills[currentAction].skillObject.SetActive(true);
            yield return new WaitForSeconds(skills[currentAction].time);
            skills[currentAction].skillObject.SetActive(false);

            animator.SetBool(isUsingSkill, false);

            NextAction();
        }
    }
    private IEnumerator ChangeCollider()
    {
        normalCollider.enabled = false;
        skill2Collider.enabled = true;

        yield return new WaitForSeconds(skills[1].time);

        normalCollider.enabled = true;
        skill2Collider.enabled = false;
    }

    private void NextAction()
    {
        currentAction += 1;
        if (currentAction >= skills.Count)
        {
            currentAction = 0;
        }
    }
    public void HandleHurt(int damage)
    {
        HP -= damage;
        Debug.Log("Remain HP of Boss: " + HP.ToString());
        NotifyObservers(BossAction.Hurt, damage);
    }


}
