using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelToLoad : MonoBehaviour
{
	static LevelToLoad original;
	public string scene;

	private void Awake()
	{
		if(original != this)
		{
			if (original != null)
				Destroy(original.gameObject);
			DontDestroyOnLoad(this.gameObject);
			original = this;
			scene = SceneManager.GetActiveScene().name;
		}
	}
}
