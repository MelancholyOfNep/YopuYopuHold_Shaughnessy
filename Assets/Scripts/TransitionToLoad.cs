using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionToLoad : MonoBehaviour
{
	[SerializeField]
	string sceneSelected;
	LevelToLoad ltlScript;

	public void SwitchScene()
	{
		ltlScript = GameObject.Find("SceneSelector").GetComponent<LevelToLoad>();
		sceneSelected = ltlScript.scene;
		Time.timeScale = 1.0f;
		StartCoroutine(AsyncSceneLoad());
	}

	IEnumerator AsyncSceneLoad()
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneSelected);

		// Wait until the scene fully loads
		while (!asyncLoad.isDone)
			yield return null;
	}
}
