using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyListSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI lobbyAvailablSlotsText;
    private Lobby _lobby;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(JoinLobby);
    }

    private void JoinLobby()
    {
        GameLobby.Instance.JoinWithId(_lobby.Id);
    }

    public void SetLobby(Lobby lobby)
    {
        _lobby = lobby;
        lobbyNameText.text = lobby.Name;
        lobbyAvailablSlotsText.text = (lobby.MaxPlayers - lobby.AvailableSlots).ToString() + "/" + lobby.MaxPlayers;
    }
}
