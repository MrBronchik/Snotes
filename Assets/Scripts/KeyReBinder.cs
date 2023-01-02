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
        keyCodeToDisplayGroup.transform.GetChild(keyBindID).gameObject.GetComponent<Text>().text = keyBindings.keyBindingChecks[keyBindID].keyCode.ToString();
    }

    public void ChangeKey(int actionID) {
        if (isListening) return;
        isListening = true;
        actionIDWhichIsListened = actionID;
    }    
}
