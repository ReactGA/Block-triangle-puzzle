using System.IO;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MiniJSONV;
using FullSerializer;

public class SolutionGenerator : MonoBehaviour
{
    private fsSerializer serializer = new fsSerializer();
    public TilePlacement tilePlacement;
    string Filename = "SolutionData";
    List<GameObject> EditTileHolders = new List<GameObject>();
    [SerializeField] Transform EditTileHoldersParent;
    List<TilesPerLevel> TotalData1 = new List<TilesPerLevel>();
    int CurrentTileIndex = -1;
    public GameObject TilePrefab;
    public GameObject TileSet;
    public LevelsGenerator lvlGen;
    public TilePlacement placement;
    [SerializeField] Transform GameTileHolder;
    bool first;
    Vector2 firstCoord;
    float InitRot;
    Vector2 HintCenter;
    bool isAdding;
    int ColorIndex = 1;
    [SerializeField] LevelsGenerator levelGen;
    TilesPerLevel tilesPerLevel;
    [SerializeField] bool DebugLevels;

    private void Start()
    {
        if (!DebugLevels)
            LoadData();
        else
            newTilePerLevel();
    }
    #region ReadWrite
    public static string LoadDataNew()
    {
        var value = Resources.Load("SolutionData").ToString();
        return value;
    }
    private void SaveData()
    {
        string path = Application.dataPath + "/Resources";
        path += $"/{Filename}.json";
        fsData fs;
        serializer.TrySerialize(typeof(Dictionary<string, object>), TotalData1, out fs);
        var jsondata = fsJsonPrinter.CompressedJson(fs);
        File.WriteAllText(path, jsondata);
        Debug.Log("data Put!");
    }
    void LoadData()
    {
        var value = Resources.Load("SolutionData").ToString();
        var data = fsJsonParser.Parse(value);
        object deserialized = null;
        serializer.TryDeserialize(data, typeof(List<TilesPerLevel>), ref deserialized);
        TotalData1 = deserialized as List<TilesPerLevel>;
        // Debug.Log(TotalData1.Count);
    }
    #endregion

    Transform TilesBackground;

