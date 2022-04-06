using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YopuSet : MonoBehaviour
{
	public GameObject[] pieceArr = new GameObject[2];

	public float dropRate;
	public float doubleTime;
	public float regularTime;
	public float interval = 0;
	public bool swapped = false;

	public GameObject setPF;
	public GameObject setEffect;

	[SerializeField]
	GameManager manager;

    private void Start()
	{
		manager = GameObject.Find("GameManager").GetComponent<GameManager>();
		dropRate = manager.dropSpeed;
		regularTime = dropRate;
		doubleTime = dropRate / 2;

		pieceArr[0] = Instantiate(setPF, transform.position, Quaternion.identity);
		pieceArr[0].name = "MainPiece";
		pieceArr[1] = Instantiate(setPF, new Vector2(transform.position.x + 1, transform.position.y), Quaternion.identity);
		pieceArr[1].name = "SidePiece";
		pieceArr[0].transform.parent = gameObject.transform;
		pieceArr[1].transform.parent = gameObject.transform;
		BoardUpdate();
	}

	private void Update()
	{
		DropTimer();
	}

	bool MoveCheck(Vector3 dir)
	{
		foreach(Transform set in transform)
		{
			Vector3 newpos = new Vector3(set.position.x + dir.x, set.position.y + dir.y, 0);

			if (!manager.SpaceCheck(newpos, this.transform))
				return false;
		}
		return true;
	}

	private void Movement(Vector3 dir, Transform target)
	{
		BoardClear();
		target.position += dir;
		BoardUpdate();
	}

	void BoardUpdate()
	{
		foreach (Transform piece in transform)
			manager.Move(piece.position.x, piece.position.y, piece); // GameManager.Move takes transforms
	}

	void BoardClear()
	{
		foreach (Transform piece in transform)
			manager.Clear(piece.transform.position.x, piece.transform.position.y); // GameManager.Clear takes floats
	}

	bool SetFallCheck()
	{
		if (pieceArr[0].GetComponent<YopuPiece>().falling
			|| pieceArr[1].GetComponent<YopuPiece>().falling)
			return true;
		else return false;
	}		

	public void MoveInput(string dir)
	{
		if (dir == "Right")
		{
			if(MoveCheck(Vector3.right))
				Movement(Vector3.right, this.transform);
		}
		if (dir == "Left")
		{
			if (MoveCheck(Vector3.left))
				Movement(Vector3.left, this.transform);
		}
		if (dir == "Down")
		{
			if (MoveCheck(Vector3.down))
				Movement(Vector3.down, this.transform);
			else DisableSet();
		}
	}

	bool RotCheck(Vector3 dir)
	{
		Vector3 pos = pieceArr[1].transform.position;
		Vector3 newpos = new Vector3(pos.x + dir.x, pos.y + dir.y);
		return manager.SpaceCheck(newpos, transform);
	}

	Vector3 RotClockwiseVector()
	{
		Vector3 piecePos = new Vector2(Mathf.Round(pieceArr[1].transform.position.x), Mathf.Round(pieceArr[1].transform.position.y));

		if (Vector3.Distance(piecePos + Vector3.left, transform.position) == 0)
			return new Vector3(-1, -1);
		else if (Vector3.Distance(piecePos + Vector3.up, transform.position) == 0)
			return new Vector3(-1, 1);
		else if (Vector3.Distance(piecePos + Vector3.right, transform.position) == 0)
			return new Vector3(1, 1);
		else if (Vector3.Distance(piecePos + Vector3.down, transform.position) == 0)
			return new Vector3(1, -1);

		return new Vector3(0, 0);
	}

	Vector3 RotCounterClockwiseVector()
	{
		Vector3 piecePos = new Vector2(Mathf.Round(pieceArr[1].transform.position.x), Mathf.Round(pieceArr[1].transform.position.y));

		if (Vector3.Distance(piecePos + Vector3.left, transform.position) == 0)
			return new Vector3(-1, 1);
		else if (Vector3.Distance(piecePos + Vector3.up, transform.position) == 0)
			return new Vector3(1, 1);
		else if (Vector3.Distance(piecePos + Vector3.right, transform.position) == 0)
			return new Vector3(1, -1);
		else if (Vector3.Distance(piecePos + Vector3.down, transform.position) == 0)
			return new Vector3(-1, -1);

		return new Vector3(0, 0);
	}

	public void RotateInput(string input)
	{
		Vector3 rotationVector;
		if (input == "Left")
		{
			rotationVector = RotCounterClockwiseVector();
			if(RotCheck(rotationVector))
				Movement(rotationVector, pieceArr[1].transform);
		}
		else if (input == "Right")
		{
			rotationVector = RotClockwiseVector();
			if (RotCheck(rotationVector))
				Movement(rotationVector, pieceArr[1].transform);
		}
	}

	void DropPieces()
	{
		foreach (Transform piece in transform)
			StartCoroutine(piece.gameObject.GetComponent<YopuPiece>().Drop());
	}

	void DisableSet()
	{
		Instantiate(setEffect, this.transform.position, Quaternion.identity);
		manager.YopuRemainingDecrement();
		gameObject.GetComponent<Controller>().enabled = false;
		DropPieces();
		enabled = false;
		StartCoroutine(SpawnBlock());
	}

	public void DisableSetHoldSpawn()
	{
		float x, y, x1, y1;
		x = transform.position.x;
		y = transform.position.y;
		x1 = pieceArr[1].transform.position.x;
		y1 = pieceArr[1].transform.position.y;

		transform.position = new Vector2(9, 0);
		pieceArr[0].transform.position = new Vector2(9, 0);
		pieceArr[1].transform.position = new Vector2(10, 0);
		name = "HeldPiece";
		gameObject.GetComponent<Controller>().enabled = false;
		manager.Clear(x, y);
		manager.Clear(x1, y1);
		// DropPieces();
		enabled = false;
		GameObject.Find("Spawner").GetComponent<Spawner>().HoldSpawn();
	}

	public void DisableSetHoldSwap()
	{
		float x, y, x1, y1;
		x = pieceArr[0].transform.position.x;
		y = pieceArr[0].transform.position.y;
		x1 = pieceArr[1].transform.position.x;
		y1 = pieceArr[1].transform.position.y;

		GameObject heldPiece = GameObject.Find("HeldPiece");
		heldPiece.GetComponent<YopuSet>().transform.position = GameObject.Find("Spawner").transform.position;
		heldPiece.GetComponent<YopuSet>().pieceArr[0].transform.position = GameObject.Find("Spawner").transform.position;
		heldPiece.GetComponent<YopuSet>().pieceArr[1].transform.position = GameObject.Find("Spawner").transform.position + Vector3.right;
		heldPiece.GetComponent<YopuSet>().swapped = true;
		heldPiece.GetComponent<YopuSet>().enabled = true;
		heldPiece.name = "YopuSet";

		name = "HeldPiece";
		pieceArr[0].transform.position = new Vector2(9, 0);
		pieceArr[1].transform.position = new Vector2(10, 0);
		gameObject.GetComponent<Controller>().enabled = false;
		heldPiece.GetComponent<Controller>().enabled = true;
		manager.Clear(x, y);
		manager.Clear(x1, y1);
		// DropPieces();
		enabled = false;
	}

	IEnumerator SpawnBlock()
	{
		yield return new WaitUntil(() => !SetFallCheck());
		GameObject.Find("Spawner").GetComponent<Spawner>().SpawnCycle();
	}

	void DropTimer()
	{
		if (interval > dropRate)
		{
			MoveInput("Down");
			interval = 0;
		}
		else
			interval += Time.deltaTime;
	}
}
