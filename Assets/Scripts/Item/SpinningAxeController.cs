using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningAxeController : MonoBehaviour
{

    [SerializeField] protected float rotationSpeed;

    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

    }
}
