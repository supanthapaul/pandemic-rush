using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.UI;

public class PauseManager : MonoBehaviour
{
  public static PauseManager instance;

	public UIView pauseMenuView;
	public bool isGamePaused = false;

	private void Awake() {
		if(instance == null) {
			instance = this;
		}
	}

	private void Update() {
		if(Input.GetKeyDown(KeyCode.Escape) && !TimedRush.instance.isGameOver) {
			if(isGamePaused) {
				// Resume game
				ResumeGame();
			}
			else {
				// pause game
				PauseGame();
			}
		}
	}

	void PauseGame() {
		isGamePaused = true;
		PauseTime();
		// unlock cursor
		Inventory.instance.LockUnlockCursor(true);
		pauseMenuView.Show();
	}

	public void ResumeGame() {
		isGamePaused = false;
		ResumeTime();
		// unlock cursor
		Inventory.instance.LockUnlockCursor(false);
		pauseMenuView.Hide();
	}

	public void PauseTime() {
		Time.timeScale = 0;
	}
	public void ResumeTime() {
		Time.timeScale = 1;
	}

}
