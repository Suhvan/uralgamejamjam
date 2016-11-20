
using UnityEngine;

class MazeLighter : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D other)
	{
		var cell = other.gameObject.GetComponent<MazeCell>();
		if (cell != null)
		{
			cell.Lit = true;
		}
	}


	void OnTriggerExit2D(Collider2D other)
	{
		var cell = other.gameObject.GetComponent<MazeCell>();
		if (cell != null)
		{
			cell.Lit = false;
		}
	}

}

