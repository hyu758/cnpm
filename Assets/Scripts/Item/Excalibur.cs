using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Excalibur : MonoBehaviour
{
    public GameObject arcOfEnergyPrefab;
    [SerializeField] KeyCode inputKey;
    

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.Input.GetKeyDown(inputKey) && PlayerStatus.Instance.Weapons[Item.Excalibur] > 0)
        {
            Debug.Log("Attack by Exca");
            ExcaliburAttack();
            PlayerStatus.Instance.RemoveWeaponQuantity(Item.Excalibur, 1);
        }
    }

    void ExcaliburAttack()
    {
        GameObject exca = Instantiate(arcOfEnergyPrefab, this.transform.position, this.transform.rotation);
        //exca.transform.parent = this.transform;

        Vector2 direction = PlayerMovement.Instance.Direction;
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            exca.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

}
