using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSwipe : MonoBehaviour
{
    private RectTransform menuLocation; 
    private Vector2 lastTouchPos;
    private bool isPressed = false;
    private Vector2 startTouchPos;
    [SerializeField] Collider2D startCollider;
    [SerializeField] Collider2D endCollider;
    [SerializeField] Camera cameraUI;
    [SerializeField] float timeLerpKef;

    void Start()
    {
        menuLocation = this.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch(0);
            if (!isPressed) {
                isPressed = true;
                startTouchPos = cameraUI.ScreenToWorldPoint(new Vector2 (touch.position.x, touch.position.y));
            }
            lastTouchPos = cameraUI.ScreenToWorldPoint(new Vector2 (touch.position.x, touch.position.y));
        } else {
            if (isPressed) {
                isPressed = false;
                if (startCollider.OverlapPoint(startTouchPos) && endCollider.OverlapPoint(lastTouchPos)) {
                    SwipeToBook();
                }
            }
        }
    }

    void SwipeToBook()
    {
        StartCoroutine(SmoothMove(0, -720, timeLerpKef));
    }

    public void SwipeToMenu()
    {
        StartCoroutine(SmoothMove(-720, 0, timeLerpKef));
    }

    IEnumerator SmoothMove(float startpos, float endpos, float seconds) {
        float t = 0;
        while(t <= 1.0) {
            t += Time.deltaTime / seconds;
            menuLocation.anchoredPosition = new Vector2 (0, Mathf.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t)));
            yield return null;
        }
    }
}
   