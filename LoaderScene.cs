using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderScene : MonoBehaviour
{
    public GameController controller;

    private void Awake
    {
        if(GameController.intance == null)
        {
        Intantiate(controller);
        }
        GameController.intance.RespawnPlayer();
        GameController.intance.ShowGUI();

    }

}
