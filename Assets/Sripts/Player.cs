using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : NetworkBehaviour
{
    public static Player Instance { get; private set; }

    public event EventHandler OnScoreChange;
    public event EventHandler OnDamageReceive;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float _healthMax = 100f;
    [SerializeField] private float healthCur = 100f;
    [SerializeField] public float damage = 10f;
    [SerializeField] public float reloadCd = 1.5f;
    [SerializeField] public int barrelMax = 10;
    [SerializeField] public float shotCd = 0.5f;
    [SerializeField] public float bulletSpeed = 30f;
    [SerializeField] public float bulletDamage = 10;

    private int score;
    private Rigidbody playerRb;

    private void Awake()
    {
        score = 0;
        Instance = this;
        playerRb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        GameInput.Instance.OnShot += GameInput_OnShot;
    }

    private void GameInput_OnShot(object sender, System.EventArgs e)
    {
        BulletFactory.Instance.Shot();
    }

    private void FixedUpdate()
    {
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
        Vector2 inputVector = GameInput.Instance.GetRotationVectorNormalized();

        Vector3 rotateDir = new Vector3(inputVector.x, 0f, inputVector.y);

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
        healthCur -= damage;
        OnDamageReceive?.Invoke(this, EventArgs.Empty);
    }

    public float GetCurrentHealthOutMax(out float healthMax)
    {
        healthMax = _healthMax;
        return healthCur;
    }

}
