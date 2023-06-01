using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    public void ImportData(StreamReader file, PlayZoneHandler plh)
    {
        string identificator, arg1, arg2;

        while (true)
        {
            if ((identificator = file.ReadLine()) == null) break;

            switch (identificator)
            {
                case "N":
                    if ((arg1 = file.ReadLine()) == null) { Debug.LogError("Unexpected file ending!"); break; }
                    if ((arg2 = file.ReadLine()) == null) { Debug.LogError("Unexpected file ending!"); break; }
                    plh.PlaceNote(int.Parse(arg1), float.Parse(arg2));
                    break;

                case "F":
                    if ((arg1 = file.ReadLine()) == null) { Debug.LogError("Unexpected file ending!"); break; }
                    if ((arg2 = file.ReadLine()) == null) { Debug.LogError("Unexpected file ending!"); break; }
                    plh.PlaceFroze(int.Parse(arg1), float.Parse(arg2));
                    break;

                default:
                    Debug.LogWarning("Unknown object identificator, skip.");
                    break;
            }
        }
        file.Close();
    }
}