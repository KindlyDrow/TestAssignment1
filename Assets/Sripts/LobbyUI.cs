using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button createLobby;
    [SerializeField] private Button quickJoinLobby;
    [SerializeField] private Button joinCodeButton;
    [SerializeField] private Button refreshLobbyListButton;
    [SerializeField] private TMP_InputField joinCodeInputField;
    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private LobbyCreateUI lobbyCreateUI;
    [SerializeField] private GameObject lobbyContainer;
    [SerializeField] private GameObject lobbyTemplate;

    private void Awake()
    {
        createLobby.onClick.AddListener(CreateLobby);
        quickJoinLobby.onClick.AddListener(QuickJoinLobby);
        joinCodeButton.onClick.AddListener(JoinWithCode);
        refreshLobbyListButton.onClick.AddListener(RefreshLobbyList);
        playerNameInputField.onValueChanged.AddListener((string nexText) =>
        {
            MultiplayerManager.Instance.SetPlayerName(nexText);
        });
        lobbyTemplate.SetActive(false);
    }

    private void Start()
    {
        playerNameInputField.text = MultiplayerManager.Instance.GetPlayerName();
        GameLobby.Instance.OnLobbyListChanged += GameLobby_OnLobbyListChanged;
        UpdateLobbyList(new List<Lobby>());
    }

    private void GameLobby_OnLobbyListChanged(object sender, GameLobby.OnLobbyListChangedEventArgs e)
    {
        UpdateLobbyList(e.lobbyList);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList)
    {
        foreach (Transform child in lobbyContainer.transform)
        {
            if (child == lobbyTemplate.transform) continue;
            Destroy(child.gameObject);
        }

        foreach (Lobby lobby in lobbyList)
        {
            GameObject lobbyGO = Instantiate(lobbyTemplate, lobbyContainer.transform);
            lobbyGO.SetActive(true);
            lobbyGO.GetComponent<LobbyListSingleUI>().SetLobby(lobby);
        }
    }

    private void RefreshLobbyList()
    {
        GameLobby.Instance.ListLobbies();
    }

    private void CreateLobby()
    {
        lobbyCreateUI.Show();
    } 
    
    private void QuickJoinLobby()
    {
        GameLobby.Instance.QuickJoinLobby();
    }

    private void JoinWithCode()
    {
        GameLobby.Instance.JoinWithCode(joinCodeInputField.text);
    }

    private void OnDestroy()
    {
        GameLobby.Instance.OnLobbyListChanged -= GameLobby_OnLobbyListChanged;
    }
}
