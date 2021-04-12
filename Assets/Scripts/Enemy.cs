using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public string enemyName;
    [SerializeField] public int column;
    [SerializeField] public int row;
    [SerializeField] public float attackFrequency = 2f;
    [SerializeField] public float bulletGravity = 1f;
    [SerializeField] public float hitDemage = 1f;
    [SerializeField] public int scorePoints = 1;

    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bullet;

    public GameScore gameScore;
    public bool isEnabled = true;

    private float timer = 0;

    void Update()
    {
        if (isEnabled)
        {
            timer += Time.deltaTime;

            if (timer >= attackFrequency)
            {
                Attack();
                timer = timer % attackFrequency;
            }
        }
    }

    void Attack()
    {
        var bulletObj = Instantiate(bullet, firePoint.position, firePoint.rotation);
        bulletObj.GetComponent<Rigidbody2D>().gravityScale = bulletGravity;
    }

    public void TakeDemage(GameObject instance, Vector3 position)
    {
        if (gameScore != null)
        {
            gameScore.IncreaseScore(scorePoints);
        }

        Die(instance);
    }

    void Die(GameObject enemyObj)
    {
        isEnabled = false;
        gameScore.GetComponent<EnemyMatrix>().RemoveEnemy(enemyObj);
    }

    public void EnableEnemy()
    {
        isEnabled = true;
    }

    public void DisableEnemy()
    {
        isEnabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("1119: " + collision.name); // XXX
    }

}
