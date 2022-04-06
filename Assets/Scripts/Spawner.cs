using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public GameObject yopuPF;
	[SerializeField]
	GameManager manager;
	[SerializeField]
	LevelToLoad ltlScript;

	// Start is called before the first frame update
	void Start()
	{
		manager = GameObject.Find("GameManager").GetComponent<GameManager>();
		ltlScript = GameObject.Find("SceneSelector").GetComponent<LevelToLoad>();
		SpawnCycle();
	}

	public void SpawnCycle()
	{
		if(manager.DeleteCheck())
			StartCoroutine(Delete());

		StartCoroutine(Spawn());
	}

	IEnumerator Delete()
    {
		manager.ForceDrop();

		yield return new WaitUntil(() => !manager.FallCheck());

		if(manager.DeleteCheck())
        {
			StartCoroutine(Delete());
        }
    }

	IEnumerator Spawn()
    {
		yield return new WaitUntil(() => !manager.FallCheck() && !manager.DeleteCheck());
		if(GameOverCheck())
        {
			Debug.LogWarning("Fail");
			ltlScript.scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
			UnityEngine.SceneManagement.SceneManager.LoadScene("LossScene");
        }
		Instantiate(yopuPF, transform.position, Quaternion.identity).GetComponent<YopuSet>();
    }

	public void HoldSpawn()
    {
		if (GameOverCheck())
		{
			Debug.LogWarning("Fail");
			ltlScript.scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
			UnityEngine.SceneManagement.SceneManager.LoadScene("LossScene");
		}
		Instantiate(yopuPF, transform.position, Quaternion.identity).GetComponent<YopuSet>();
	}

	public bool GameOverCheck()
    {
		if (manager.board[(int)transform.position.x, (int)transform.position.y] != null
			|| manager.board[(int)transform.position.x + 1, (int)transform.position.y] != null)
			return true;
		else return false;
    }
}
