﻿using UnityEngine;


public class CutsceneDriver : MonoBehaviour
{

	public bool MustShowEmotion { get { return _mustShowEmotion; } }
	public bool MustFollow { get { return _mustFollow; } }

	

    [SerializeField] private bool _mustShowEmotion;
    [SerializeField] private bool _mustFollow;

	[SerializeField] private BoomonController.State _boomonState;
	[SerializeField] private BoomonController.Emotion _boomonEmotion;

}
