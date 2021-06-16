using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skrzynia : Interactable
{
    public List<Item> items = new List<Item>();
    public MeshRenderer meshrenderer;
    public Animator anim;

    public override void Start()
    {

        OutlineWidth = meshrenderer.materials[0].GetFloat("Szerokośćobrysu");
        renderer.materials[0].SetFloat("Szerokośćobrysu", 0);
        if (PlayerPrefs.HasKey(GameController.instance.player.name + "_OpenedSkrzynia1"))
            isInteractable = false;
    }
    public override void Interact()
    {
        if (isInteractable)
        {
            PlayerPrefs.SetString(GameController.instance.player.name + "_OpenedSkrzynia1", "0");
            base.Interact();
            anim.SetTrigger("Open");
            foreach (Item i in items)
            {
                Inventory.instnace.Add(i);
            }
        }

    }
}
