using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum BonusType
{
    heart,
    shield,
    laser
}

public class BonusItem : MonoBehaviour
{
    [SerializeField] BonusType bonusType;
    public GameScore gameScore;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GameOverOnCollision"))
        {
            Destroy(this);
        }
        else if (collision.CompareTag("Player"))
        {
            TriggerBonusEvent(collision.gameObject);
        }
    }

    private void TriggerBonusEvent(GameObject target)
    {
        var player = target.GetComponent<Player>();
        if (bonusType == BonusType.heart)
        {
            ApplyHeartBonus(player);
        }
        else if (bonusType == BonusType.shield)
        {
            ApplyShieldBonus(player);
        }
        else if (bonusType == BonusType.laser)
        {
            ApplyLaserBonus(player);
        }
        Destroy(gameObject);
    }

    private void ApplyHeartBonus(Player player)
    {
        if (gameScore.PlayerHealthIsNotMaximum())
        {
            gameScore.IncreasePlayerHealth(1);
        }
    }

    private void ApplyShieldBonus(Player player)
    {
        player.EnableShield();
    }

    private void ApplyLaserBonus(Player player)
    {
        player.MakeLaserAvailable();
    }
}
