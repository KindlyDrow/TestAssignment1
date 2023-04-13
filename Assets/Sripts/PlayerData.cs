using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public struct PlayerData : IEquatable<PlayerData>, INetworkSerializable
{

    public ulong clientId;
    public int colorId;
    public FixedString32Bytes playerName;
    public FixedString32Bytes playerId;
    public int playerScore;

    public bool Equals(PlayerData other)
    {
        return 
            clientId == other.clientId && 
            colorId == other.colorId && 
            playerName == other.playerName &&
            playerId == other.playerId && 
            playerScore == other.playerScore;

    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientId);
        serializer.SerializeValue(ref colorId);
        serializer.SerializeValue(ref playerName);
        serializer.SerializeValue(ref playerId);
        serializer.SerializeValue(ref playerScore);
    }
}
