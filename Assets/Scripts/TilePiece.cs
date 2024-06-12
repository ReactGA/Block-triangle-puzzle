using UnityEngine;

public class TilePiece : MonoBehaviour
{
    Transform DropPos;
    TileInstance tileInstance;
    [HideInInspector]public bool OnAnotherTile;
    private void Start()
    {
        PickAndDrop.Instance.droppedNotify += DroppedTileSet;
        tileInstance = GetComponentInParent<TileInstance>();
    }
    private void OnTriggerStay2D(Collider2D c)
    {
        if (c.GetComponent<TilePiece>())
        {
            OnAnotherTile = true;
        }
        else
        {
            if (Mathf.Abs(c.transform.localEulerAngles.z) != Mathf.Abs(transform.localEulerAngles.z)) return;
            tileInstance.TileInPlace[transform.GetSiblingIndex()] = 1;
            // Debug.Log(c.transform.name +"->"+ transform.name +" = "+transform.GetSiblingIndex());
            DropPos = c.transform;
        }
    }
    private void OnTriggerExit2D(Collider2D c)
    {
        if (c.GetComponent<TilePiece>())
        {
            OnAnotherTile = false;
        }
        else
        {
            tileInstance.TileInPlace[transform.GetSiblingIndex()] = 0;
            // Debug.Log("Exit:  "+c.transform.name +"->"+ transform.name +" = "+transform.GetSiblingIndex());
            DropPos = null;
        }
    }

    void DroppedTileSet()
    {
        if (!tileInstance.CurrentlyPicked) return;
        if (transform.localPosition == Vector3.zero)
        {
            if (tileInstance.droppable() && tileInstance.droppable1())
            {
                tileInstance.DropTileInGrid(DropPos.position);
            }
            else
            {
                tileInstance.ReturnTile();
            }
            tileInstance.SetPicked(false);
        }

    }
}