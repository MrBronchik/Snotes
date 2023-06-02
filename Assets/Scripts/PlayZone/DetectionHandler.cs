using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionHandler : MonoBehaviour
{
    [SerializeField] GameObject m_functionalDetectorsParent;
    [SerializeField] GameObject m_visualDetector;

    private int m_nextChildID = 0;
    private PlayZoneHandler plh;

    private BoxCollider2D m_successDetector;
    private BoxCollider2D m_failureDetector;

    private void Start()
    {
        m_successDetector = m_functionalDetectorsParent.transform.GetChild(0).GetComponent<BoxCollider2D>();
        m_failureDetector = m_functionalDetectorsParent.transform.GetChild(1).GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (plh.levelIsCompleted) return;

        while (!plh.levelIsCompleted)
        {
            Transform objTrans = plh.m_objectStorer.transform.GetChild(m_nextChildID);
            if (m_successDetector.bounds.Contains(objTrans.position))
            {
                switch (objTrans.tag)
                {
                    case "RNote":
                        plh.detectedNotes.Add(objTrans.gameObject);
                        m_nextChildID++;
                        break;
                    case "GNote":
                        plh.detectedNotes.Add(objTrans.gameObject);
                        m_nextChildID++;
                        break;
                    case "BNote":
                        plh.detectedNotes.Add(objTrans.gameObject);
                        m_nextChildID++;
                        break;
                    case "ONote":
                        plh.detectedNotes.Add(objTrans.gameObject);
                        m_nextChildID++;
                        break;


                    case "RFroze":
                        m_nextChildID++;
                        plh.FreezeButton(0);
                        objTrans.gameObject.SetActive(false);
                        break;
                    case "GFroze":
                        m_nextChildID++;
                        plh.FreezeButton(1);
                        objTrans.gameObject.SetActive(false);
                        break;
                    case "BFroze":
                        m_nextChildID++;
                        plh.FreezeButton(2);
                        objTrans.gameObject.SetActive(false);
                        break;
                    case "OFroze":
                        m_nextChildID++;
                        plh.FreezeButton(3);
                        objTrans.gameObject.SetActive(false);
                        break;

                    case "End":
                        plh.LevelCompleted();
                        break;

                    default:
                        Debug.Log("Unknown object detected, skip.");
                        m_nextChildID++;
                        break;
                }
            }
            else
            {
                break;
            }
        }

        while(true)
        {
            if (plh.detectedNotes.Count == 0) break;

            GameObject obj = plh.detectedNotes[0];

            if (m_failureDetector.bounds.Contains(obj.transform.position))
            {
                plh.detectedNotes.RemoveAt(0);

                plh.NoteSkipped();
            }
            else
            {
                break;
            }
        }
    }

    public void PassReference(PlayZoneHandler _plh)
    {
        plh = _plh;
    }
    public void SetDetectorThickness(float thicknessRatio, float scrWidth)
    {
        m_functionalDetectorsParent.transform.localScale = new Vector3(
            scrWidth * thicknessRatio,
            m_functionalDetectorsParent.transform.localScale.y,
            m_functionalDetectorsParent.transform.localScale.z
            );

        m_visualDetector.transform.localScale = new Vector3(
            thicknessRatio,
            m_visualDetector.transform.localScale.y,
            m_visualDetector.transform.localScale.z
            );
    }
}
