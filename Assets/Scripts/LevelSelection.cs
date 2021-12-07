using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

namespace Proj3
{
    public class LevelSelection : MonoBehaviour
    {
        //private List<string> unlockedLevels;
        private string unlockedLevels;
        private int levelNum;
        [SerializeField]
        private string[] levelList;
        //[SerializeField]
        //private List<UnityEngine.UI.Text> levelTextObjList;
        [SerializeField]
        private UnityEngine.UI.Text levelTextObj;
        private GameObject level1Button;
        private GameObject level1ButtonText;

        // Start is called before the first frame update
        void Start()
        {        
            if (SceneManager.GetActiveScene().name == "LevelSelect")
            {
                try
                {
                    LoadPrefs();
                }
                catch
                {
                    unlockedLevels = "Level 1";
                }
                levelList = unlockedLevels.Split(',');
                levelNum = levelList.Length;

                level1Button = GameObject.Find("Level1Button");
                level1ButtonText = GameObject.Find("Text");
                for (int i = 0; i < levelList.Length; i++)
                {
                    if (i == 0)
                    {
                        level1ButtonText.GetComponent<UnityEngine.UI.Text>().text = levelList[i];
                    }
                    else
                    {
                        GameObject newLevelButton = Instantiate(level1Button);
                        Vector3 buttonPos = newLevelButton.GetComponent<RectTransform>().position;
                        buttonPos.y -= newLevelButton.GetComponent<RectTransform>().localScale.y;
                        newLevelButton.GetComponent<RectTransform>().position = buttonPos;
                        newLevelButton.GetComponentInChildren<UnityEngine.UI.Text>().text = levelList[i];
                    }
                }
                Destroy(levelTextObj);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SavePrefs() // called when a level is beaten, not called when the game is closed
        {
            levelNum = SceneManager.GetActiveScene().buildIndex + 1;
            if (!unlockedLevels.Contains(levelNum.ToString()))
            {
                unlockedLevels += ",Level " + levelNum;
                PlayerPrefs.SetString("Levels", unlockedLevels);
            }
        }

        public void LoadPrefs()
        {
            unlockedLevels = PlayerPrefs.GetString("Levels", "Level 1");
        }
    }
}
