using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Coin : NetworkBehaviour
{
    public CoinFactory.CoinType coinType;

    [SerializeField] public int coinValue;
    private float rotationSpeed = 20f;
    public float rotationRandom;

    private void Awake()
    {
        
    }

    private void Update()
    {
        transform.Rotate(Vector3.left, rotationSpeed * Time.deltaTime * rotationRandom);
    }

    public void ReturnCoin()
    {
        NetworkObject coinNO = NetworkObject;
        MultiplayerGameHandler.Instance.ReturnCoinServerRpc(coinNO);
    }

}
