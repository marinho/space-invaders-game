using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public int damage = 1;
    [SerializeField] public string targetTag;

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (!target.CompareTag(targetTag))
        {
            return;
        }

        var position = transform.position;

        if (targetTag == "Player")
        {
            var player = (target.gameObject.GetComponent<Player>());
            player.TakeDamage(damage, position);
        }
        else if (targetTag == "Enemy")
        {
            var enemy = (target.gameObject.GetComponent<Enemy>());
            enemy.TakeDamage(damage, position);
        }

        Destroy(gameObject);
    }
}
