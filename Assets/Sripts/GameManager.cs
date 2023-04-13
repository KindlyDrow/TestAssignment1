using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    public event EventHandler OnGameEnd;
    public event EventHandler OnGameStarted;

    private Dictionary<ulong, bool> isPlayerAlive;

    public enum State
    {
        GamePreparing = default,
        GameStarted,
        GameEnd,
    }

    public State _state { get; private set; }

    private float coinSpawInterval;
    [SerializeField] private float coinSpawIntervalMax;
    public NetworkVariable<float> timer { private set; get; } = new NetworkVariable<float>(5f);

    private void Awake()
    {
        Instance = this;

        isPlayerAlive = new Dictionary<ulong, bool>();

        ChangeState(State.GamePreparing);
    }

    private void Update()
    {
        switch (_state)
        {
            case State.GamePreparing:
                
                if (IsServer) timer.Value -= Time.deltaTime;
                if (timer.Value < 0) ChangeState(State.GameStarted);
                break;
            case State.GameStarted:
                SpawnCoins();
                break;
            case State.GameEnd:
                OnGameEnd?.Invoke(this, EventArgs.Empty);
                break;
        }
    }

    private void ChangeState(State state)
    {
        EndState(_state);
        switch (state)
        {
            case State.GamePreparing:
                break;
            case State.GameStarted:
                OnGameStarted?.Invoke(this, EventArgs.Empty);
                break;
            case State.GameEnd:
                break;
        }
        _state = state;
    }

    private void EndState(State state)
    {
        switch (state)
        {
            case State.GamePreparing:
                break;
            case State.GameStarted:
                break;
            case State.GameEnd:
                break;
        }
    }

    private void SpawnCoins()
    {
       if(!IsServer) { return; }

        coinSpawInterval -= Time.deltaTime;
        if (coinSpawInterval < 0)
        {
            coinSpawInterval = coinSpawIntervalMax;
            if (CoinFactory.Instance.CanGetCoin())
            {
                MultiplayerGameHandler.Instance.SpawnCoinServerRpc();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetPlayerAliveServerRpc(bool isAlive, ServerRpcParams serverRpcParams = default)
    {
        SetPlayerAliveClientRpc(isAlive, serverRpcParams.Receive.SenderClientId);
    }

    [ClientRpc]
    private void SetPlayerAliveClientRpc(bool isAlive, ulong clientId)
    {
        isPlayerAlive[clientId] = isAlive;

        int playersAlive = 0;
        foreach (ulong playerClientId in isPlayerAlive.Keys)
        {
            if (isPlayerAlive[playerClientId])
            {
                playersAlive++;
            }
        }
        if (playersAlive < 2 && _state == State.GameStarted)
        {
            ChangeState(State.GameEnd);
        }
    }

    public Dictionary<ulong, bool> GetPlayerDictionary()
    {
        return isPlayerAlive;
    }
}