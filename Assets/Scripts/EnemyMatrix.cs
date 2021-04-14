using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMatrix : MonoBehaviour
{
    [SerializeField] List<GameObject> enemies;
    [SerializeField] List<GameObject> bonusItems;
    [SerializeField] GameObject enemyContainer;
    [SerializeField] int columns;
    [SerializeField] int gap;
    [SerializeField] float timeToNextRow = 2f;
    [SerializeField] float accellerationForNextRow = .5f;
    [SerializeField] GameObject respawn;
    [SerializeField] [Range(0f,1f)] float bonusProbability = .1f;

    private List<GameObject> placedEnemies;
    private float counterTimeToNextRow = 0f;
    private bool isRunning = false;
    private bool enemyRotated = false;
    private bool enemyRotationTrigger = false;
    private float activeTimeToNextRow;

    private void Awake()
    {
        placedEnemies = new List<GameObject>();
    }

    private void Start()
    {
        activeTimeToNextRow = timeToNextRow;
    }

    private void FixedUpdate()
    {
        if (isRunning)
        {
            counterTimeToNextRow += Time.deltaTime;
            if (counterTimeToNextRow > activeTimeToNextRow)
            {
                counterTimeToNextRow = counterTimeToNextRow % activeTimeToNextRow;
                var gameScore = GetComponent<GameScore>();
                var nextPhase = gameScore.NextPhase();
                StartCoroutine(MoveOneRowDown());
                ShuffleNewEnemiesRow(nextPhase);

                if (enemyRotated && enemyRotationTrigger)
                {
                    enemyRotated = false;
                    enemyRotationTrigger = false;

                    Accelerate();
                }
            }
        }
    }

    private void Accelerate()
    {
        activeTimeToNextRow = activeTimeToNextRow * accellerationForNextRow;
        var gameScore = GetComponent<GameScore>();
        gameScore.IncreaseMaximumPlayerHealth();
        gameScore.ResetPlayerHealth();
    }

    private IEnumerator MoveOneRowDown()
    {
        var oldPosition = enemyContainer.transform.position;
        var targetPosition = new Vector3(
            oldPosition.x,
            oldPosition.y - gap,
            oldPosition.z
            );
        enemyContainer.transform.position = Vector3.MoveTowards(oldPosition, targetPosition, 1);
        yield return null;
    }

    public void ShuffleNewEnemiesRow(int phase)
    {
        int count = phase % columns;
        int enemyIndex = phase / columns % enemies.Count;
        if (enemyIndex == 0 && enemyRotationTrigger)
        {
            enemyRotated = true;
        }
        else if (enemyIndex == 1)
        {
            enemyRotationTrigger = true;
        }

        for (int counter = 0; counter < count; counter++)
        {
            float column = CalculateColumn(counter, count);
            if (Random.Range(0f, 1f) < bonusProbability)
            {
                AddBonusItem((int)Random.Range(0f, 2.99f), column);
            }
            else {
                var enemyObj = AddEnemy(enemyIndex, column);
                placedEnemies.Add(enemyObj);
            }
        }
    }

    float CalculateColumn(int position, int count)
    {
        float start = (float)count / 2 * -1;
        float col = start + position;
        return col;
    }

    GameObject AddEnemy(int enemyIndex, float column)
    {
        var rotation = GetComponent<Transform>().rotation;
        var position = CalculatePositionInMatrix(column);
        var enemyObj = Instantiate(enemies[enemyIndex], position, rotation);
        enemyObj.transform.parent = enemyContainer.transform;

        var enemyInfo = enemyObj.GetComponent<Enemy>();
        enemyInfo.gameScore = GetComponent<GameScore>();

        return enemyObj;
    }

    GameObject AddBonusItem(int itemIndex, float column)
    {
        var rotation = GetComponent<Transform>().rotation;
        var position = CalculatePositionInMatrix(column);
        var itemObj = Instantiate(bonusItems[itemIndex], position, rotation);
        itemObj.transform.parent = enemyContainer.transform;

        var itemInfo = itemObj.GetComponent<BonusItem>();
        itemInfo.gameScore = GetComponent<GameScore>();

        return itemObj;
    }

    Vector3 CalculatePositionInMatrix(float column)
    {
        float shift = gap / 2;
        return new Vector3(
            column * gap + shift,
            respawn.transform.position.y,
            enemyContainer.transform.position.z
        );
    }

    public void ResetEnemies()
    {
        while (placedEnemies.Count > 0)
        {
            RemoveEnemy(placedEnemies[0]);
        }
        enemyRotated = false;
        enemyRotationTrigger = false;
        activeTimeToNextRow = timeToNextRow;
        enemyContainer.transform.position = new Vector3(enemyContainer.transform.position.x, 0, enemyContainer.transform.position.z);
    }

    public void EnableEnemies()
    {
        foreach (var enemyObj in placedEnemies)
        {
            var enemyInfo = enemyObj.GetComponent<Enemy>();
            enemyInfo.EnableEnemy();
        }
        isRunning = true;
    }

    public void DisableEnemies()
    {
        foreach (var enemy in placedEnemies)
        {
            var enemyInfo = enemy.GetComponent<Enemy>();
            enemyInfo.DisableEnemy();
        }
        isRunning = false;
    }

    public void RemoveEnemy(GameObject removedEnemy)
    {
        placedEnemies.Remove(removedEnemy);
        Destroy(removedEnemy);
    }

}
