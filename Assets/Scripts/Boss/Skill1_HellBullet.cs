using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1_HellBullet : MonoBehaviour
{
    [SerializeField] protected List<GameObject> bulletSpawnPoints;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected GameObject boss;

    [SerializeField] protected float rotationSpeed = 30f;
    [SerializeField] protected float timeBetweenShoots = 0.3f;
    protected float timer = 0;

    private float currentRotation = -90f;

    private void Update()
    {
        currentRotation += rotationSpeed * Time.deltaTime;
        timer += Time.deltaTime;

        if (timer >= timeBetweenShoots)
        {
            Rotating();
            Shooting();
            timer = 0f;
        }   
    }

    private void Shooting()
    {
        foreach (GameObject spawnBulletPoint in bulletSpawnPoints)
        {
            GameObject bullet = Instantiate(bulletPrefab, spawnBulletPoint.transform.position, spawnBulletPoint.transform.rotation);

            SetBulletDamage(bullet);
        }
    }

    private void Rotating()
    {
        for (int i = 0; i < bulletSpawnPoints.Count; i++)
        {
            bulletSpawnPoints[i].transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);

            ChangingDirectionOfRotation(bulletSpawnPoints[i]);
        }  
    }

    protected void ChangingDirectionOfRotation(GameObject bulletSpawnPoint)
    {
        if (bulletSpawnPoint.transform.rotation.eulerAngles.z >= 340f && rotationSpeed > 0)
        {
            rotationSpeed = -rotationSpeed;
        }
        if (bulletSpawnPoint.transform.rotation.eulerAngles.z <= 200f && rotationSpeed < 0)
        {
            rotationSpeed = -rotationSpeed;
        }
    }
    protected void SetBulletDamage(GameObject bullet)
    {
        BossBullet bossBullet = bullet.GetComponent<BossBullet>();
        BossController boss = GetComponent<BossController>();

        if (bossBullet == null) return;
        if (boss == null) return;

        bossBullet.damage = boss.skills[0].damage;
    }

}
