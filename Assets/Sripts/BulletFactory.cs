using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BulletFactory : NetworkBehaviour
{
    public static BulletFactory Instance { get; private set; }

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int initialBulletPoolSize;
    private List<GameObject> bulletPool;

    private void Awake()
    {
            Instance = this;
    }

    private void Start()
    {
        bulletPool = new List<GameObject>();
        if (!IsServer) { return; }

        
        for (int i = 0; i < initialBulletPoolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }


    public GameObject GetBullet()
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            if (!bulletPool[i].activeSelf)
            {
                return bulletPool[i];
            }
        }

        GameObject bullet = Instantiate(bulletPrefab);
        bullet.SetActive(false);
        bulletPool.Add(bullet);
        return bullet;
    }


    public void ReturnBullet(GameObject bullet)
    {
        bullet.transform.position = transform.position;
        bullet.SetActive(false);
    }
}
