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
        if (bonusType == BonusType.heart)
        {
            ApplyHeartBonus(target);
        }
        else if (bonusType == BonusType.shield)
        {
            ApplyShieldBonus(target);
        }
        else if (bonusType == BonusType.laser)
        {
            ApplyLaserBonus(target);
        }
    }

    private void ApplyHeartBonus(GameObject target)
    {
        if (gameScore.PlayerHealthIsNotMaximum())
        {
            gameScore.IncreasePlayerHealth(1);
        }
    }

    private void ApplyShieldBonus(GameObject target)
    {
        gameScore.EnableShield();
    }

    private void ApplyLaserBonus(GameObject target)
    {
        // TODO
        //Debug.Log("1113"); // XXX
    }
}