    #region Using
    //Call on every new level
    public void newTilePerLevel()
    {
        if (tilesPerLevel != null) Save();
        tilesPerLevel = new TilesPerLevel();
        // levelGen.DebugNextLevel();
    }
    void SavePreviousTile()
    {
        if (tilesPerLevel.Tiles.Count == 0) return;
        tilesPerLevel.Tiles[CurrentTileIndex].color = ColorIndex;
        tilesPerLevel.Tiles[CurrentTileIndex].InitRot = (int)InitRot;
        tilesPerLevel.Tiles[CurrentTileIndex].HintXPos = HintCenter.x;
        tilesPerLevel.Tiles[CurrentTileIndex].HintYPos = HintCenter.y;
    }
    //Call on every new Tile
    public void AddTileInstance2()
    {
        SavePreviousTile();
        first = false;
        isAdding = true;
        TileSet.SetActive(true);
        TileSet.GetComponent<TextMeshProUGUI>().text = "Tile set: " + (CurrentTileIndex + 1);
        CurrentTileIndex += 1;
        EditTileHolders.Add(new GameObject("Tile_" + CurrentTileIndex));
        EditTileHolders[CurrentTileIndex].transform.parent = EditTileHoldersParent;
        tilesPerLevel.Tiles.Add(new TilesInLevel());

    }
    public void AddTilePiece2(Transform t)
    {
        var cord = ConvertToCord(t.position);
        if (!first)
        {
            firstCoord = cord;
            InitRot = t.localEulerAngles.z;
            if (CurrentTileIndex == 0) HintCenter = firstCoord;
            first = true;
        }
        cord -= firstCoord;

        if (cord.y > tilesPerLevel.Tiles[CurrentTileIndex].coord.Count - 1)
        {
            for (int i = 0; i < cord.y - (tilesPerLevel.Tiles[CurrentTileIndex].coord.Count - 1); i++)
            {
                tilesPerLevel.Tiles[CurrentTileIndex].coord.Add(new List<int>());
            }
        }
        tilesPerLevel.Tiles[CurrentTileIndex].coord[(int)cord.y].Add((int)cord.x);
        ShowVisual(t.position, t.localEulerAngles.z);
    }
    #endregion
    public void ShowNextLevel(int levelToShow)
    {
        TilesBackground = GameManager.Instance.TileBackground;
        levelToShow--;
        if (TilesBackground.childCount > 0)
            foreach (Transform t in TilesBackground) { Destroy(t.gameObject); }
        for (int i = 0; i < TotalData1[levelToShow].Tiles.Count; i++)
        {
            var g = placement.SetTileConfig(
                TotalData1[levelToShow].Tiles[i].coord,
                TotalData1[levelToShow].Tiles[i].color,
                (int)TotalData1[levelToShow].Tiles[i].InitRot
            );
            g.transform.parent = TilesBackground;
            g.GetComponent<TileInstance>().ReturnTile();
        }
    }
    public Vector2 GetHintCenter(int level)
    {
        var pos = new Vector2(TotalData1[level-1].Tiles[0].HintXPos,
        TotalData1[level-1].Tiles[0].HintYPos);

        pos = new Vector2((pos.x * tilePlacement.xOffset) + tilePlacement.originX,
            (pos.y * tilePlacement.yOffset) +tilePlacement.originY);
        
        return pos;
    }
    public GameObject GetHintTile(int level)
    {
        return placement.SetTileConfig(
                TotalData1[level-1].Tiles[0].coord,
                TotalData1[level-1].Tiles[0].color,
                (int)TotalData1[level-1].Tiles[0].InitRot
            );
    }
    public void Save()
    {
        SavePreviousTile();
        tilesPerLevel.levelNo = lvlGen.startLevel;
        TotalData1.Add(tilesPerLevel);
        tilesPerLevel = null;
        CurrentTileIndex = -1;
        TileSet.SetActive(false);
        isAdding = false;
        first = false;
        EditTileHolders.Clear();
        foreach (Transform t in EditTileHoldersParent) Destroy(t.gameObject);
        Debug.Log("saved!");
        SaveData();
    }
    public void SaveAll()
    {
        SaveData();
    }

    #region Helpers
    public void AddColor(int col)
    {
        ColorIndex = col;
        foreach (Transform t in EditTileHolders[CurrentTileIndex].transform)
        {
            SetColor(t);
        }
    }

    void SetColor(Transform g)
    {
        for (int i = 0; i < g.childCount; i++)
        {
            g.GetChild(i).gameObject.SetActive(ColorIndex == (g.GetChild(i).GetSiblingIndex() + 1));
        }
    }
    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && isAdding)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider && !(hit.transform.GetComponentInParent<TilePiece>()))
            {
                hit.collider.enabled = false;
                AddTilePiece2(hit.transform);
            }
        }
    }
    Vector2 ConvertToCord(Vector2 Pos)
    {
        return new Vector2(
            (Pos.x - tilePlacement.originX) / tilePlacement.xOffset,
            (Pos.y - tilePlacement.originY) / tilePlacement.yOffset
        );
    }
    void ShowVisual(Vector2 pos, float Rot)
    {
        var g = Instantiate(TilePrefab, pos, Quaternion.Euler(0, 0, Rot), EditTileHolders[CurrentTileIndex].transform);
        SetColor(g.transform);
    }
    #endregion
}

[System.Serializable]
public class TilesPerLevel
{
    public int levelNo = 0;
    public List<TilesInLevel> Tiles = new List<TilesInLevel>();
}
[System.Serializable]
public class TilesInLevel
{
    public int color = 0;
    public float HintXPos = 0;
    public float HintYPos = 0;
    public float InitRot = 0;
    public List<List<int>> coord = new List<List<int>>();
}

