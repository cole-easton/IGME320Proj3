using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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
        [SerializeField]
        private GameObject canvas;
        private List<GameObject> buttonList;

        // Start is called before the first frame update
        void Start()
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
            if (SceneManager.GetActiveScene().name == "LevelSelect")
            {
                level1Button = GameObject.Find("Level1Button");
                level1ButtonText = GameObject.Find("Text");
                buttonList = new List<GameObject>();
                for (int i = 0; i < levelList.Length; i++)
                {
                    if (i == 0)
                    {
                        level1ButtonText.GetComponent<UnityEngine.UI.Text>().text = levelList[i];
                        //string[] buttonText = level1ButtonText.GetComponent<UnityEngine.UI.Text>().text.Split(' ');
                        level1Button.GetComponent<ButtonBehavior>().levelNumber = i + 1;
                        //level1Button.GetComponent<ButtonBehavior>().levelNumber = int.Parse(buttonText[1]);
                        buttonList.Add(level1Button);
                    }
                    else
                    {
                        GameObject newLevelButton = Instantiate(level1Button, canvas.transform, false);
                        /*
                        Vector3 buttonPos = newLevelButton.GetComponent<RectTransform>().localPosition;
                        buttonPos.y -= 1.25f * newLevelButton.GetComponent<RectTransform>().rect.height;
                        newLevelButton.GetComponent<RectTransform>().localPosition = buttonPos;
                        */
                        newLevelButton.GetComponentInChildren<UnityEngine.UI.Text>().text = levelList[i];
                        //string[] buttonText = newLevelButton.GetComponent<UnityEngine.UI.Text>().text.Split(' ');
                        newLevelButton.GetComponent<ButtonBehavior>().levelNumber = i + 1;
                        //newLevelButton.GetComponent<ButtonBehavior>().levelNumber = int.Parse(buttonText[1]);
                        Debug.Log(i);
                        buttonList.Add(newLevelButton);
                    }
                }
                for (int i = 1; i <= buttonList.Count; i++)
                {
                    Vector3 buttonPos = buttonList[0].GetComponent<RectTransform>().localPosition;
                    int remainder = buttonList.Count % 3;
                    int rowQuotient = (i - 1) / 3;
                    buttonPos.y -= rowQuotient * buttonList[i - 1].GetComponent<RectTransform>().rect.height;
                    if (buttonList.Count - i < remainder)
                    {
                        switch (remainder)
                        {
                            case 1:
                                buttonPos.x = 0;
                                break;
                            case 2:
                                int rowRemainder = (i - 1) % remainder;
                                switch (rowRemainder)
                                {
                                    case 0:
                                        buttonPos.x = -0.5f * buttonList[i - 1].GetComponent<RectTransform>().rect.width;
                                        break;
                                    default:
                                        buttonPos.x = 0.5f * buttonList[i - 1].GetComponent<RectTransform>().rect.width;
                                        break;
                                }                                
                                break;
                            default:
                                break;
                        }

                    }
                    else
                    {
                        int rowRemainder = (i - 1) % 3;
                        buttonPos.x = (rowRemainder - 1) * buttonList[i - 1].GetComponent<RectTransform>().rect.width;
                    }
                    buttonList[i - 1].GetComponent<RectTransform>().localPosition = buttonPos;
                    //buttonList[i - 1].GetComponent<Button>().onClick.AddListener(() => { LoadLevel(i); });
                }
                Destroy(levelTextObj);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void LoadLevel(int levelNumber)
        {
            SceneManager.LoadScene("Level" + levelNumber);
            Debug.Log(levelNumber);
        }

        public void SavePrefs() // called when a level is beaten, not called when the game is closed
        {
            levelNum = SceneManager.GetActiveScene().buildIndex + 2;
            if (!unlockedLevels.Contains(levelNum.ToString()))
            {
                unlockedLevels += ",Level " + levelNum;
                PlayerPrefs.SetString("Levels", unlockedLevels);
                //Debug.Log(PlayerPrefs.GetString("Levels", "Level 1"));
            }
        }

        public void LoadPrefs()
        {
            unlockedLevels = PlayerPrefs.GetString("Levels", "Level 1");
            //Debug.Log(unlockedLevels);
        }
    }
}
