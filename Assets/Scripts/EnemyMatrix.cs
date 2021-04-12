using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMatrix : MonoBehaviour
{
    [SerializeField] List<GameObject> enemies;
    [SerializeField] GameObject placeholderSprite;
    [SerializeField] int columns;
    [SerializeField] int gap;
    [SerializeField] float centerX = 0f;
    [SerializeField] float centerY = 0f;
    [SerializeField] float centerZ = 0f;
    [SerializeField] float timeToNextRow = 3f;

    private int latestRow = 0;
    private List<GameObject> placedEnemies;
    private float counterTimeToNextRow = 0f;

    private void Update()
    {
        counterTimeToNextRow += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (counterTimeToNextRow > timeToNextRow)
        {
            StartCoroutine(MoveOneRowDown());
            //ShuffleNewEnemies();
            counterTimeToNextRow = counterTimeToNextRow % timeToNextRow;
        }
    }

    IEnumerator MoveOneRowDown()
    {
        // TODO
        yield return null;
    }

    public void ShuffleNewEnemies(int count)
    {
        if (placedEnemies == null)
        {
            placedEnemies = new List<GameObject>();
        }

        int enemyIndex = 0; // XXX temp

        for (int counter = 0; counter < count; counter++)
        {
            int row = CalculateRow(latestRow);
            int column = CalculateColumn(counter);
            var enemyObj = AddEnemy(enemyIndex, column, row);

            placedEnemies.Add(enemyObj);
        }
    }

    int CalculateRow(int reference)
    {
        return (int)(centerY + reference);
    }

    int CalculateColumn(int reference)
    {
        return (int)(centerX + reference);
    }

    GameObject AddEnemy(int enemyIndex, int column, int row)
    {
        var rotation = GetComponent<Transform>().rotation;
        var position = new Vector3(column * gap, row * gap, centerZ);
        var enemyObj = Instantiate(enemies[enemyIndex], position, rotation);

        var enemyInfo = enemyObj.GetComponent<Enemy>();
        enemyInfo.column = column;
        enemyInfo.row = row;
        enemyInfo.gameScore = GetComponent<GameScore>();

        return enemyObj;
    }

    public void EnableEnemies()
    {
        foreach (var enemyObj in placedEnemies)
        {
            var enemyInfo = enemyObj.GetComponent<Enemy>();
            enemyInfo.EnableEnemy();
        }
    }

    public void DisableEnemies()
    {
        foreach (var enemy in placedEnemies)
        {
            var enemyInfo = enemy.GetComponent<Enemy>();
            enemyInfo.DisableEnemy();
        }
    }

    public void RemoveEnemy(GameObject removedEnemy)
    {
        placedEnemies.Remove(removedEnemy);
        Destroy(removedEnemy);
    }

}
