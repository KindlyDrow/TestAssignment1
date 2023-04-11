using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMasageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(Hide);
    }

    private void Start()
    {
        MultiplayerManager.Instance.OnFailedToJoingGame += MultiplayerManager_OnFailedToJoingGame;
        MultiplayerManager.Instance.OnTryingToJoingGame += MultiplayerManager_OnTryingToJoingGame;
        GameLobby.Instance.OnCreateLobbyStarted += GameLobby_OnCreateLobbyStarted;
        GameLobby.Instance.OnCreateLobbyFailed += GameLobby_OnCreateLobbyFailed;
        GameLobby.Instance.OnJoinStarted += GameLobby_OnJoinStarted;
        GameLobby.Instance.OnJoinFailed += GameLobby_OnJoinFailed;
        Hide();
    }

    private void GameLobby_OnJoinFailed(object sender, System.EventArgs e)
    {
        if (NetworkManager.Singleton.DisconnectReason == "")
        {
            ShowMessageWithCloseButton("Failed to join lobby");
        }
        else
        {
            ShowMessageWithCloseButton(NetworkManager.Singleton.DisconnectReason);
        }
    }

    private void GameLobby_OnJoinStarted(object sender, System.EventArgs e)
    {
        ShowMessage("Joining lobby...");
    }

    private void GameLobby_OnCreateLobbyFailed(object sender, System.EventArgs e)
    {
        if (NetworkManager.Singleton.DisconnectReason == "")
        {
            ShowMessageWithCloseButton("Failed to create lobby");
        }
        else
        {
            ShowMessageWithCloseButton(NetworkManager.Singleton.DisconnectReason);
        }
    }

    private void GameLobby_OnCreateLobbyStarted(object sender, System.EventArgs e)
    {
        ShowMessage("Creating lobby");
    }

    private void MultiplayerManager_OnTryingToJoingGame(object sender, System.EventArgs e)
    {
        ShowMessage("CONNECTING...");
    }

    private void MultiplayerManager_OnFailedToJoingGame(object sender, System.EventArgs e)
    {

        if (NetworkManager.Singleton.DisconnectReason == "")
        {
            ShowMessageWithCloseButton("Failed to connect");
        }
        else
        {
            ShowMessageWithCloseButton(NetworkManager.Singleton.DisconnectReason);
        }
    }

    private void ShowMessage(string message)
    {
        Show();
        messageText.text = message;
    }

    private void ShowMessageWithCloseButton(string message)
    {
        Show();
        ShowCloseButton();
        messageText.text = message;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        HideCloseButton();
        messageText.text = ("CONNECTING...");
        gameObject.SetActive(false);
    }

    private void ShowCloseButton()
    {
        closeButton.gameObject.SetActive(true);
    }

    private void HideCloseButton()
    {
        closeButton.gameObject.SetActive(false);
    }


    private void OnDestroy()
    {
        MultiplayerManager.Instance.OnFailedToJoingGame -= MultiplayerManager_OnFailedToJoingGame;
        MultiplayerManager.Instance.OnTryingToJoingGame -= MultiplayerManager_OnTryingToJoingGame;
        GameLobby.Instance.OnCreateLobbyStarted -= GameLobby_OnCreateLobbyStarted;
        GameLobby.Instance.OnCreateLobbyFailed -= GameLobby_OnCreateLobbyFailed;
        GameLobby.Instance.OnJoinStarted -= GameLobby_OnJoinStarted;
        GameLobby.Instance.OnJoinFailed -= GameLobby_OnJoinFailed;
    }
}
