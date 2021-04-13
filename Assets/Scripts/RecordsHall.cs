using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordsHall : MonoBehaviour
{
    [SerializeField] GameObject recordHolder1;
    [SerializeField] GameObject recordHolder2;
    [SerializeField] GameObject recordHolder3;
    [SerializeField] GameObject recordHolder4;
    [SerializeField] GameObject recordHolder5;

    private List<RecordHolder> recordHolders;

    public bool IsScoreHighEnough(int score)
    {
        return score > recordHolders[recordHolders.Count - 1].score;
    }

    public void AddRecordHolderNameFromInput()
    {
        var gameScore = GetComponent<GameScore>();
        var playerName = gameScore.recordNameInput.text;
        var playerScore = gameScore.playerScore;
        var recordHolder = new RecordHolder(playerScore, playerName);
        AddRecordHolder(recordHolder);
    }

    private void Awake()
    {
        recordHolders = new List<RecordHolder>();
    }

    private void Start()
    {
        LoadRecordHolders();
    }

    private void UpdateRecordDisplays()
    {
        UpdateRecordHolderDisplay(1, recordHolder1, recordHolders[0]);
        UpdateRecordHolderDisplay(2, recordHolder2, recordHolders[1]);
        UpdateRecordHolderDisplay(3, recordHolder3, recordHolders[2]);
        UpdateRecordHolderDisplay(4, recordHolder4, recordHolders[3]);
        UpdateRecordHolderDisplay(5, recordHolder5, recordHolders[4]);
    }

    private void UpdateRecordHolderDisplay(int idNumber, GameObject display, RecordHolder recordHolder)
    {
        if (display == null)
        {
            return;
        }

        var displayInfo = display.GetComponent<RecordHolderDisplay>();
        displayInfo.UpdateRecordHolder(idNumber, recordHolder);
    }

    private void LoadRecordHolders()
    {
        recordHolders.Add(new RecordHolder(PlayerPrefs.GetInt(PlayerPrefKeys.RecordHolder01Score, 0),
            PlayerPrefs.GetString(PlayerPrefKeys.RecordHolder01Name, PlayerPrefKeys.NobodyName)));
        recordHolders.Add(new RecordHolder(PlayerPrefs.GetInt(PlayerPrefKeys.RecordHolder02Score, 0),
            PlayerPrefs.GetString(PlayerPrefKeys.RecordHolder02Name, PlayerPrefKeys.NobodyName)));
        recordHolders.Add(new RecordHolder(PlayerPrefs.GetInt(PlayerPrefKeys.RecordHolder03Score, 0),
            PlayerPrefs.GetString(PlayerPrefKeys.RecordHolder03Name, PlayerPrefKeys.NobodyName)));
        recordHolders.Add(new RecordHolder(PlayerPrefs.GetInt(PlayerPrefKeys.RecordHolder04Score, 0),
            PlayerPrefs.GetString(PlayerPrefKeys.RecordHolder04Name, PlayerPrefKeys.NobodyName)));
        recordHolders.Add(new RecordHolder(PlayerPrefs.GetInt(PlayerPrefKeys.RecordHolder05Score, 0),
            PlayerPrefs.GetString(PlayerPrefKeys.RecordHolder05Name, PlayerPrefKeys.NobodyName)));

        UpdateRecordDisplays();
    }

    private void SaveRecordHolders()
    {
        PlayerPrefs.SetInt(PlayerPrefKeys.RecordHolder01Score, recordHolders[0].score);
        PlayerPrefs.SetString(PlayerPrefKeys.RecordHolder01Name, recordHolders[0].name);
        PlayerPrefs.SetInt(PlayerPrefKeys.RecordHolder02Score, recordHolders[1].score);
        PlayerPrefs.SetString(PlayerPrefKeys.RecordHolder02Name, recordHolders[1].name);
        PlayerPrefs.SetInt(PlayerPrefKeys.RecordHolder03Score, recordHolders[2].score);
        PlayerPrefs.SetString(PlayerPrefKeys.RecordHolder03Name, recordHolders[2].name);
        PlayerPrefs.SetInt(PlayerPrefKeys.RecordHolder04Score, recordHolders[3].score);
        PlayerPrefs.SetString(PlayerPrefKeys.RecordHolder04Name, recordHolders[3].name);
        PlayerPrefs.SetInt(PlayerPrefKeys.RecordHolder05Score, recordHolders[4].score);
        PlayerPrefs.SetString(PlayerPrefKeys.RecordHolder05Name, recordHolders[4].name);
    }

    private void AddRecordHolder(RecordHolder recordHolder)
    {
        int position = 0;
        while (recordHolders[position].score >= recordHolder.score)
        {
            position++;
        }

        recordHolders.Insert(position, recordHolder);
        recordHolders.RemoveAt(recordHolders.Count - 1);

        SaveRecordHolders();
        UpdateRecordDisplays();
    }

}

public class RecordHolder
{
    public int score;
    public string name;

    public RecordHolder(int _score, string _name)
    {
        score = _score;
        name = _name;
    }
}

public static class PlayerPrefKeys
{
    public static string RecordHolder01Score = "RecordHolder01Score";
    public static string RecordHolder01Name = "RecordHolder01Name";
    public static string RecordHolder02Score = "RecordHolder02Score";
    public static string RecordHolder02Name = "RecordHolder02Name";
    public static string RecordHolder03Score = "RecordHolder03Score";
    public static string RecordHolder03Name = "RecordHolder03Name";
    public static string RecordHolder04Score = "RecordHolder04Score";
    public static string RecordHolder04Name = "RecordHolder04Name";
    public static string RecordHolder05Score = "RecordHolder05Score";
    public static string RecordHolder05Name = "RecordHolder05Name";

    public static string NobodyName = "NOBODY";
}
