using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI loseText;
    [SerializeField] private GameObject infoContainer;
    [SerializeField] private GameObject infoTemplate;
    [SerializeField] private Button backButton;

    private void Awake()
    {
        backButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.LobbyScene);
        });
    }

    private void Start()
    {
        GameManager.Instance.OnGameEnd += GameManager_OnGameEnd;
        winText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void GameManager_OnGameEnd(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        SetPlayerInfo();
    }

    private void SetPlayerInfo()
    {
        foreach (Transform child in infoContainer.transform)
        {
            if (child == infoTemplate.transform) continue;
            Destroy(child.gameObject);
        }

        Dictionary<ulong,bool> players = GameManager.Instance.GetPlayerDictionary();
        Dictionary<string, int> playersNotSorted = new Dictionary<string, int>();

        foreach (ulong playerId in players.Keys)
        {
            
            int playerScore = MultiplayerManager.Instance.GetPlayerDataFromClientId(playerId).playerScore;
            string playerName = MultiplayerManager.Instance.GetPlayerDataFromClientId(playerId).playerName.ToString();
            playersNotSorted[playerName] = playerScore;

        }

        Dictionary<string, int> playersSorted = playersNotSorted.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        int index = 0;

        foreach (var player in playersSorted)
        {
            index++;
            GameObject infoGO = Instantiate(infoTemplate, infoContainer.transform);
            infoGO.SetActive(true);
            infoGO.GetComponent<PlayerInfoSingleUI>().SetPlayerInfo(player.Key, player.Value, index);
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameEnd -= GameManager_OnGameEnd;
    }
}
