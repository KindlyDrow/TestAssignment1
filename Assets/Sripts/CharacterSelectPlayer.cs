using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectPlayer : MonoBehaviour
{
    [SerializeField] private PlayerVisual playerVisual;
    [SerializeField] private int playerIndex;
    [SerializeField] private GameObject readyMarker;
    [SerializeField] private Button kickButton;
    [SerializeField] private TextMeshProUGUI playerNameText;

    private void Awake()
    {
        kickButton.onClick.AddListener(KickPlayerFromLobby);
        kickButton.gameObject.SetActive(false);
    }

    private void KickPlayerFromLobby()
    {
        PlayerData playerData = MultiplayerManager.Instance.GetPlayerDataFromPlayerIndex(playerIndex);
        GameLobby.Instance.KickPlayer(playerData.playerId.ToString());
        MultiplayerManager.Instance.KickPlayer(playerData.clientId);
    }

    private void Start()
    {
        MultiplayerManager.Instance.OnPlayerDataNetworkListChanged += MultiplayerManager_OnPlayerDataNetworkListChanged;
        CharacterSelectUI.Instance.OnReadyChanged += CharacterSelectUI_OnReadyChanged;

        kickButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);

        UpdatePlayer();
    }

    private void CharacterSelectUI_OnReadyChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();
    }

    private void MultiplayerManager_OnPlayerDataNetworkListChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();
    }

    private void UpdatePlayer()
    {
        if (MultiplayerManager.Instance.IsPlayerIndexConnected(playerIndex))
        {
            Show();

            PlayerData playerData = MultiplayerManager.Instance.GetPlayerDataFromPlayerIndex(playerIndex);

            readyMarker.SetActive(CharacterSelectUI.Instance.IsPlayerReady(playerData.clientId));

            playerNameText.text = playerData.playerName.ToString();
            playerNameText.color = MultiplayerManager.Instance.GetPlayerColor(playerData.colorId);

            playerVisual.SetPlayercolor(MultiplayerManager.Instance.GetPlayerColor(playerData.colorId));
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        MultiplayerManager.Instance.OnPlayerDataNetworkListChanged -= MultiplayerManager_OnPlayerDataNetworkListChanged;
        CharacterSelectUI.Instance.OnReadyChanged -= CharacterSelectUI_OnReadyChanged;
    }

}
