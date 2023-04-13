using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CoinFactory : NetworkBehaviour
{
    public static CoinFactory Instance { get; private set; }

    public enum CoinType
    {
        BronzeCoin = default,
        SilverCoin,
        GoldCoin,
    }

    private CoinType _coinType;

    [SerializeField] private GameObject bronzeCoinPrefab;
    [SerializeField] private int initialBronzeCoinPoolSize;

    [SerializeField] private GameObject silverCoinPrefab;
    [SerializeField] private int initialSilverCoinPoolSize;

    [SerializeField] private GameObject goldCoinPrefab;
    [SerializeField] private int initialGoldCoinPoolSize;

    private int fullSize;
    private int currentAllSpawnedCoins;
    private int currentBronzeSpawnedCoins;
    private int currentSilverSpawnedCoins;
    private int currentGoldSpawnedCoins;

    private List<GameObject> bronzeCoinPool;
    private List<GameObject> silverCoinPool;
    private List<GameObject> goldCoinPool;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CreateCoinsPool();
    }

    public bool CanGetCoin()
    {
        return currentAllSpawnedCoins < fullSize;
    }

    public GameObject GetCoin()
    {
        GetRandomType();
        if (!CanGetCoin())
        {
            _coinType = CoinType.BronzeCoin;
        }

        GameObject coin = GetCoinByType();

        currentAllSpawnedCoins++;
        if (coin != null) return coin;

        GameObject reservCoin = Instantiate(bronzeCoinPrefab);
        reservCoin.SetActive(false);
        reservCoin.GetComponent<Coin>().coinType = CoinType.BronzeCoin;
        bronzeCoinPool.Add(reservCoin);
        
        return reservCoin;

    }

    public void ReturnCoin(GameObject coinGO)
    {
        coinGO.transform.position = transform.position;
        Coin coin = coinGO.GetComponent<Coin>();
        currentAllSpawnedCoins--;

        switch (coin.coinType)
        {
            case CoinType.BronzeCoin:
                currentBronzeSpawnedCoins--;
                break;
            case CoinType.SilverCoin:
                currentSilverSpawnedCoins--;
                break;
            case CoinType.GoldCoin:
                currentGoldSpawnedCoins--;
                break;
        }

        coinGO.SetActive(false);
    }

    private void GetRandomType()
    {
        int randomcoin = UnityEngine.Random.Range(1, 100);
        if (randomcoin > 80)
        {
            _coinType = CoinType.GoldCoin;
            if (CanGetCoinType(_coinType)) { return; }
        }
        if (randomcoin > 50)
        {
            _coinType = CoinType.SilverCoin;
            if (CanGetCoinType(_coinType)) { return; }
        }
        if (randomcoin > 0)
        {
            _coinType = CoinType.BronzeCoin;
            if (CanGetCoinType(_coinType)) { return; }
        }


        if (CanGetCoinType(CoinType.SilverCoin)) 
        {
            _coinType = CoinType.SilverCoin;
            if (CanGetCoinType(_coinType)) { return; }
        }
        if (CanGetCoinType(CoinType.GoldCoin))
        {
            _coinType = CoinType.GoldCoin;
            if (CanGetCoinType(_coinType)) { return; }
        }

        _coinType = CoinType.BronzeCoin;

    }

    private void CreateCoinsPool()
    {
        bronzeCoinPool = new List<GameObject>();
        silverCoinPool = new List<GameObject>();
        goldCoinPool = new List<GameObject>();
        if (!IsServer) { return; }

        fullSize = initialBronzeCoinPoolSize + initialSilverCoinPoolSize + initialGoldCoinPoolSize;

        for (int i = 0; i < initialBronzeCoinPoolSize; i++)
        {
            GameObject coin = Instantiate(bronzeCoinPrefab);
            coin.GetComponent<Coin>().coinType = CoinType.BronzeCoin;
            coin.SetActive(false);
            bronzeCoinPool.Add(coin);
        }

        for (int i = 0; i < initialSilverCoinPoolSize; i++)
        {
            GameObject coin = Instantiate(silverCoinPrefab);
            coin.GetComponent<Coin>().coinType = CoinType.SilverCoin;
            coin.SetActive(false);
            silverCoinPool.Add(coin);
        }

        for (int i = 0; i < initialGoldCoinPoolSize; i++)
        {
            GameObject coin = Instantiate(goldCoinPrefab);
            coin.GetComponent<Coin>().coinType = CoinType.GoldCoin;
            coin.SetActive(false);
            goldCoinPool.Add(coin);
        }
    }

    private bool CanGetCoinType(CoinType state)
    {
        switch (state)
        {
            case CoinType.BronzeCoin:
                return currentBronzeSpawnedCoins < initialBronzeCoinPoolSize;
            case CoinType.SilverCoin:
                return currentSilverSpawnedCoins < initialSilverCoinPoolSize;
            case CoinType.GoldCoin:
                return currentGoldSpawnedCoins < initialGoldCoinPoolSize;
            default: return false;
        }
    }

    private GameObject GetCoinByType()
    {
        switch (_coinType)
        {
            case CoinType.BronzeCoin:
                for (int i = 0; i < bronzeCoinPool.Count; i++)
                {
                    if (!bronzeCoinPool[i].activeSelf)
                    {
                        currentBronzeSpawnedCoins++;
                        return bronzeCoinPool[i];
                    }
                }
                break;
            case CoinType.SilverCoin:
                for (int i = 0; i < silverCoinPool.Count; i++)
                {
                    if (!silverCoinPool[i].activeSelf)
                    {
                        currentSilverSpawnedCoins++;
                        return silverCoinPool[i];
                    }
                }
                break;
            case CoinType.GoldCoin:
                for (int i = 0; i < goldCoinPool.Count; i++)
                {
                    if (!goldCoinPool[i].activeSelf)
                    {
                        currentGoldSpawnedCoins++;
                        return goldCoinPool[i];
                    }
                }
                break;
        }
        Debug.Log("SomeThingGoWrong");
        return null;
    }
}