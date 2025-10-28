using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyReBinder : MonoBehaviour
{
    [SerializeField] private KeyBindings keyBindings;
    [SerializeField] private GameObject keyCodeToDisplayGroup;

    bool isListening = false;
    int actionIDWhichIsListened;
    
    private void Start() {
        for (int i = 0; i < keyCodeToDisplayGroup.transform.childCount; i++) {
            UpdateKeyCode(i);
        }
    }

    private void Update()
    {
        if (isListening)
        {
            foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if(Input.GetKey(vKey))
                {
                    isListening = false;
                    keyBindings.keyBindingChecks[actionIDWhichIsListened].keyCode = vKey;
                    UpdateKeyCode(actionIDWhichIsListened);
                }
            }
        }
    }

    private void UpdateKeyCode(int keyBindID) {
        GameObject keyDisplayObject = keyCodeToDisplayGroup.transform.GetChild(keyBindID).gameObject;
        string keyText = keyBindings.keyBindingChecks[keyBindID].keyCode.ToString();
        
        // Update main text
        Text mainText = keyDisplayObject.GetComponent<Text>();
        if (mainText != null) {
            mainText.text = keyText;
        }
        
        // Update shadow text if it exists
        Text shadowText = keyDisplayObject.GetComponentInChildren<Text>();
        if (shadowText != null && shadowText != mainText) {
            shadowText.text = keyText;
        }
    }

    public void ChangeKey(int actionID) {
        if (isListening) return;
        isListening = true;
        actionIDWhichIsListened = actionID;
    }    
}
