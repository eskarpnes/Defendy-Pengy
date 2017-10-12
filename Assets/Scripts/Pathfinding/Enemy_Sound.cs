﻿using UnityEngine;
using System.Collections;

abstract partial class Enemy
{
	public AudioClip[] idleSounds;
	public AudioClip[] hurtSounds;

	protected float lastIdleSoundTime = -1f;

	protected abstract float IdleSoundFreq { get; }
	protected abstract float IdleSoundChance { get; }

	protected static readonly float[] RANDOM_PITCH_RANGE = { 0.9f, 1.1f };

	protected void HandleSound()
	{
		if (Time.time + IdleSoundFreq >= lastIdleSoundTime)
		{
			if (Random.value <= IdleSoundChance)
				PlayRandomSound(idleSounds);

			lastIdleSoundTime += IdleSoundFreq;
		}
	}

	protected void PlayRandomSound(AudioClip[] sounds)
	{
		AudioSource audio = GetComponent<AudioSource>();
		int soundIndex = (int)(Random.value * (sounds.Length - 1));
		audio.PlayOneShot(sounds[soundIndex]);
		audio.pitch = Random.value * (RANDOM_PITCH_RANGE[1] - RANDOM_PITCH_RANGE[0]) + RANDOM_PITCH_RANGE[0];
	}
}
