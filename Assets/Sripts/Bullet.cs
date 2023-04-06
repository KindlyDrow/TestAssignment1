using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float bulletSpeed = 20f;
    public float bulletDamage = 10f;

    private void Awake()
    {
    }

    private void FixedUpdate()
    {
        transform.position += -transform.up * bulletSpeed * Time.deltaTime;
    }

    public void ReturnBullet()
    {
        BulletFactory.Instance.ReturnBullet(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Player collisionPlayer = collision.gameObject.GetComponent<Player>();
                collisionPlayer.DamageReceive(bulletDamage);
                ReturnBullet();
            }
            if (collision.gameObject.CompareTag("Coin"))
            {
                Coin collisionCoin = collision.gameObject.GetComponent<Coin>();
                Destroy(collisionCoin.gameObject);
                ReturnBullet();
            }
        }
    }

}
