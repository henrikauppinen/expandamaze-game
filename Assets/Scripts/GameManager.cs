using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Maze mazePrefab;
	public PlayerController playerPrefab;
	public GoalController goalPrefab;
	public Text ScoreText;
	public Text TimerText;

	public Canvas StartMenu;

	public Canvas GameUI;

	private Maze mazeInstance;
	private PlayerController playerInstance;
	private GoalController goalInstance;

	private int level;
	private int mazeStartSize;

	private double timer;

	// Use this for initialization
	void Start () {
		mazeStartSize = 5;
		level = 1;

		StartMenu.enabled = true;
		GameUI.enabled = false;

	}
	
	// Update is called once per frame
	void Update () {

		if (timer > 0.0) {
			timer -= Time.deltaTime;
			TimerText.text = timer.ToString ("0");
		}
		else {
			// game over!
		}
	}

	public void StartGame() {

		StartMenu.enabled = false;
		GameUI.enabled = true;

		mazeInstance = Instantiate (mazePrefab) as Maze;

		ScoreText.text = "Level " + level;
		timer = 30.0;

		IntVector2 newSize = new IntVector2(mazeStartSize + level, mazeStartSize + level);

		mazeInstance.SetSize (newSize);
		mazeInstance.Generate ();

		playerInstance = Instantiate (playerPrefab) as PlayerController;
		playerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));

		goalInstance = Instantiate (goalPrefab) as GoalController;
		goalInstance.SetLocation (mazeInstance.GetCell(mazeInstance.RandomCoordinates));

	}

	public void RestartGame() {
		Destroy (mazeInstance.gameObject);
		Destroy (playerInstance.gameObject);
		Destroy (goalInstance.gameObject);
		StartGame ();
	}

	public void NextLevel() {
		level += 1;
		RestartGame ();
	}
}

