using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MultiplayerManager : NetworkBehaviour
{
    public static MultiplayerManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ShootServerRpc(float bulletSpeed, float bulletDamage, NetworkObjectReference playerNOR)
    {
        GameObject bulletGO = BulletFactory.Instance.GetBullet();

        NetworkObject bulletNO = bulletGO.GetComponent<NetworkObject>();
        if (!bulletNO.IsSpawned) bulletNO.Spawn(true);

        ShootClientRpc(bulletSpeed, bulletDamage, playerNOR, bulletNO);
    }

    [ClientRpc]
    private void ShootClientRpc(float bulletSpeed, float bulletDamage, NetworkObjectReference playerNOR, NetworkObjectReference bulletNOR)
    {
        
        bulletNOR.TryGet(out NetworkObject bulletNO);
        GameObject bulletGO = bulletNO.gameObject;
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        playerNOR.TryGet(out NetworkObject playerNO);
        Player player = playerNO.GetComponent<Player>();
        Transform shootPosition = player.gameObject.transform;
        

        bulletGO.transform.position = shootPosition.position;
        bulletGO.transform.up = shootPosition.forward;

        bullet.myPlayer = player;
        bullet.bulletSpeed = bulletSpeed;
        bullet.bulletDamage = bulletDamage;

        bulletGO.SetActive(true);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ReturnBulletServerRpc(NetworkObjectReference bulletNOR)
    {
        ReturnBulletClientRpc(bulletNOR);
    }
    
    [ClientRpc]
    public void ReturnBulletClientRpc(NetworkObjectReference bulletNOR)
    {
        bulletNOR.TryGet(out NetworkObject bulletNO);
        GameObject bulletGO = bulletNO.gameObject;
        BulletFactory.Instance.ReturnBullet(bulletGO);
    }
}
