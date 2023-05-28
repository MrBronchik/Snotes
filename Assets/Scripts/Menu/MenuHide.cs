using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuHide : MonoBehaviour

{
    [SerializeField] float timeLerpKef;

    public void Show(GameObject go)
    {
        go.SetActive(true);
        StartCoroutine(ChangeAlpha(go, 0, 1, timeLerpKef, true));
    }

    public void Hide(GameObject go)
    {
        StartCoroutine(ChangeAlpha(go, 1, 0, timeLerpKef, false));
    }

    IEnumerator ChangeAlpha(GameObject go, float startAlpha, float endAlpha, float seconds, bool act) {
        float t = 0;
        while(t <= 1.0) {
            t += Time.deltaTime / seconds;
            go.transform.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(startAlpha, endAlpha, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        go.SetActive(act);
    }
}
   