using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
	[SerializeField]
	GameObject cheekibreeki, shamisen;
	string scene;

	// Start is called before the first frame update
	void Start()
	{
		scene = GameObject.Find("SceneSelector").GetComponent<LevelToLoad>().scene;
		if (scene == "Level1" || scene == "Level2" || scene == "Level3")
			Instantiate(shamisen);
		if (scene == "Level4" || scene == "Level5" || scene == "Level6")
			Instantiate(cheekibreeki);
	}
}
