using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
	#region Fields

    [Header("Connections")]
    public GameObject heroPrefab;
    public Transform spawnPoint;

    [Header("Buttons")]
    public Button startGame;
    public Button resumeGame;
    public Button mainMenu;

    [Header("Meters")]
    public Text scoreMeter;

    GameObject hero;

    // statics
    public static int state = 0; // 0 = main menu, 1 = in game, 2 = game over
    public static int npcCount;
    static int npcEliminations;
    static GameManager gm;

	#endregion

	#region Engine Events

    private void Awake()
    {
        // init
        npcEliminations = 0;
        npcCount = 0;

        if (resumeGame != null) { resumeGame.gameObject.SetActive(false); }
        if (mainMenu != null) { mainMenu.gameObject.SetActive(false); }
		if (scoreMeter != null) { scoreMeter.text = ""; }

        gm = this;
        Time.timeScale = 1;

        if (startGame == null)
        {
            StartGame();
        }
    }

    private void Update()
    {
        if (state == 1)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (resumeGame != null)
                {
                    resumeGame.gameObject.SetActive(true);
                    Time.timeScale = 0;
                }

                if (mainMenu != null)
                {
                    mainMenu.gameObject.SetActive(true);
                }
            }
        }
    }

	#endregion

	#region Methods

	public static void EliminateNPC()
	{
		npcEliminations++;

		if (gm.scoreMeter != null)
		{
			gm.scoreMeter.text = "Score: " + npcEliminations.ToString();
		}


		if (npcEliminations == npcCount)
		{
			gm.Invoke("GameOver", 2);
			state = 2;
		}
	}

	void GameOver()
	{
		if (mainMenu != null)
		{
			hero.GetComponent<HeroController>().DisableMovement();
			mainMenu.gameObject.SetActive(true);
		}
	}

	#endregion

	#region UI Callbacks

    public void StartGame()
    {
        state = 1;
        if (startGame != null)
        {
            startGame.gameObject.SetActive(false);
        }

        hero = Instantiate(heroPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public void ResumeGame()
    {
        if (resumeGame != null)
        {
            resumeGame.gameObject.SetActive(false);
        }

        if (mainMenu != null)
        {
            mainMenu.gameObject.SetActive(false);
        }

        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

	#endregion
}
