﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogsCore : MonoBehaviour {

	[SerializeField]
	private DialogLine dlPrefab;

	[SerializeField]
	private List<string> StartDialog;

	[SerializeField]
	private List<string> femaleCall;

	[SerializeField]
	private List<string> femaleCallEvil;

	[SerializeField]
	private List<string> maleFrustration;

	[SerializeField]
	private List<string> demonDialog;

	[SerializeField]
	private List<string> endDialog;

	int femaleCallIndex = 0;

	[SerializeField]
	int femaleCallCD = 30;

	[SerializeField]
	float femaleCallTimer = 0;


	[SerializeField]
	int maleCallCD = 45;

	[SerializeField]
	float maleCallTimer = 0;
	// Use this for initialization
	void Start () {
		femaleCallTimer = femaleCallCD;
		maleCallTimer = maleCallCD;
    }
	
	// Update is called once per frame
	void Update () {
		if (GameCore.instance.GameOver)
			return;
		if (!GameCore.instance.PikedUpWater)
		{
			femaleCallTimer -= Time.deltaTime;
			if (femaleCallTimer < 0)
			{
				if (femaleCallIndex < femaleCall.Count)
				{
					CreateDialog(femaleCall[femaleCallIndex++]);
				}
				else if (femaleCallEvil.Count > 0)
				{
					CreateDialog(femaleCallEvil[Random.Range(0, femaleCallEvil.Count)]);
				}
				femaleCallTimer = femaleCallCD;
			}
		}
		else 
		{
			maleCallTimer -= Time.deltaTime;
			if (maleCallTimer < 0)
			{
				maleCallTimer = maleCallCD;
				CreateDialog(maleFrustration[Random.Range(0, maleFrustration.Count)]);
			}
		}

    }

	public void DropIndexes()
	{
		//femaleCallIndex = 0;
		femaleCallTimer = femaleCallCD;
		maleCallTimer = femaleCallCD;
	}
		 

	public void OnGameStart()
	{
		CreateDialog(new Queue<string>(StartDialog));
    }

	public void OnGameEnd()
	{
		CreateDialog(new Queue<string>(endDialog));
	}

	public void OnWaterPickUp()
	{
		CreateDialog(new Queue<string>(demonDialog));
	}

	public void CreateDialog(string code)
	{
		var dialog = Instantiate(dlPrefab);
		dialog.Init(code, null);
	}

	public void CreateDialog(Queue<string> dialogQueue)
	{
		var dialog = Instantiate(dlPrefab);
		dialog.Init(dialogQueue.Dequeue(), dialogQueue);
	}

	public AudioClip GetClip(string clipName)
	{
		var clip = Resources.Load<AudioClip>("Voice/" + clipName);
		if (clip == null)
			Debug.LogError("Failed to load Voice/" + clipName);
		return clip;
	}

	public string GetText(string clipName)
	{
		var txt = Resources.Load<TextAsset>("Text/" + clipName);
		if (txt == null)
			Debug.LogError("Failed to load Text/" + clipName);
		return txt.text;
	}

}
