using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
	// array for the board
	public Transform[,] board = new Transform[6, 13];

	public float dropSpeed;
	public bool paused;
	public float speedUp;

	[SerializeField]
	GameObject pauseObj;

	[SerializeField]
	LevelToLoad ltlScript;

	[SerializeField]
	TextMeshProUGUI uiScore;

	[SerializeField]
	GameObject breakFX;

	[SerializeField]
	AudioClip holdSFX;

	public int remainingYopus;

	public bool swapped = false;

	private void Start()
	{
		ltlScript = GameObject.Find("SceneSelector").GetComponent<LevelToLoad>();
		uiScore = GameObject.Find("ScoreUI").GetComponent<TextMeshProUGUI>();
		uiScore.text = "Remaining: " + (remainingYopus);
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			paused = !paused;
			PauseGame();
		}
		if (!paused && Input.GetKeyDown(KeyCode.Space))
			AudioSource.PlayClipAtPoint(holdSFX, Vector3.zero);
	}

	public void YopuRemainingDecrement()
	{
		remainingYopus--;
		uiScore.text = "Remaining: " + (remainingYopus);
		if (remainingYopus % 20 == 0)
			dropSpeed -= speedUp;
		if (remainingYopus <= 0)
		{
			ltlScript.scene = SceneManager.GetActiveScene().name;
			SceneManager.LoadScene("VictoryScene");
		}
	}

	void PauseGame()
	{
		if (paused)
		{
			Time.timeScale = 0f;
			pauseObj.SetActive(true);
		}
		else
		{
			Time.timeScale = 1f;
			pauseObj.SetActive(false);
		}
	}

	public bool BorderCheck(Vector3 target) 
	{
		if (target.x > -1 && target.x < 6 && target.y > -1 && target.y < 13)
			return true;
		else
			return false;
	}

	public bool SpaceCheck(Vector3 target, Transform parent) 
	{
		if (BorderCheck(target))
			if (board[(int)target.x, (int)target.y] == null
				|| board[(int)target.x, (int)target.y].parent == parent)
				return true;
			else return false;
		else return false;
	}

	public bool EmptyCheck(int x, int y) 
	{
		if (BorderCheck(new Vector3(x, y, 0)))
			if (board[x, y] == null)
				return true;
		return false;
	}

	public bool ColorMatch(int x, int y, Transform yopu) 
	{
		if (BorderCheck(new Vector3(x, y, 0)))
			if (board[x, y].GetComponent<YopuPiece>().colorDesignation == yopu.GetComponent<YopuPiece>().colorDesignation)
				return true;
		return false;
	}

	public bool AdjacencyMatchCheck(Vector2 pos, Vector3 dir, Transform pieceTrans) 
	{
		Vector2 newpos = new Vector2(pos.x + dir.x, pos.y + dir.y);
		if (!EmptyCheck((int)newpos.x, (int)newpos.y) && ColorMatch((int)newpos.x, (int)newpos.y, pieceTrans))
			return true;
		return false;
	}

	public void Move(float x, float y, Transform piece) 
	{
		board[(int)x, (int)y] = piece;
	}

	public void Clear(float x, float y) 
	{
		board[(int)x, (int)y] = null;
	}

	public void Delete(Transform yopu) 
	{
		Vector2 pos = new Vector2(Mathf.Round(yopu.position.x), Mathf.Round(yopu.position.y));
		Clear(pos.x, pos.y);
		Instantiate(breakFX, pos, Quaternion.identity);
		CamShake cam = GameObject.Find("Main Camera").GetComponent<CamShake>();
		cam.countdown = .5f;
		UnityEngine.Object.Destroy(yopu.gameObject);
	}

	void DeleteGroup(List<Transform> deletionGroup) 
	{
		foreach (Transform piece in deletionGroup)
			Delete(piece);
	}

	void AddNeighbor(Transform piece, List<Transform> group)
	{
		Vector3[] dirArr = { Vector3.up, Vector3.down, Vector3.right, Vector3.left };
		if (group.IndexOf(piece) == -1)
			group.Add(piece);
		else return;

		foreach (Vector3 dir in dirArr)
		{
			int x = Mathf.RoundToInt(piece.position.x) + Mathf.RoundToInt(dir.x);
			int y = Mathf.RoundToInt(piece.position.y) + Mathf.RoundToInt(dir.y);

			if (!EmptyCheck(x,y) && ColorMatch(x, y, piece))
			{
				Transform nextPiece = board[x, y];
				AddNeighbor(nextPiece, group);
			}
		}
	}

	public bool DeleteCheck() 
	{
		List<Transform> deletionGroup = new List<Transform>();

		for (int y = 0; y < 13; y++)
		{
			for(int x = 0; x < 6; x++)
			{
				List<Transform> group = new List<Transform>();

				if(board[x,y] != null)
				{
					Transform piece = board[x, y];
					if (deletionGroup.IndexOf(piece) == -1)
						AddNeighbor(piece, group);

					if (group.Count >= 4)
						foreach (Transform yopu in group)
							deletionGroup.Add(yopu);
				}
			}
		}

		if (deletionGroup.Count != 0)
		{
			DeleteGroup(deletionGroup);
			return true;
		}
		else return false;
	}

	public bool FallCheck() 
	{
		for(int y = 12; y >= 0; y--) // needs to scan from the top down, so start at 12 (13) and go down
		{
			for(int x = 0; x < 6; x++)
			{
				if (board[x, y] != null)
				{
					if (board[x, y].gameObject.GetComponent<YopuPiece>().forceDown == true)
						return true;
					else if (board[x, y].gameObject.GetComponent<YopuPiece>().falling == true)
						return true;
				}
			}
		}
		return false;
	}

	public void ForceDrop() 
	{
		for (int y = 0; y < 13; y++)
			for(int x = 0; x<6; x++)
				if(board[x,y] != null)
				{
					Transform piece = board[x, y];
					piece.gameObject.GetComponent<YopuPiece>().DropExt();
				}
	}
}