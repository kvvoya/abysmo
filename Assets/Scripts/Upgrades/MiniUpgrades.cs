using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniUpgrades : MonoBehaviour
{
    Image[] upgradeIcons;
    [SerializeField] float offOpacity;
    [SerializeField] float onOpacity;

    private void Start()
    {
        upgradeIcons = GetComponentsInChildren<Image>();

        for (int i = 0; i < 9; i++)
        {
            DoAction(i, false);
        }
    }

    public void DoAction(int index, bool enable)
    {
        Image targetImage = upgradeIcons[index];
        if (enable)
        {
            targetImage.color = new Color(1, 1, 1, onOpacity);
        }
        else
        {
            targetImage.color = new Color(1, 1, 1, offOpacity);
        }
    }
}
