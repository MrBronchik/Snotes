using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSwipe : MonoBehaviour
{
    private RectTransform menuLocation; 
    private Vector2 lastPointerPos;
    private bool isPressed = false;
    private Vector2 startPointerPos;
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
        // gets input from touch
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (!isPressed) {
                isPressed = true;
                startPointerPos = cameraUI.ScreenToWorldPoint(new Vector2 (touch.position.x, touch.position.y));
            }
            lastPointerPos = cameraUI.ScreenToWorldPoint(new Vector2 (touch.position.x, touch.position.y));
        }
        // gets input from mouse
        else if (Input.GetMouseButton(0))
        {
            Debug.Log("Mouse pressed");
            if (!isPressed) {
                isPressed = true;
                startPointerPos = cameraUI.ScreenToWorldPoint(new Vector2 (Input.mousePosition.x, Input.mousePosition.y));
            }
            lastPointerPos = cameraUI.ScreenToWorldPoint(new Vector2 (Input.mousePosition.x, Input.mousePosition.y));
        }
        else
        {
            if (isPressed) {
                isPressed = false;
                if (startCollider.OverlapPoint(startPointerPos) && endCollider.OverlapPoint(lastPointerPos)) {
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
   