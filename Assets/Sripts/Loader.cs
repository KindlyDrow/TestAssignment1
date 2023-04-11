using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{

    public enum Scene
    {
        LobbyScene,
        CharacterSelectScene,
        GameScene,
    }

    public static void LoadNetwork(Scene targetScene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    }

    public static void Load(Scene targetScene)
    {
        SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    }
}
