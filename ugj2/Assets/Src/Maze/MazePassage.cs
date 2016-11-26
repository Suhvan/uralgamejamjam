using UnityEngine;
using System.Collections;
using System;

public class MazePassage : MazeCellEdge
{
	public bool DoorPassage { get; set; }

	public bool isEntry { get; set; }

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
	
	}
}
