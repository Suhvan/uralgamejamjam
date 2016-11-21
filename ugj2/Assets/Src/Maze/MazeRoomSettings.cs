using UnityEngine;

[System.Serializable]
public class MazeRoomSettings
{
	Sprite[] cellSprites;

	[SerializeField]
	string spriteFolder;

	public void Init()
	{
		cellSprites = Resources.LoadAll<Sprite>(spriteFolder);
	}

	public Sprite GetRandomSprite()
	{
		return cellSprites[Random.Range(0, cellSprites.Length)];
	}
}
