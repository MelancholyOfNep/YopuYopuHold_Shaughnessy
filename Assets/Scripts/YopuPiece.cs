using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YopuPiece : MonoBehaviour
{
    GameManager manager;

	private Color[] colorArr = { Color.cyan, Color.magenta, Color.yellow, Color.white };
	public bool falling = true;
	public bool forceDown = false;

	public int colorDesignation;

    private void Awake()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        colorDesignation = Random.Range(0, 4);
        GetComponent<SpriteRenderer>().color = colorArr[colorDesignation];
    }

    public IEnumerator Drop()
    {
        WaitForSeconds waitTrigger = new WaitForSeconds(manager.dropSpeed);
        Vector3 pos = new Vector2(Mathf.Round(gameObject.transform.position.x), Mathf.Round(gameObject.transform.position.y));
        for(int y = (int)pos.y - 1; y >= 0; y--) // -1 here
        {
            int x = (int)pos.x;
            if (manager.EmptyCheck(x, y))
            {
                forceDown = true;
                manager.Clear(x, y + 1);
                manager.Move(x, y, gameObject.transform);
                gameObject.transform.position += Vector3.down;
                yield return waitTrigger;
            }

            else
            {
                falling = false;
                forceDown = false;

                break;
            }
        }

        falling = false;
        forceDown = false;
    }

    public void DropExt()
    {
        StartCoroutine(Drop());
    }

}
