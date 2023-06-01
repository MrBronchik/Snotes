using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is for circled buttons on the screen
public class ButtonActivator : MonoBehaviour
{
    [Header ("Other Scripts")]
    //[SerializeField] FrozeButton frozeButton;
    [SerializeField] PauseGameCS pauseGameCS;

    private InputManager inputManagerInst;

    private void Start() {
        inputManagerInst = InputManager.instance;
    }

#region Non-touch screen
    private void Update() {
        if (inputManagerInst.GetKeyDown(KeyBindingActions.Pause)) {
            if (!pauseGameCS.gamePaused) {
                pauseGameCS.PauseGame();
            } else {
                pauseGameCS.UnPauseGame();
            }
        } else if (inputManagerInst.GetKeyDown(KeyBindingActions.FirstBut)) {
            //frozeButton.keyCheck(0);
        } else if (inputManagerInst.GetKeyDown(KeyBindingActions.SecondBut)) {
            //frozeButton.keyCheck(1);
        } else if (inputManagerInst.GetKeyDown(KeyBindingActions.ThirdBut)) {
            //frozeButton.keyCheck(2);
        } else if (inputManagerInst.GetKeyDown(KeyBindingActions.FourthBut)) {
            //frozeButton.keyCheck(3);
        }
    }
#endregion

    public void OnCircledButtonClick(int id) {
        //detector1.CheckTrigger(id);
    }
}
