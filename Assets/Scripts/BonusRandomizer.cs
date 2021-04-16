using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusRandomizer : MonoBehaviour
{
    [SerializeField] GameObject bonusContainer;
    [SerializeField] GameObject bonusLeftEdge;
    [SerializeField] GameObject bonusRightEdge;
    [SerializeField] List<GameObject> bonusItems;
    [SerializeField] [Range(0f, 10f)] float timeToNextBonus;

    private float counterTimeToNextBonus = 0f;
    private bool isRunning = false;

    private void FixedUpdate()
    {
        if (isRunning)
        {
            counterTimeToNextBonus += Time.deltaTime;
            if (counterTimeToNextBonus > timeToNextBonus)
            {
                counterTimeToNextBonus = counterTimeToNextBonus % timeToNextBonus;
                AddBonusItem((int)Random.Range(0f, 2.99f));
            }
        }
    }

    GameObject AddBonusItem(int itemIndex)
    {
        var rotation = GetComponent<Transform>().rotation;
        var position = CalculatePlacementPosition();
        var itemObj = Instantiate(bonusItems[itemIndex], position, rotation);
        itemObj.transform.parent = bonusContainer.transform;

        var itemInfo = itemObj.GetComponent<BonusItem>();
        itemInfo.gameScore = GetComponent<GameScore>();

        return itemObj;
    }

    Vector3 CalculatePlacementPosition()
    {
        float x = Random.Range(bonusLeftEdge.transform.position.x, bonusRightEdge.transform.position.x);
        return new Vector3(
            x,
            bonusContainer.transform.position.y,
            bonusContainer.transform.position.z
        );
    }

    public void EnableBonus()
    {
        isRunning = true;
    }

    public void DisableBonus()
    {
        isRunning = false;
    }

}
