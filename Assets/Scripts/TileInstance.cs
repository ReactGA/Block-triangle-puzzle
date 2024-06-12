using UnityEngine;
using System.Collections;
using System.Linq;

public class TileInstance : MonoBehaviour
{
    [HideInInspector] public int[] TileInPlace;
    [HideInInspector] public bool CurrentlyPicked { get; private set; }
    private void Start()
    {
        TileInPlace = new int[transform.childCount];
        //StartCoroutine(DebugCorotine());
    }

    public void DropTileInGrid(Vector2 center)
    {
        AudioManager.PlaySound(Sounds.blockPlace);
        transform.position = center;
        transform.parent = GameManager.Instance.gridTilesBG;
        SetTileSortingOrder(0);
        PickAndDrop.Instance.PopulateTilesInSlot();
        PickAndDrop.Instance.CheckSolutionNew();
        PickAndDrop.Instance.CheckSolution();
        Utils.Invoke(0.1f, () => PickAndDrop.Instance.SetTilesPos());
        Utils.Invoke(0.5f, () => PickAndDrop.Instance.CheckSolutionNew());
        Utils.Invoke(1f, () => PickAndDrop.Instance.CheckSolution());
    }
    public void SetTileSortingOrder(int index){
        foreach(Transform t in transform) {
            var sprites = t.GetComponentsInChildren<SpriteRenderer>();
            sprites.ToList().ForEach(i => i.sortingOrder = index);
        }
    }
    public bool StartDebug;
    IEnumerator DebugCorotine()
    {
        while (true)
        {
            if (StartDebug)
            {
                var str = "";
                for (int i = 0; i < TileInPlace.Length; i++)
                {
                    str += TileInPlace[i] + " , ";
                }
                Debug.Log(str);
            }
            yield return new WaitForSeconds(2);
        }

    }
    public void ReturnTile()
    {
        if (transform.parent != GameManager.Instance.gridTilesBG)
        {
            var x = (transform.GetSiblingIndex() * 2) - 1;
            transform.localPosition = new Vector2(x, 0);
        }
        else
        {
            transform.parent = GameManager.Instance.TileBackground;
            var x = (transform.GetSiblingIndex() * 2) - 1;
            transform.localPosition = new Vector2(x, 0);
        }
        SetTileSortingOrder(0);
    }
    public void SetPicked(bool value)
    {
        CurrentlyPicked = value;
    }
    public bool droppable()
    {
        for (int i = 0; i < TileInPlace.Length; i++)
        {
            if (TileInPlace[i] != 1) return false;
        }
        return true;
    }
    public bool droppable1()
    {
        foreach(Transform t in transform) {
            var piece = t.GetComponent<TilePiece>();
            if(piece.OnAnotherTile) return false;
        }
        return true;
    }
}
