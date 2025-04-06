using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarController : MonoBehaviour
{
    [SerializeField] private List<Image> hotbarImages;

    private void OnEnable()
    {
        AllGray();
    }

    public void AllGray()
    {
        for (int i = 0; i < hotbarImages.Count; i++)
        {
            hotbarImages[i].color = Color.gray;
        }
    }

    public void SelectHotbarImage(int index)
    {
        for (int i = 0; i < hotbarImages.Count; i++)
        {
            if (i == index)
            {
                hotbarImages[i].color = Color.white;
            }
            else
            {
                hotbarImages[i].color = Color.gray;
            }
        }
    }
}
