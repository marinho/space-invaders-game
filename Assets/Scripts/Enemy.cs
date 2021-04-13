using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IBulletTarget
{
    [SerializeField] public string enemyName;
    [SerializeField] public float attackFrequency = 2f;
    [SerializeField] public float bulletGravity = 1f;
    [SerializeField] public int hitDemage = 1;
    [SerializeField] public int scorePoints = 1;

    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bullet;

    public GameScore gameScore;
    public bool isRunning = true;

    private float timer = 0;

    void Update()
    {
        if (isRunning)
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
        bulletObj.GetComponent<Bullet>().damage = hitDemage;
    }

    public void TakeDamage(int damage, Vector3 position)
    {
        if (gameScore != null)
        {
            gameScore.IncreaseScore(scorePoints);
        }

        Die();
    }

    void Die()
    {
        isRunning = false;
        gameScore.GetComponent<EnemyMatrix>().RemoveEnemy(gameObject);
    }

    public void EnableEnemy()
    {
        isRunning = true;
    }

    public void DisableEnemy()
    {
        isRunning = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GameOverOnCollision") || collision.CompareTag("Player"))
        {
            gameScore.GameOver();
        }
    }

}
