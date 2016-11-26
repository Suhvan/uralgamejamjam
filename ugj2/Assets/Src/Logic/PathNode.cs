using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class PathNode
{
	public IntVector2 realCoord;
	public int searchIndx;
	public IntVector2 fakeCoord;

	public PathNode(IntVector2 rCoord, int indx, IntVector2 fCoord)
	{
		realCoord = rCoord;
		searchIndx = indx;
		fakeCoord = fCoord;
	}
}

