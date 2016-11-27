using UnityEngine;
using System.Collections;

public class ExitPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void Restart()
	{
		GameCore.instance.StartCoroutine(GameCore.instance.RestartGame(false));
	}

	public void Exit()
	{
		Application.Quit();
	}
}
