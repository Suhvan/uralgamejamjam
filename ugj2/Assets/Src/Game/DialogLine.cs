using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogLine : MonoBehaviour {
	[SerializeField]
	Text text;

	[SerializeField]
	AudioClip audioClip;

	private string dialogCode;

	private Queue<string> DialogQueue; 

	public void Init(string code, Queue<string> dialogQueue)
	{
		dialogCode = code;
		audioClip = GameCore.instance.dialogSystem.GetClip(dialogCode);
		text.text = GameCore.instance.dialogSystem.GetText(dialogCode);
		DialogQueue = dialogQueue;
    }

	// Use this for initialization
	void Start () {
		AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
		StartCoroutine(DestryIn(audioClip.length));
    }

	

	private IEnumerator DestryIn(float seconds)
	{
		yield return new WaitForSeconds(seconds + 0.5f);
		Destroy(gameObject);
		if (DialogQueue.Count > 0)
			GameCore.instance.dialogSystem.CreateDialog(DialogQueue);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
