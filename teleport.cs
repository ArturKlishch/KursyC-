using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    public Vector3 destination;
    public int mapID;
    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadSceneAsenc(mapID);
        GameController.instance.player.SetPosition(destination);
    }
    private IEnumerator.instance.FadeOut()
        {
         while(GameController.instance.fadeAnim.IsPlaying)
        {
        yield return null;
        }

    SceneManager.LoadSceneAnync(mapID);
    GameController.instance.player.SetPosition(destination);
        }
  

}
