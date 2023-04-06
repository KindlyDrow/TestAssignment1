using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
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

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("Collision!");
    //    if (collision != null)
    //    {
    //        if (collision.gameObject.tag == Player.Instance.gameObject.tag)
    //        {
    //            Player.Instance.AddCoin(coinValue);
    //            Destroy(gameObject);
    //        }
    //    }
    //}


}
