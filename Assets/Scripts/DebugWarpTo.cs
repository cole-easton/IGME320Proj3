using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Proj3
{
	/// <summary>
	/// Teleports the given obj to the given destination obj.
	/// </summary>
	public class DebugWarpTo : MonoBehaviour
	{
		/// <summary>
		/// The point to warp to.
		/// </summary>
		//[SerializeField]
		//[Tooltip("The point to warp to.")]
		//private GameObject warpPoint;

		/// <summary>
		/// The object to warp.
		/// </summary>
		//[SerializeField]
		//[Tooltip("The object to warp.")]
		//private GameObject playerObj;

		/// <summary>
		/// The text box for respawn info.
		/// </summary>
		[SerializeField]
		[Tooltip("The text box for respawn info.")]
		private UnityEngine.UI.Text respawnTextObj;

		/// <summary>
		/// Default respawn text.
		/// </summary>
		[SerializeField]
		[Tooltip("Default respawn text.")]
		private string respawnText = "Click and drag to get the particle stream to the receptor. Press R to reset.\nIf the cannon can be moved, you can control it with the arrow keys.";

		/// <summary>
		/// Default stage clear text.
		/// </summary>
		[SerializeField]
		[Tooltip("Default stage clear text.")]
		//private string clearText = "Congratulations! You've finished! To reset, press R.";
		private string clearText = "Congratulations! You've finished! To go to the next level, press R.";

		///// <summary>
		///// Levels, in order.
		///// </summary>
		//[SerializeField]
		//[Tooltip("Levels, in order.")]
		//private Scene[] levels;

		[SerializeField]
		private GameObject levelSelectionObject;
		private LevelSelection levelSelectionScript;

		private bool levelCleared = false;

		private GameObject nextLevelButton;
		private Button nextLevelButtonScript;

		void Start()
		{
			//if (warpPoint == null)
			//{
			//	Debug.LogWarning("warpPoint not set on " + this.gameObject.name + ". Warp point will be (0,0,0) until set.");
			//}
			//if (playerObj == null)
			//{
			//	Debug.LogWarning("playerObj not set on " + this.gameObject.name + ". The warp will not work until set.");
			//}
			//FindObjectOfType<CollectibleManager>().AllCollected += DebugWarpTo_AllCollected;
			respawnTextObj.text = respawnText;
			FindObjectOfType<CannonController>().OnReceptorReached += DebugWarpTo_ReceptorReached;

			levelSelectionScript = levelSelectionObject.GetComponent<LevelSelection>();

			nextLevelButton = GameObject.Find("NextLevelButton");
			nextLevelButtonScript = nextLevelButton?.GetComponent<Button>();
			if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1 && nextLevelButton!=null)
				nextLevelButton.GetComponentInChildren<Text>().text = "Start Over";

			if (nextLevelButton != null)
				nextLevelButtonScript.interactable = false;
		}

		private void DebugWarpTo_ReceptorReached(object sender, System.EventArgs e)
		{
			if (respawnTextObj != null)
			{
				respawnTextObj.text = clearText;
				if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
					respawnTextObj.text = "Congratulations! You've finished! To start over, press R.";
			}
			levelCleared = true;
			if (nextLevelButtonScript != null)
				nextLevelButtonScript.interactable = true;
			if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
				levelSelectionScript.SavePrefs();
		}

		// Update is called once per frame
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				if (levelCleared)
				{
					if (respawnTextObj != null)
					{
						respawnTextObj.text = respawnText;
					}
					// Reload scene
					//SceneManager.LoadScene(SceneManager.GetActiveScene().name);
					if (SceneManager.GetActiveScene().buildIndex >= SceneManager.sceneCountInBuildSettings - 1)
						SceneManager.LoadScene(0);
					else
						SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
				}
				else
				{
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				}
			}
		}
	}
}
