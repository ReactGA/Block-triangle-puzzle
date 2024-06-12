using UnityEngine;
using System;
using System.Collections.Generic;

public class PickAndDrop : MonoBehaviour
{
    public static PickAndDrop Instance;
    public event Action droppedNotify;
    public float initYPos = -2.8f;
    Transform Picked;
    Vector2 mousePos;
    Vector2 startDiff;
    public int arbitararyTilelenght = 3;
    // public Transform TileHolder, GridHolder;
    private void Awake()
    {
        Instance = this;
    }
    private void Pick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider && hit.transform.GetComponentInParent<TileInstance>())
            {
                Picked = hit.transform.parent;
                startDiff = Picked.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                ToogleCol(false);
                Picked.GetComponent<TileInstance>().SetTileSortingOrder(1);

                //debug
                // Picked.GetComponent<TileInstance>().StartDebug = true;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ToogleCol(true);
            droppedNotify?.Invoke();
            //debug
            // Picked.GetComponent<TileInstance>().StartDebug = false;

            Picked = null;
        }
    }

    private void Move()
    {
        if (Picked)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Picked.position = mousePos + startDiff;
        }
    }
    private void Update()
    {
        Pick();
        Move();
    }

    void ToogleCol(bool value)
    {
        if (!Picked) return;
        Picked.GetComponent<TileInstance>().SetPicked(true);
        foreach (Transform t in Picked)
        {
            t.GetComponent<PolygonCollider2D>().enabled = value;
        }
    }

    public void SetTilesPos()
    {
        foreach (Transform t in GameManager.Instance.TileBackground)
        {
            var x = ((t.GetSiblingIndex() * 2) - 1);
            t.localPosition = new Vector3(x, 0, 0);
        }
    }

    public void CheckSolution()
    {
        if (Allfilled())
        {
            GameManager.Instance.CompletedLevel();
        }
    }

    bool Allfilled()
    {
        foreach (Transform t in GameManager.Instance.gridBackground)
        {
            if (t.GetComponent<Collider2D>())
            {
                t.GetComponent<Collider2D>().enabled = false;
                Utils.Invoke(0.1f, () => t.GetComponent<Collider2D>().enabled = true);
                var col = Physics2D.OverlapCircle(t.position, 0.01f);
                if (!col) return false;
            }
        }
        return true;
    }
    int GridSlotsCount;
    int TilesInSlotsCount;
    public void PopulateGridSlots()
    {
        GridSlotsCount = GameManager.Instance.gridBackground.childCount;
        // Debug.Log("GridSlotsCount: " + GridSlotsCount);
    }
    public void PopulateTilesInSlot()
    {
        int tileCount = 0;
        foreach (Transform tile in GameManager.Instance.gridTilesBG)
        {
            tileCount += tile.childCount;
        }
        TilesInSlotsCount = tileCount;
        // Debug.Log("TilesInSlotsCount: " + TilesInSlotsCount);
    }
    public void CheckSolutionNew()
    {
        if (GridSlotsCount == TilesInSlotsCount)
        {
            GameManager.Instance.CompletedLevel();
        }
    }
}
