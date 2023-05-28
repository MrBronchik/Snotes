using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuOfLevels : MonoBehaviour
{
    [Header ("Options")]
    [SerializeField] GameObject lvlParent;
    [SerializeField] GameObject lvlSmallModel;
    [SerializeField] GameObject lvlBigModel;
    [SerializeField] GameObject dataStorer;
    [SerializeField] int elementsInColumn;
    [SerializeField] float gapBetweenElements;
    [SerializeField] float yFromAbove;
    [SerializeField] Button nextPageButton;
    [SerializeField] Button previousPageButton;
    [SerializeField] Sprite basicLvlLogo;

    [Header ("Menu Objects")]
    [SerializeField] GameObject mainMenuGO;
    [SerializeField] GameObject songMenuGO;

    private int page = 0;
    private int maxPage;
    private int diffLevel = 0;
    private string activeLvlDirectory;
    private GameObject presentSelectedLvl;

    private void Awake() {
        OnUpdate();
    }
    
    // Generates the book's page with available levels
    public void OnUpdate() { // NOT UPDATE FUNCTION!!!!!

        /*mainMenuGO.SetActive(false);
        songMenuGO.SetActive(true);*/

        // Clears the space for it
        foreach (Transform child in lvlParent.transform) {
            Destroy(child.gameObject);
        }

        string lvlFolderPath = Application.dataPath + "/Levels";

        string[] lvlDirectories = Directory.GetDirectories(lvlFolderPath, "*", SearchOption.TopDirectoryOnly);

        maxPage = (int) Mathf.Ceil( (float) lvlDirectories.Length / elementsInColumn);
        
        // Generating the levels on the page
        for(int i = 0; i < elementsInColumn; i++) 
        {
            int index = i + page*elementsInColumn;
            if (index >= lvlDirectories.Length) break;
            string lvlDirectory = lvlDirectories[index];

            GameObject lvlInfo = Instantiate(lvlSmallModel);
            RectTransform rtlvlModel = lvlSmallModel.GetComponent<RectTransform>();

            lvlInfo.gameObject.SetActive(true);
            lvlInfo.transform.SetParent(lvlParent.transform, true);
            lvlInfo.transform.localScale = new Vector3(1, 1, 1);
            lvlInfo.GetComponent<RectTransform>().anchoredPosition = new Vector3(
                                                                                    rtlvlModel.anchoredPosition.x,
                                                                                    rtlvlModel.anchoredPosition.y - (lvlInfo.GetComponent<RectTransform>().sizeDelta.y + gapBetweenElements)*i,
                                                                                    0
                                                                                );

            string[] musicFilePaths = Directory.GetFiles(lvlDirectory, "*.mp3");

            lvlInfo.name = Path.GetFileNameWithoutExtension(musicFilePaths[0]);
            
            lvlInfo.transform.GetChild(0).GetComponent<Image>().sprite = SetLogo(lvlDirectory);
            lvlInfo.transform.GetChild(1).GetComponent<Text>().text = lvlInfo.name;
            lvlInfo.transform.GetChild(2).name = lvlDirectory;
        }

        CheckButtons();
    }

    private Sprite SetLogo(string _path_) {
        Texture2D texture = LoadLvlsLogo(_path_);
        if (texture == null) return basicLvlLogo;
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f);

        return Sprite.Create(texture, rect, pivot);
    }

    private Texture2D LoadLvlsLogo(string _path)
    {
        Texture2D texture = new Texture2D(32, 32);

        if (File.Exists(_path + "/logo.png")) {

            texture.LoadImage(File.ReadAllBytes(_path + "/logo.png"));

        } else if (File.Exists(_path + "/logo.jpg")) {

            texture.LoadImage(File.ReadAllBytes(_path + "/logo.jpg"));

        } else if (File.Exists(_path + "/logo.jpeg")) {

            texture.LoadImage(File.ReadAllBytes(_path + "/logo.jpeg"));

        } else return null;

        return texture; 
    }

    public void NextPage() {
        page++;
        OnUpdate();
    }

    public void PreviousPage() {
        page--;
        OnUpdate();
    }

    // Checks arrows to switch buttons, does they have to be active or not
    private void CheckButtons() {
        nextPageButton.interactable = true;
        previousPageButton.interactable = true;

        if (page+1 == maxPage) {
            nextPageButton.interactable = false;
        } 
        if (page == 0) {
            previousPageButton.interactable = false;
        }
    }

    // If a level was selected from the given list
    public void SelectLevel(GameObject lvlSelected) {

        // Destroys previous selected level
        Destroy(presentSelectedLvl);

        // Generates new one
        presentSelectedLvl = Instantiate(lvlBigModel);
        presentSelectedLvl.transform.SetParent(lvlParent.transform.parent.transform, false);
        presentSelectedLvl.SetActive(true);

        activeLvlDirectory = lvlSelected.transform.GetChild(2).name;
        presentSelectedLvl.transform.GetChild(0).GetComponent<Image>().sprite = lvlSelected.transform.GetChild(0).GetComponent<Image>().sprite;
        presentSelectedLvl.transform.GetChild(1).GetComponent<Text>().text = lvlSelected.transform.GetChild(1).GetComponent<Text>().text;

        string[] difficultiesFilePaths = Directory.GetFiles(activeLvlDirectory, "*.txt");

        foreach (string difficultyPath in difficultiesFilePaths)
        { 
            string difficulty;
            using (StreamReader sr = new StreamReader(difficultyPath))
            {
                difficulty = sr.ReadLine();
            }

            // By deafault difficulty buttons are not active, so if .txt schedules are accessible, button becomes interactable
            presentSelectedLvl.transform.GetChild(2).transform.GetChild(int.Parse(difficulty)).transform.GetChild(0).name = difficultyPath;
            presentSelectedLvl.transform.GetChild(2).transform.GetChild(int.Parse(difficulty)).GetComponent<Button>().interactable = true;
        }
    }

    // When button with difficulty is pressed
    public void SetDifficulty(int levelOfDifficulty) {
        diffLevel = levelOfDifficulty;
        presentSelectedLvl.transform.GetChild(3).GetComponent<Button>().interactable = true;
    }

    // When button Play was pressed
    public void PlayPressed() {
        PlayerPrefs.SetString("txt path", presentSelectedLvl.transform.GetChild(2).transform.GetChild(diffLevel).transform.GetChild(0).name);
        PlayerPrefs.SetString("music path", Directory.GetFiles(activeLvlDirectory, "*.mp3")[0]);
        PlayerPrefs.SetString("lvl name", presentSelectedLvl.transform.GetChild(1).GetComponent<Text>().text);
        dataStorer.transform.GetChild(0).GetComponent<Image>().sprite = presentSelectedLvl.transform.GetChild(0).GetComponent<Image>().sprite;
        Loader.Load(Loader.Scene.PlayZone);
    }
}
