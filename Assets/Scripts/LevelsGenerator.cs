using System.IO;
using System.Collections.Generic;
using UnityEngine;
using MiniJSONV;
using TMPro;
using FullSerializer;

public class LevelsGenerator : MonoBehaviour
{
    private fsSerializer serializer = new fsSerializer();
    [SerializeField] string filename;
    int levelCount = 100;
    public TilePlacement placement;
    [SerializeField] Transform GridBackground;
    Dictionary<string, List<List<int>>> Leveldata;
    [SerializeField] public int startLevel;
    [SerializeField] bool DebugLevels;
    [SerializeField] TextMeshProUGUI DebugLevelText;



    private void Awake()
    {
        ReadfromFile();
        if (DebugLevels)
        {
            DebugLevelText.text = "Level " + startLevel;
            ShowNextLevel(startLevel);
        }
    }

    #region ReadWrite
    void ReadfromFile()
    {
        var value = Resources.Load("Levels").ToString();
        var data = fsJsonParser.Parse(value);
        object deserialized = null;
        serializer.TryDeserialize(data, typeof(Dictionary<string, List<List<int>>>), ref deserialized);
        Leveldata = deserialized as Dictionary<string, List<List<int>>>;
    }

    void WriteToFile()
    {
        string path = Application.dataPath + "/Resources";
        path += $"/{filename}.json";
        var jsondata = Json.Serialize(NewConfigDict());
        File.WriteAllText(path, jsondata);
        Debug.Log("data Put!");
    }

    #endregion

    public void DebugNextLevel()
    {
        startLevel++;
        DebugLevelText.text = "Level " + startLevel;
        ShowNextLevel(startLevel);
    }
    public void ShowNextLevel(int levelToShow)
    {
        if (GridBackground.childCount > 0)
            foreach (Transform t in GridBackground) { Destroy(t.gameObject); }

        placement.SetLevelWithConfig(
            Leveldata[levelToShow.ToString()], GetYStartNew(Leveldata[levelToShow.ToString()]), GridBackground
        );
        Utils.Invoke(0.5f, PickAndDrop.Instance.PopulateGridSlots);
    }

    #region Helpers

    int GetYStartNew(List<List<int>> list)
    {
        if (list.Count == 2 || list.Count == 3) return 0;
        if (list.Count == 4) return -1;
        if (list.Count == 5) return -2;
        return 0;
    }
    Dictionary<int, List<List<int>>> NewConfigDict()
    {
        var dict = new Dictionary<int, List<List<int>>>();
        for (int i = 0; i < levelCount; i++)
        {
            dict.Add((i + 1), getlevels(i + 1));
        }
        return dict;
    }
    List<List<int>> getlevels(int j)
    {
        List<List<int>> lvl = new List<List<int>>();

        for (int i = 0; i < tileRowsPerlevel(j); i++)
        {
            List<int> row = getRowTiles(j);
            lvl.Add(row);
        }
        return lvl;
    }
    int tileRowsPerlevel(int j)
    {
        if (j > 0 && j <= 10) return 2;
        if (j > 10 && j <= 30) return Random.Range(2, 4);
        if (j > 30 && j <= 50) return Random.Range(2, 4);
        if (j > 50 && j <= 70) return Random.Range(2, 5);
        if (j > 70 && j <= 90) return Random.Range(3, 5);
        if (j > 90 && j <= 100) return Random.Range(3, 5);
        return 2;
    }
    List<int> getRowTiles(int j)
    {
        List<int> row = new List<int>();
        for (int i = getRowStart(j); i < getEnd(j); i++)
        {
            row.Add(i);
        }
        PluckfromRow(row, j);
        return row;
    }
    int getRowStart(int j)
    {
        if (j > 0 && j <= 10) return Random.Range(-2, 0);
        if (j > 10 && j <= 30) return Random.Range(-3, 0);
        if (j > 30 && j <= 50) return Random.Range(-3, -1);
        if (j > 50 && j <= 70) return Random.Range(-3, -1);
        if (j > 70 && j <= 90) return Random.Range(-3, -1);
        if (j > 90 && j <= 100) return Random.Range(-3, -1);
        return -2;
    }
    int getEnd(int j)
    {
        if (j > 0 && j <= 10) return Random.Range(0, 4);
        if (j > 10 && j <= 30) return Random.Range(1, 4);
        if (j > 30 && j <= 50) return Random.Range(1, 5);
        if (j > 50 && j <= 70) return Random.Range(2, 5);
        if (j > 70 && j <= 90) return Random.Range(3, 5);
        if (j > 90 && j <= 100) return Random.Range(3, 5);
        return 2;
    }

    void PluckfromRow(List<int> row, int j)
    {
        /* if(j>0 && j<= 10)
        if(j>10 && j<= 30)
        if(j>30 && j<= 50)
        if(j>50 && j<= 70)
        if(j>70 && j<= 90)
        if(j>90 && j<= 100)Debug.Log(""); */
    }

    #endregion
}
