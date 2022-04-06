using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScene : MonoBehaviour
{
	[SerializeField]
	string sceneSelected;

	public void SwitchScene()
	{
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
