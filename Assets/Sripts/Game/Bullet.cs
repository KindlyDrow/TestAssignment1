using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : NetworkBehaviour
{

    public float bulletSpeed = 20f;
    public float bulletDamage = 10f;
    public Player myPlayer;

    private void Awake()
    {
    }

    private void FixedUpdate()
    {
        transform.localPosition += transform.up * bulletSpeed * Time.deltaTime;
        CameraBounderyCheck();
    }

    private void CameraBounderyCheck()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        if (screenPosition.x < 0 || screenPosition.x > Screen.width)
        {
            ReturnBullet();
        }

        if (screenPosition.y < 0 || screenPosition.y > Screen.height)
        {
            ReturnBullet();
        }
    }

    public void ReturnBullet()
    {
        NetworkObject bulletNO = NetworkObject;
        MultiplayerGameHandler.Instance.ReturnBulletServerRpc(bulletNO);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Player collisionPlayer = collision.gameObject.GetComponent<Player>();
                if (collisionPlayer == myPlayer) return;

                if (IsClient) return;
                collisionPlayer.DamageReceive(bulletDamage);
                ReturnBullet();
            }
        }
    }

}
