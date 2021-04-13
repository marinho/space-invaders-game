using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameScore : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Image healthImage;
    [SerializeField] float healthImageWidth;
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] public GameObject gameOverWithRecordCanvas;
    [SerializeField] GameObject startGameCanvas;
    [SerializeField] GameObject pauseCanvas;
    [SerializeField] Player player;
    [SerializeField] public InputField recordNameInput;

    [SerializeField] public int phase = 1;
    [SerializeField] public int initialPlayerHealth = 3;
    [SerializeField] public int playerScore = 0;

    private bool gameHasStarted = false;
    private bool gameIsPaused = false;
    private bool isShowingDialogWithInput = false;
    private int playerHealth;
    private int maximumPlayerHealth;

    private void Start()
    {
        startGameCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
        gameOverWithRecordCanvas.SetActive(false);
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

        if (Input.GetButton("Start") && !gameHasStarted)
        {
            StartNewGame();
        }
        else if (Input.GetButton("Cancel") && gameHasStarted)
        {
            ToggleGamePause(!gameIsPaused);
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

    public void GameOver()
    {
        ToggleGameFreeze(true);

        var recordHall = GetComponent<RecordsHall>();
        if (recordHall.IsScoreHighEnough(playerScore))
        {
            isShowingDialogWithInput = true;
            recordNameInput.text = "";
            gameOverWithRecordCanvas.SetActive(true);
            FocusInputField(recordNameInput);
        }
        else
        {
            gameOverCanvas.SetActive(true);
        }

        gameHasStarted = false;
    }

    private void FocusInputField(InputField input)
    {
        EventSystem.current.SetSelectedGameObject(input.gameObject, null);
        //input.OnPointerClick(null);
    }

    public void CloseDialogWithRecordHolderInput()
    {
        isShowingDialogWithInput = false;
        gameOverWithRecordCanvas.SetActive(false);
        ToggleGamePause(true);
    }

    public void ToggleGamePause(bool newGamePaused)
    {
        gameIsPaused = newGamePaused;
        ToggleGameFreeze(newGamePaused);
        pauseCanvas.SetActive(newGamePaused);
    }

    public void ToggleGameFreeze(bool becomeFreezed)
    {
        var enemyMatrix = GetComponent<EnemyMatrix>();
        if (becomeFreezed)
        {
            enemyMatrix.DisableEnemies();
            player.DisablePlayer();
        }
        else
        {
            enemyMatrix.EnableEnemies();
            player.EnablePlayer();
        }
    }

    public void StartNewGame()
    {
        HideCanvasScreens();

        gameHasStarted = true;
        maximumPlayerHealth = initialPlayerHealth;
        playerHealth = initialPlayerHealth;
        playerScore = 0;
        phase = 1;
        player.EnablePlayer();

        var enemyMatrix = GetComponent<EnemyMatrix>();
        enemyMatrix.ResetEnemies();
        enemyMatrix.EnableEnemies();
    }

    public int NextPhase()
    {
        return phase++;
    }

    private void HideCanvasScreens()
    {
        gameOverCanvas.SetActive(false);
        gameOverWithRecordCanvas.SetActive(false);
        startGameCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
    }

    public void IncreaseMaximumPlayerHealth()
    {
        maximumPlayerHealth++;
    }

    public void ResetPlayerHealth()
    {
        playerHealth = maximumPlayerHealth;
    }

    public bool PlayerHealthIsZero()
    {
        return playerHealth <= 0;
    }
}
