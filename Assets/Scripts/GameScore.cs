using UnityEngine;
using UnityEngine.UI;

public class GameScore : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Image healthImage;
    [SerializeField] float healthImageWidth;
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] GameObject startGameCanvas;
    [SerializeField] GameObject pauseCanvas;
    [SerializeField] Player player;

    [SerializeField] int phase = 1;
    [SerializeField] public int playerHealth = 3;
    [SerializeField] public int playerScore = 0;

    private bool gameHasStarted = false;
    private bool gameIsPaused = false;

    private void Start()
    {
        startGameCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
    }

    void Update()
    {
        if (scoreText != null)
        {
            scoreText.text = string.Format("{0}", playerScore);
        }

        if (healthImage != null)
        {
            var rt = healthImage.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(playerHealth * healthImageWidth, rt.rect.height);
        }

        if (Input.GetButton("Jump") && !gameHasStarted)
        {
            StartNewGame();
        }
        else if (Input.GetButton("Cancel") && gameHasStarted)
        {
            ToggleGamePause();
        }
    }

    public void HitDamage(int damage)
    {
        playerHealth -= damage;
    }

    public void IncreaseScore(int scorePoints)
    {
        playerScore += scorePoints;
    }

    public void ShowGameOver()
    {
        gameOverCanvas.SetActive(true);
        gameHasStarted = false;
    }

    public void ToggleGamePause()
    {
        var enemyMatrix = GetComponent<EnemyMatrix>();
        gameIsPaused = !gameIsPaused;
        if (gameIsPaused)
        {
            enemyMatrix.DisableEnemies();
            player.DisablePlayer();
        }
        else
        {
            enemyMatrix.EnableEnemies();
            player.EnablePlayer();
        }
        pauseCanvas.SetActive(!pauseCanvas.activeInHierarchy);

    }

    public void StartNewGame()
    {
        HideCanvasScreens();

        gameHasStarted = true;
        playerHealth = 3;
        playerScore = 0;
        player.EnablePlayer();

        var enemyMatrix = GetComponent<EnemyMatrix>();
        enemyMatrix.ShuffleNewEnemies(phase);
    }

    private void HideCanvasScreens()
    {
        gameOverCanvas.SetActive(false);
        startGameCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
    }
}
