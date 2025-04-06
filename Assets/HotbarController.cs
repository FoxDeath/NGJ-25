using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotbarController : MonoBehaviour
{
    [SerializeField] private List<Image> hotbarImages;
    [SerializeField] private List<Image> hotbarIcons;
    [SerializeField] private List<TextMeshProUGUI> hotbarCosts;

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

    public void Initialize(List<TowerSO> towerConfigs)
    {
        for(int i = 0; i < towerConfigs.Count; i++)
        {
            if (i >= hotbarImages.Count || i >= hotbarIcons.Count || i >= hotbarCosts.Count)
            {
                Debug.LogWarning("Not enough UI elements in the hotbar to display all towers.");
                break;
            }
            
            hotbarIcons[i].sprite = towerConfigs[i].towerSprite;
            hotbarCosts[i].text = towerConfigs[i].attributes.cost.ToString();
        }
    }
}
