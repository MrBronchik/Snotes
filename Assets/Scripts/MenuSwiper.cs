using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSwiper : MonoBehaviour
{
    private RectTransform menuLocation;
    private float previousTouchPositionY;
    private bool isPressed = false;
    private float startAnchorPosY;
    [SerializeField] float timeLerpKef;

    // Start is called before the first frame update
    void Awake()
    {
        menuLocation = GetComponent<RectTransform>();
    }

    private void Update() {

        if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch(0);
            if (isPressed) {
                // Sets new position of menu
                float difference = previousTouchPositionY - touch.position.y;
                menuLocation.anchoredPosition = new Vector2(0, Mathf.Max( Mathf.Min(menuLocation.anchoredPosition.y-difference, 0), -720));
            } else {
                // Memorises original position of menu
                isPressed = true;
                startAnchorPosY = menuLocation.anchoredPosition.y;
            }
            previousTouchPositionY = touch.position.y;

        } else if (Input.touchCount != 1) {

            // Script decides:
            //      - menu has to be passed to the next page
            //      - menu has to be returned to the previous page
            if (isPressed) {
                if (Mathf.Abs(startAnchorPosY - menuLocation.anchoredPosition.y) > 150) // Next page
                {
                    if (startAnchorPosY == 0){
                        StartCoroutine(SmoothContinuousMove(0, -720, Mathf.Abs(menuLocation.anchoredPosition.y - startAnchorPosY) / 600 * timeLerpKef ));
                    } else {
                        StartCoroutine(SmoothContinuousMove(-720, 0, Mathf.Abs(startAnchorPosY - menuLocation.anchoredPosition.y) / 600 * timeLerpKef ));
                    }

                } else {    // Previous page
                    if (startAnchorPosY == 0){
                        StartCoroutine(SmoothContinuousMove(-720, 0, /*Mathf.Abs(startAnchorPosY - menuLocation.anchoredPosition.y) / 600 * */timeLerpKef ));
                    } else {
                        StartCoroutine(SmoothContinuousMove(0, -720, /*Mathf.Abs(menuLocation.anchoredPosition.y - startAnchorPosY) / 600 * */timeLerpKef ));
                    }
                }
            }
            isPressed = false;
        }
    }

    // For smooth transfer between pages with not touch screen
    public void MoveTo(float endpos_) {
        StartCoroutine(SmoothMove(menuLocation.anchoredPosition.y, endpos_, timeLerpKef));
    }

    IEnumerator SmoothMove(float startpos, float endpos, float seconds) {
        float t = 0;
        while(t <= 1.0) {
            t += Time.deltaTime / seconds;
            menuLocation.anchoredPosition = new Vector2 (0, Mathf.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t)));
            yield return null;
        }
    }

    IEnumerator SmoothContinuousMove(float startpos, float endpos, float seconds) {
        float t = seconds;
        while(t <= 1.0) {
            t += Time.deltaTime / seconds;
            menuLocation.anchoredPosition = new Vector2 (0, Mathf.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t)));
            yield return null;
        }
    }
}
