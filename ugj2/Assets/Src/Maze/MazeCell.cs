using UnityEngine;
using System.Collections;

public class MazeCell : MonoBehaviour {

	public IntVector2 coordinates;

	private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];

	public MazeCellEdge GetEdge(MazeDirection direction)
	{
		return edges[(int)direction];
	}

	private int initializedEdgeCount;

	public bool IsFullyInitialized
	{
		get
		{
			return initializedEdgeCount == MazeDirections.Count;
		}
	}

	private SpriteRenderer m_sr;
	private SpriteRenderer SR
	{
		get
		{
			if (m_sr == null)
			{
				m_sr = GetComponent<SpriteRenderer>();
			}
			return m_sr;
		}
	}

	private bool m_lit;

	public bool Lit
	{
        get
		{
			return m_lit;
		}
		set
		{
			m_lit = value;
			if (SR != null)
			{
				if (value)
					SR.color = Color.green;
				else
					SR.color = Color.white;
			}
		}
	}

	public void ValidateEdges()
	{
		for (int i = 0; i < MazeDirections.Count; i++)
		{
			if (edges[i].otherCell == null || !edges[i].otherCell.Lit)
			{
				Destroy(edges[i].gameObject);
				edges[i] = null;
                initializedEdgeCount--;
            }
		}
	}
		 

	public void SetEdge(MazeDirection direction, MazeCellEdge edge)
	{
		edges[(int)direction] = edge;
		initializedEdgeCount += 1;
	}

	public MazeDirection RandomUninitializedDirection
	{
		get
		{
			int skips = Random.Range(0, MazeDirections.Count - initializedEdgeCount);
			for (int i = 0; i < MazeDirections.Count; i++)
			{
				if (edges[i] == null)
				{
					if (skips == 0)
					{
						return (MazeDirection)i;
					}
					skips -= 1;
				}
			}
			throw new System.InvalidOperationException("MazeCell has no uninitialized directions left.");
		}
	}

}
