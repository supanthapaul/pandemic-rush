using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Doozy.Engine.UI;
using Doozy.Engine.Soundy;

public enum GameStates
{
	FindItems,
	GetShoppingCart,
	ReachCheckout
}

public class TimedRush : MonoBehaviour
{
	public PlayerMovement playerMovement;
	public int score = 0;
	public int scorePerItem = 100;
	public float timeToComplete;
	public TextMeshProUGUI timeText;
	public TextMeshProUGUI objectiveText;
	public TextMeshProUGUI scoreText;
	public TextMeshProUGUI inventorySpaceText;
	[Header("Game Over references")]
	public UIView gameOverView;
	public TextMeshProUGUI gameOverScoreText;
	public TextMeshProUGUI gameOverHighscoreText;
	public bool isGameOver = false;

	private GameStates gameState;
	private bool timerIsRunning = false;

	public static TimedRush instance;
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}
	private void Start()
	{
		//StartTimer();
		SoundyManager.StopAllSounds();
		SoundyManager.Play("Music", "GameMusic");

		Inventory.onInventoryChange += OnInventoryItemsChange;
	}

	private void Update()
	{
		TickTimer();
		objectiveText.text = GetObjectiveFromState(gameState);
		DisplayTime(timeToComplete);
		DisplayScore();

		switch (gameState)
		{
			case GameStates.GetShoppingCart:
				{
					if (playerMovement.disableMovement)
					{
						// got shopping cart
						gameState = GameStates.ReachCheckout;
					}
					break;
				}
			case GameStates.ReachCheckout:
				{
					break;
				}
		}
	}
	void OnInventoryItemsChange()
	{
		// start cart state if inventory is full
		if(Inventory.instance.IsFull()) {
			gameState = GameStates.GetShoppingCart;
		}
		// set inventory space text
		inventorySpaceText.text = Inventory.instance.GetInventorySize() + "/" + Inventory.instance.maxInventorySize+ " Inventory Space";
	}

	public void SetGameState(GameStates state) {
		gameState = state;
	}

	public void StartTimer()
	{
		timerIsRunning = true;
	}
	void TickTimer()
	{
		if (timerIsRunning)
		{
			if (timeToComplete > 0)
			{
				timeToComplete -= Time.deltaTime;
			}
			else
			{
				Debug.Log("Time has run out!");
				GameOver();
				timeToComplete = 0;
				timerIsRunning = false;
			}
		}
	}

	void GameOver()
	{
		isGameOver = true;
		// pause time
		PauseManager.instance.PauseTime();
		// unlock cursor
		Inventory.instance.LockUnlockCursor(true);
		// enable game over view
		gameOverView.Show();
		// set score texts
		int highscore = PlayerPrefs.GetInt("Highscore", 0);
		if(score > highscore) {
			PlayerPrefs.SetInt("Highscore", score);
		}
		gameOverScoreText.text = string.Format("{0:0000}", score);
		gameOverHighscoreText.text = string.Format("{0:0000}", PlayerPrefs.GetInt("Highscore", 0));
	}

	public void IncrementScore(int value)
	{
		score += value;
	}

	void DisplayScore()
	{
		scoreText.text = string.Format("{0:000}", score);
	}
	void DisplayTime(float timeToDisplay)
	{
		float minutes = Mathf.FloorToInt(timeToDisplay / 60);
		float seconds = Mathf.FloorToInt(timeToDisplay % 60);

		timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
	}

	string GetObjectiveFromState(GameStates state)
	{
		switch (state)
		{
			case GameStates.FindItems:
				{
					return "Grab Supplies!";
				}
			case GameStates.GetShoppingCart:
				{
					return "Get a Shopping Cart!";
				}
			case GameStates.ReachCheckout:
				{
					return "Return to Checkout!";
				}
		}
		return "Grab Supplies";
	}
}


