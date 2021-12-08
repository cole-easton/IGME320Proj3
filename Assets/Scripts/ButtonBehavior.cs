using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Proj3
{
    public class ButtonBehavior : MonoBehaviour, IPointerClickHandler
    {
        //[SerializeField]
        public int levelNumber;
        private float halfWidth;
        private float halfHeight;
        private float xMax;
        private float xMin;
        private float yMax;
        private float yMin;

        public int LevelNumber
        {
            set
            {
                levelNumber = value;
            }
        }

        
        public void OnPointerClick(PointerEventData eventData)
        {
            SceneManager.LoadScene(levelNumber - 1); //might have to change to just levelNumber if buildIndexes are changed to include levelSelect scene
        }
        

        public void LoadLevel()
        {
            if (SceneManager.GetActiveScene().name == "LevelSelect")
                SceneManager.LoadScene(levelNumber - 1); //might have to change to just levelNumber if buildIndexes are changed to include levelSelect scene
            else
                if (SceneManager.GetActiveScene().buildIndex >= SceneManager.sceneCountInBuildSettings - 1)
                    SceneManager.LoadScene(0);
                else
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        // Start is called before the first frame update
        void Start()
        {
            halfWidth = GetComponent<RectTransform>().rect.width / 2.0f;
            halfHeight = GetComponent<RectTransform>().rect.height / 2.0f;
            xMax = GetComponent<RectTransform>().localPosition.x + halfWidth;
            xMin = GetComponent<RectTransform>().localPosition.x - halfWidth;
            yMax = GetComponent<RectTransform>().localPosition.x + halfHeight;
            yMin = GetComponent<RectTransform>().localPosition.x - halfHeight;            
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Input.mousePosition;
                if (mousePos.x < xMax && mousePos.x > xMin && mousePos.y < yMax && mousePos.y > yMin)
                {
                    LoadLevel();
                }
                if (levelNumber == 1)
                {
                    Debug.Log(mousePos.x + ", " + mousePos.y);
                    Debug.Log(xMin + " " + xMax + " " + yMin + " " + yMax);
                }
            }
        }
    }
}
