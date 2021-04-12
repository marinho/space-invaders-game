using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int damage = 1;

    private void OnTriggerEnter2D(Collider2D target)
    {
        var player = target.GetComponent<Player>();
        if (player != null)
        {
            var position = transform.position;
            player.TakeDemage(damage, position);
        }
        else
        {
            var enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                var position = transform.position;
                enemy.TakeDemage(target.gameObject, position);
            }
        }
    }
}
