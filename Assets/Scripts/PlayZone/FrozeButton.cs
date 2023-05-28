using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrozeButton : MonoBehaviour
{
    public Detector1 detector1;

    [SerializeField] Button[] frozenButtons;
    [SerializeField] Sprite[] frozeVisualLevels;
    private int[] frozenButtonLevel = {0, 0, 0, 0};
    private int[] frozenButtonMaxLevel = {0, 0, 0, 0};

    // When froze GO was triggered, frozen button appears
    public void FrozeButtonWithIndex(int index/*, frozeLevel*/)
    {
        frozenButtons[index].gameObject.SetActive(true);
        frozenButtonLevel[index] = 3;
        frozenButtonMaxLevel[index] = 3;
    }

    // When frozen button was pressed, it melts
    public void meltButtonWithIndex(int index)
    {
        frozenButtonLevel[index]--;

        if (frozenButtonLevel[index] == 0) {
            frozenButtons[index].gameObject.SetActive(false);
            frozenButtons[index].gameObject.GetComponent<Image>().sprite = frozeVisualLevels[frozeVisualLevels.Length - 1];
        } else {
            frozenButtons[index].gameObject.GetComponent<Image>().sprite = frozeVisualLevels[(int) (((float) (frozenButtonLevel[index]-1) / frozenButtonMaxLevel[index]) * frozeVisualLevels.Length)];
        }
    }

    // Only for not touch screens, to check if button is pressed or melt
    public void keyCheck(int index) {
        if (frozenButtonLevel[index] == 0) {
            detector1.CheckTrigger(index);
        } else {
            meltButtonWithIndex(index);
        }
    }
}
