using UnityEngine;
using System.Collections;
using System;

public class MazePassage : MazeCellEdge
{
	public bool DoorPassage { get; set; }

	public bool isEntry { get; set; }

	[SerializeField]
	private SpriteRenderer passSprite;

	public bool SameComponents
	{
        get
		{
			if (otherCell == null || !otherCell.Lit)
				return true;
			return cell.Component == otherCell.Component;
		}
	}


	public override void EnableRoomSettings()
	{
		passSprite.sprite = cell.room.Settings.GetRandomSprite(SpriteType.Pass);
	}
}
