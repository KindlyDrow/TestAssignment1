using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Coin : NetworkBehaviour
{
    [SerializeField] public int coinValue;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rotationRandom;

    private void Awake()
    {
        rotationRandom = Random.Range(1f,3f);
    }

    private void Update()
    {
        transform.Rotate(Vector3.left, rotationSpeed * Time.deltaTime * rotationRandom);
    }

}
