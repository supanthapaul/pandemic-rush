using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.Soundy;

public class MainMenuManager : MonoBehaviour
{
	private void Start() {
		SoundyManager.StopAllSounds();
		SoundyManager.Play("Music", "MenuMusic");
	}
  public void QuitGame() {
		Application.Quit();
	}
}
