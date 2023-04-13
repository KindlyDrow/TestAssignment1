using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MultiplayerGameHandler : NetworkBehaviour
{
    public static MultiplayerGameHandler Instance;

    private void Awake()
    {
        Instance = this;
    }

    //BULLET

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

    //COINS

    [ServerRpc(RequireOwnership = false)]
    public void SpawnCoinServerRpc()
    {
        GameObject coinGO = CoinFactory.Instance.GetCoin();

        NetworkObject coinNO = coinGO.GetComponent<NetworkObject>();
        if (!coinNO.IsSpawned) coinNO.Spawn(true);

        Vector3 SpawnPosition = GetRandomPosition();
        
        SpawnCoinClientRpc(coinNO, SpawnPosition);
    }

    public Vector3 GetRandomPosition()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float randomX = Random.Range(0, screenWidth);
        float randomZ = Random.Range(0, screenHeight);

        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(randomX, randomZ, Camera.main.transform.position.y));

        worldPoint.y = 1.2f;

        return worldPoint;
    }

    [ClientRpc]
    private void SpawnCoinClientRpc(NetworkObjectReference coinNOR, Vector3 spawnPosition)
    {

        coinNOR.TryGet(out NetworkObject coinNO);
        GameObject coinGO = coinNO.gameObject;

        coinGO.GetComponent<Coin>().rotationRandom  = Random.Range(1f, 3f);

        coinGO.transform.position = spawnPosition;

        coinGO.SetActive(true);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ReturnCoinServerRpc(NetworkObjectReference bulletNOR)
    {
        ReturnCoinClientRpc(bulletNOR);
    }

    [ClientRpc]
    public void ReturnCoinClientRpc(NetworkObjectReference CoinNOR)
    {
        CoinNOR.TryGet(out NetworkObject CoinNO);
        GameObject CoinGO = CoinNO.gameObject;
        CoinFactory.Instance.ReturnCoin(CoinGO);
    }
}
