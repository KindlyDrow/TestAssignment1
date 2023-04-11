using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCreateUI : MonoBehaviour
{

    [SerializeField] private Button closeButton;
    [SerializeField] private Button createPublicButton;
    [SerializeField] private Button createPrivateButton;
    [SerializeField] private TMP_InputField lobbyNameInputField;

    private void Awake()
    {
        createPrivateButton.onClick.AddListener(CreatePrivate);   
        createPublicButton.onClick.AddListener(CreatePublic);
        closeButton.onClick.AddListener(Hide);
    }

    private void Start()
    {
        Hide();
    }

    private void CreatePrivate()
    {
        GameLobby.Instance.CreateLobby(lobbyNameInputField.text, true);
    }
    
    private void CreatePublic()
    {
        GameLobby.Instance.CreateLobby(lobbyNameInputField.text, false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
