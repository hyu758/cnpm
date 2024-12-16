using UnityEngine;

public class DarkExcalibur : MonoBehaviour
{
    [SerializeField] protected GameObject finalSparkPrefab;
    
    [SerializeField] KeyCode inputKey = KeyCode.E;

    private void Awake()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(inputKey) && PlayerStatus.Instance.Weapons[Item.DarkExcalibur] > 0)
        {
            DarkExcaliburAttack();
            PlayerStatus.Instance.RemoveWeaponQuantity(Item.DarkExcalibur, 1);
        }
    }

    void DarkExcaliburAttack()
    {
        GameObject darkExca = Instantiate(finalSparkPrefab, this.transform.position, this.transform.rotation);
        Vector2 direction = PlayerMovement.Instance.Direction;
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            darkExca.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            darkExca.transform.position += new Vector3(direction.x, direction.y, 0f);
        }
    }
}
