using UnityEngine;
using System.Collections.Generic;

public class TilePlacement : MonoBehaviour
{
    public float originX = 0, originY = 1.8f;
    public float xOffset = 0.45f, yOffset = 0.75f;
    int startY = 0;
    [SerializeField] GameObject TileBackground, TileObj;

    public void SetLevelWithConfig(List<List<int>> levelConfig, int startY = 0,Transform parent = null)
    {
        for (int i = 0; i < levelConfig.Count; i++)
        {
            var row = levelConfig[i];
            for (int j = 0; j < row.Count; j++)
            {
                var tile = Instantiate(TileBackground, Vector3.zero, Quaternion.identity, parent);
                var tileRot = (i + Mathf.Abs(row[j])) % 2 == 0 ? 0 : 180;

                tile.transform.localEulerAngles = new Vector3(0, 0, tileRot);
                tile.transform.position = new Vector3(originX + (xOffset * row[j]), startY + originY + (yOffset * i), 0);
            }
        }
    }
    public GameObject SetTileConfig(List<List<int>> levelConfig, int color = 0, int Rot = 0)
    {
        var TileParent = new GameObject("Tile");
        TileParent.AddComponent<TileInstance>();
        for (int i = 0; i < levelConfig.Count; i++)
        {
            var row = levelConfig[i];
            for (int j = 0; j < row.Count; j++)
            {
                var tile = Instantiate(TileObj, Vector3.zero, Quaternion.identity, TileParent.transform);
                var tileRot = (i + row[j]) % 2 == 0 ? 0 : 180;
                tileRot -= Rot;

                tile.transform.localEulerAngles = new Vector3(0, 0, tileRot);
                tile.transform.position = new Vector3(0 + ((xOffset - xmin) * row[j]), 0 + ((yOffset - ymin) * i), 0);
                foreach (Transform g in tile.transform) { g.gameObject.SetActive(false); }
                tile.transform.GetChild(color - 1).gameObject.SetActive(true);
            }
        }
        return TileParent;
    }
    [SerializeField] float xmin = 0.1f, ymin = 0.05f;
    [ContextMenu("genrateTile")]
    void Test()
    {
        List<List<int>> TestConfig = new List<List<int>>(){
            new List<int>(){-2, -1, 0, 1},
            new List<int> { -1, 0, 1, 2, 3 }
        };


        List<List<int>> TestTileConfig = new List<List<int>>(){
            new List<int>(){-1, 0, 1},
            new List<int> { 0 }
        };
        List<List<int>> TestTileConfig1 = new List<List<int>>(){
            new List<int>(){ 0 },
            new List<int> { 0 }
        };

        SetLevelWithConfig(TestConfig, 0);
        // SetTileConfig(TestTileConfig1, 5, 180);
    }

    private void Start()
    {
        //Test();
    }


}
