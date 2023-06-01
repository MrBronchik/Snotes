using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.LightingExplorerTableColumn;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] GameObject[] m_circleButtons;
    [SerializeField] Sprite[] m_freezeSprites;
    [SerializeField] Sprite[] m_freezeSpritesPressed;
    [SerializeField] Sprite[] m_originalButtonSprites;
    [SerializeField] Sprite[] m_originalButtonSpritesPressed;


    private int[] m_buttonsFrozeState = { 0, 0, 0, 0 };

    private int m_numOfFreezeStates;

    private PlayZoneHandler plh;

    public void PassReference(PlayZoneHandler _plh) { plh = _plh; }

    public void FreezeButton(int id)
    {
        m_buttonsFrozeState[id] = m_freezeSprites.Length;
        SetButtonSprite(id);
    }

    public void PressButton(int id)
    {
        if (m_buttonsFrozeState[id] != 0)
        {
            m_buttonsFrozeState[id]--;
            SetButtonSprite(id);
        }
        else
        {
            plh.ButtonPressed(id);
        }
    }

    private void SetButtonSprite(int id)
    {
        SpriteState spr = new SpriteState();
        if (m_buttonsFrozeState[id] == 0)
        {
            m_circleButtons[id].GetComponent<Image>().sprite = m_originalButtonSprites[id];
            spr.pressedSprite = m_originalButtonSpritesPressed[id];
            m_circleButtons[id].GetComponent<Button>().spriteState = spr;
        }
        else
        {
            m_circleButtons[id].GetComponent<Image>().sprite = m_freezeSprites[m_buttonsFrozeState[id] - 1];
            spr.pressedSprite = m_freezeSpritesPressed[m_buttonsFrozeState[id] - 1];
            m_circleButtons[id].GetComponent<Button>().spriteState = spr;
        }
    }
}
