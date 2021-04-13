using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordHolderDisplay : MonoBehaviour
{
    [SerializeField] Text idNumberText;
    [SerializeField] Text nameText;
    [SerializeField] Text scoreText;

    private int idNumber;
    private RecordHolder recordHolder;

    public void UpdateRecordHolder(int _idNumber, RecordHolder _recordHolder)
    {
        idNumber = _idNumber;
        recordHolder = _recordHolder;
    }

    // Update is called once per frame
    void Update()
    {
        idNumberText.text = string.Format("{0}.", idNumber);
        nameText.text = recordHolder.name;
        scoreText.text = string.Format("{0}", recordHolder.score);
    }
}
