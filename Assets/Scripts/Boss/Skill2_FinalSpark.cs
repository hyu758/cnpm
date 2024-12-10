using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill2_FinalSpark : MonoBehaviour
{
    [SerializeField] protected float rotationSpeed = 2f;

    [SerializeField] protected GameObject target;
    [SerializeField] protected GameObject finalSparkGameObject;
    [SerializeField] protected GameObject bossController;

    protected FinalSpark finalSpark;

    protected bool isFirstTime = true;


    void OnEnable()
    {
        isFirstTime = true;
    }

    void Update()
    {
        FollowPlayer();
        if(isFirstTime) 
        {
            StartCoroutine(UseFinalSpark());
            isFirstTime = false;
        }
    }

    private void FollowPlayer()
    {

        Vector3 diff = target.transform.position - this.transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, rot_z), rotationSpeed * Time.deltaTime);
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.right* 10f);
    }

    private IEnumerator UseFinalSpark()
    {
        yield return new WaitForSeconds(0.5f);

        GameObject laser = Instantiate(finalSparkGameObject, this.transform.position, this.transform.rotation);
        SetLaserDamage(laser);
        laser.transform.parent = this.transform;


    }
    protected void SetLaserDamage(GameObject laser)
    {
        FinalSpark fs = laser.GetComponent<FinalSpark>();
        BossController boss = GetComponent<BossController>();

        if (fs == null) return;
        if (boss == null) return;

        fs.damage = boss.skills[1].damage;
    }

}

