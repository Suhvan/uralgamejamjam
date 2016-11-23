using UnityEngine;

public enum SpriteType
{
	Cell,
	Wall,
	Pass,
	WallCorner,
	PassCorner
}

[System.Serializable]
public class MazeRoomSettings
{
	Sprite[] cellSprites;
	Sprite[] wallSprites;
	Sprite[] passSprites;


	[SerializeField]
	string spriteFolder;

	public void Init()
	{
		cellSprites = Resources.LoadAll<Sprite>(spriteFolder+"/cell");
		wallSprites = Resources.LoadAll<Sprite>(spriteFolder + "/wall");
		passSprites = Resources.LoadAll<Sprite>(spriteFolder + "/pass");
	}

	public Sprite GetRandomSprite(SpriteType type)
	{
		switch(type)
		{
			case SpriteType.Cell:
				return cellSprites[Random.Range(0, cellSprites.Length)];
			case SpriteType.Wall:
				return wallSprites[Random.Range(0, wallSprites.Length)];
			case SpriteType.Pass:
				return passSprites[Random.Range(0, passSprites.Length)];
			default:
				return null;
		}
		
	}
}
