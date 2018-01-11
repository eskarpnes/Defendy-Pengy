﻿//Timmy Chan
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;
using Valve.VR;

public class GameManager_level01 : MonoBehaviour, IGameManager {
	public SpawnManager spawnManager;
	public GameHealthManager gameHealthManager;
	public Transform enemyManager;
	public GameObject longbow;
	public GameUI_ImportantMessage importantMessage;

	public Hand leftHand;
	public Hand rightHand;

	private float timeToStart = 3f;
	public bool started = false;
	public bool levelEnded = false;

	private float resetTriggerStartTime = -1;
	private const float RESET_BUTTON_DURATION = 1f; // seconds

	void Start()
	{
		gameHealthManager.gameManager = (IGameManager)this;
	}

	private bool areResetButtonsPressed()
	{
		//return leftHand.controller.GetPress(EVRButtonId.k_EButton_ApplicationMenu)
		//	&& rightHand.controller.GetPress(EVRButtonId.k_EButton_ApplicationMenu); // menu buttons on both left and right controllers
        return false;
	}

	void Update()
	{
		// TEMPORARY DEVELOPER HOTKEYS:
		if (areResetButtonsPressed())
		{
			if (resetTriggerStartTime < 0f)
				resetTriggerStartTime = Time.time;
			else if (Time.time > resetTriggerStartTime + RESET_BUTTON_DURATION)
				SceneManager.LoadScene("level1");
		} else if (resetTriggerStartTime > 0f)
			resetTriggerStartTime = -1;
		if (Input.GetKeyDown(KeyCode.R))
			SceneManager.LoadScene("level1");
		if (Input.GetKeyDown(KeyCode.W))
			GameStart();
		// END


		if (!levelEnded) {
			if (!started) {
				if (Input.GetKeyUp(KeyCode.A))
					GameStart();
				if (Input.GetKeyUp(KeyCode.S))
					GameLost();
				if (timeToStart >= 0) {
					//timeToStart -= Time.deltaTime;
				} else {
					//GameStart ();
				}
			}
			if (started) {
				//if there are no more lives
				/*if (gameHealthManager.RemainingGameHealth () <= 0) {
					GameLost ();
					levelEnded = true;
				}*/
				//if there are no more enemies to be spawned
				if (spawnManager.RemainingWavesCount () == 0) {
					//if there are no more enemies alive
					if (enemyManager.childCount == 0) {
						GameWin ();
						levelEnded = true;
					}
				}
			}
		}
		else {
			//what to do?
		}
	}

	public void GameLost() {
		levelEnded = true;
		print("You have lost the game");
		spawnManager.StopSpawning();
		importantMessage.Show("Game Over");
		//Invoke ("LoadMenu", 5f);
		//change scene??
		Invoke("GameRestart", 5f);
	}

	public void GameStart() {
		if (started)
			return;

		started = true;
		print ("Game Start!");
		spawnManager.StartSpawningWaves();
	}

	public void GamePause() {
		throw new System.NotImplementedException();
	}

	public void GameRestart()
	{
		SceneManager.LoadScene("level1");
	}

	public void GameWin() {
		levelEnded = true;
		print("You have won the game");
		importantMessage.Show("Game Win");
		Invoke("GameRestart", 5f);
	}

	void LoadMenu()
	{
		SceneManager.LoadScene("menu");
	}
}
