using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    public static BulletFactory Instance;

    public event EventHandler OnBarrelCurrentChange;
    public event EventHandler OnReload;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int initialBulletPoolSize;
    [SerializeField] private List<GameObject> bulletPool;

    private bool isReloadCd = false;
    private bool isShotCd = false;

    private float reloadCd = 1.5f;
    private int barrelMax = 10;
    private int barrelCur  = 10;
    private float shotCd = 0.5f;
    private float bulletSpeed;
    private float bulletDamage;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CheckUpdates();
        barrelCur = barrelMax;
        bulletPool = new List<GameObject>();

        for (int i = 0; i < initialBulletPoolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform, true);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }

    private void CheckUpdates()
    {
        reloadCd = Player.Instance.reloadCd;
        barrelMax = Player.Instance.barrelMax;
        shotCd = Player.Instance.shotCd;
        bulletSpeed = Player.Instance.bulletSpeed;
        bulletDamage = Player.Instance.bulletDamage;
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

        GameObject bullet = Instantiate(bulletPrefab, transform, true);
        bullet.SetActive(false);
        bulletPool.Add(bullet);
        return bullet;
    }

    public void Shot()
    {
        if (isShotCd || isReloadCd)
        {
            return;
        }
        CheckUpdates();

        

        GameObject bulletGO = GetBullet();
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        bulletGO.transform.position = transform.position;
        bulletGO.transform.SetParent(null);

        bullet.bulletSpeed = bulletSpeed;
        bullet.bulletDamage = bulletDamage;
        bulletGO.SetActive(true);


        StartCd();
        OnBarrelCurrentChange?.Invoke(this, EventArgs.Empty);
    }

    private void StartCd()
    {
        barrelCur--;
        if (barrelCur < 1)
        {
            ReloadCd();
        }
        else
        {
            ShotCd();
        }
    }

    private void ShotCd()
    {
        isShotCd = true;
        StartCoroutine(StartShotCd(shotCd));
    }

    private void ReloadCd()
    {
        isReloadCd = true;
        OnReload?.Invoke(this, EventArgs.Empty);
        StartCoroutine(StartReloadCd(reloadCd));
    }

    private IEnumerator StartShotCd(float t)
    {
        
        yield return new WaitForSeconds(t);
        isShotCd = false;
    }

    private IEnumerator StartReloadCd(float t)
    {

        yield return new WaitForSeconds(t);
        barrelCur = barrelMax;
        isReloadCd = false;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bullet.transform.position = transform.position;
    }

    public int GetBarrelCurrent()
    {
        return barrelCur;
    }

    public int GetBarrelMax()
    {
        return barrelMax;
    }

    public float GetReloadTime()
    {
        return reloadCd;
    }
}
