using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : NetworkBehaviour
{
    public static Player LocalInstance { get; private set; }

    public event EventHandler OnScoreChange;
    public event EventHandler OnDamageReceive;
    public event EventHandler OnAnyPlayerSpawned;
    public event EventHandler OnBarrelCurrentChange;
    public event EventHandler OnReload;

    [SerializeField] private List<Vector3> spawnPositionsList;
    [SerializeField] private PlayerVisual playerVisual;
    [SerializeField] private Transform _fireTransform;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float _healthMax = 100f;
    [SerializeField] private float healthCur = 100f;
    [SerializeField] private float reloadCd = 1.5f;
    [SerializeField] private int barrelMax = 10;
    [SerializeField] private int barrelCur = 10;
    [SerializeField] private float shotCd = 0.5f;
    [SerializeField] private float bulletSpeed = 30f;
    [SerializeField] private float bulletDamage = 10f;
    [SerializeField] private float canTakeDamageCD = 0.2f;

    private int score = 0;
    private Rigidbody playerRb;
    private bool isShotCd;
    private bool isReloadCd;
    private bool canTakeDamage = true;

    private void Awake()
    {

    }

    private void Start()
    {
        GameInput.Instance.OnShot += GameInput_OnShot;
        score = 0;
        playerRb = GetComponent<Rigidbody>();
        barrelCur = barrelMax;
        PlayerData playerData = MultiplayerManager.Instance.GetPlayerDataFromClientId(OwnerClientId);
        Debug.Log(playerData);
        playerVisual.SetPlayercolor(MultiplayerManager.Instance.GetPlayerColor(playerData.colorId));
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            LocalInstance = this;
        }

        transform.position = spawnPositionsList[MultiplayerManager.Instance.GetPlayerDataIndexFromClientId(OwnerClientId)];

        OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void GameInput_OnShot(object sender, System.EventArgs e)
    {
        if (IsOwner) Shot();
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        HandleRotation();
        HandleMovement();

    }

    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero) transform.position += (moveDir * Time.deltaTime * moveSpeed);
    }

    private void HandleRotation()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.transform.position.y));
        mousePosition.y = transform.position.y;
        Vector3 rotateDir = mousePosition - transform.position;
        rotateDir.Normalize();
        //Vector2 inputVector = GameInput.Instance.GetRotationVectorNormalized();

        //Vector3 rotateDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (rotateDir != Vector3.zero) { transform.forward = Vector3.Slerp(transform.forward, rotateDir, Time.deltaTime * rotateSpeed); }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Coin"))
            {
                Coin coin = collision.gameObject.GetComponent<Coin>();
                AddCoin(coin.coinValue);
                Destroy(collision.gameObject);
            }
        }
    }

    public void Shot()
    {
        if (isShotCd || isReloadCd)
        {
            return;
        }

        NetworkObject playerNO = NetworkObject;
        MultiplayerManager.Instance.ShootServerRpc(bulletSpeed, bulletDamage, playerNO);

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

    private IEnumerator StartReloadCanTakeDamage(float t)
    {

        yield return new WaitForSeconds(t);
        canTakeDamage = true;
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

    public void AddCoin(int value)
    {
        score += value;
        OnScoreChange?.Invoke(this, EventArgs.Empty);
    }

    public int GetScore()
    {
        return score;
    }

    public void DamageReceive(float damage)
    {
        if (canTakeDamage)
        {
            canTakeDamage = false;

            DamageReceiveServerRpc(damage);
            
            StartCoroutine(StartReloadCanTakeDamage(canTakeDamageCD));
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DamageReceiveServerRpc(float damage)
    {
        DamageReceiveClientRpc(damage);
    }
    
    [ClientRpc]
    private void DamageReceiveClientRpc(float damage)
    {
        healthCur -= damage;
        OnDamageReceive?.Invoke(this, EventArgs.Empty);
    }

    public float GetCurrentHealthOutMax(out float healthMax)
    {
        healthMax = _healthMax;
        return healthCur;
    }

    public override void OnDestroy()
    {
        GameInput.Instance.OnShot -= GameInput_OnShot;
    }

}
