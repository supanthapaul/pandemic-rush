using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Doozy.Engine.UI;

public class GameStarter : MonoBehaviour
{
  public float timeToStart;
	public UIView gameStartPanel;
	public TextMeshProUGUI startTimerText;
	public PlayerMovement playerMovememnt;

	private bool timerIsRunning = false;
	private EnemyAI[] enemies;
	bool flag = false;

	private void Awake() {
		enemies = FindObjectsOfType<EnemyAI>();
		// disable enemies
		for (int i = 0; i < enemies.Length; i++)
		{
			enemies[i].enabled = false;
		}
		// disable player movement
		playerMovememnt.enabled = false;
	}

	private void Start() {
		timerIsRunning = true;
	}

	private void Update() {
		TickTimer();
		// display time
		startTimerText.text = string.Format("{0:0}", timeToStart);
	}

	void TickTimer()
	{
		if (timerIsRunning)
		{
			if (timeToStart > 0)
			{
				timeToStart -= Time.deltaTime;
			}
			else
			{
				if(flag) {
					return;
				}
				// disable game start panel
				gameStartPanel.Hide();
				// start game timer
				TimedRush.instance.StartTimer();
				// enable player movement
				playerMovememnt.enabled = true;
				// enable enemies
				for (int i = 0; i < enemies.Length; i++)
				{
					enemies[i].enabled = true;
				}
				flag = true;

				timeToStart = 0;
				timerIsRunning = false;
			}
		}
	}
}
