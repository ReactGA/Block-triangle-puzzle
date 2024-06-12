using UnityEngine;
using UnityEngine.UI;

public class DynamicScrollRect : MonoBehaviour
{
    public GameObject AddMorebuttonsGrid(ScrollRect ScrRect, GameObject prefab, float gridSize = 2)
    {
        var ScrollContent = ScrRect.content;
        var gridLayout = ScrollContent.GetComponent<GridLayoutGroup>();
        var increment = gridLayout.cellSize.y + gridLayout.padding.top;
        var ScrcontentRect = ScrollContent.GetComponent<RectTransform>();
        if (ScrollContent.childCount % gridSize == 0)
        {
            ScrcontentRect.sizeDelta += new Vector2(0, increment);
        }
        SetMvtType(ScrRect, ScrcontentRect.sizeDelta.y);
        var newBtn = Instantiate(prefab);
        newBtn.transform.SetParent(ScrollContent, false);
        return newBtn;
    }

    public void RemovebuttonsGrid(ScrollRect ScrRect, GameObject btn = null, float gridSize = 2)
    {
        var ScrollContent = ScrRect.content;
        if (ScrollContent.childCount == 0)
            return;
        var gridLayout = ScrollContent.GetComponent<GridLayoutGroup>();
        var increment = gridLayout.cellSize.y + gridLayout.padding.top;
        var scrollRect = ScrollContent.GetComponent<RectTransform>();
        if (btn == null)
        {
            var LastBtn = ScrollContent.GetChild(ScrollContent.childCount - 1).gameObject;
            Destroy(LastBtn);
        }
        else
        {
            Destroy(btn);
        }


        if (ScrollContent.childCount % gridSize == 1)
        {
            scrollRect.sizeDelta -= new Vector2(0, increment);
        }
        SetMvtType(ScrRect, scrollRect.sizeDelta.y);
    }
    public void AddMorebuttonsVertical(ScrollRect ScrRect, GameObject prefab)
    {
        var newBtn = Instantiate(prefab);
        var btnHeight = newBtn.GetComponent<RectTransform>().rect.height;
        //ScrollContent
        var ScrollContent = ScrRect.content;
        var rect = ScrollContent.GetComponent<RectTransform>();
        var Layout = ScrollContent.GetComponent<VerticalLayoutGroup>();

        rect.sizeDelta += new Vector2(0, btnHeight + Layout.spacing);
        SetMvtType(ScrRect, rect.sizeDelta.y);
        //Btn
        newBtn.transform.SetParent(ScrollContent, false);
    }
    public void RemovebuttonsVertical(ScrollRect ScrRect, GameObject btn = null)
    {
        var ScrollContent = ScrRect.content;
        if (ScrollContent.childCount == 0)
            return;
        var LastBtn = ScrollContent.GetChild(ScrollContent.childCount - 1).gameObject;
        var btnHeight = LastBtn.GetComponent<RectTransform>().rect.height;
        //ScrollContent

        var rect = ScrollContent.GetComponent<RectTransform>();
        var Layout = ScrollContent.GetComponent<VerticalLayoutGroup>();

        rect.sizeDelta -= new Vector2(0, btnHeight + Layout.spacing);
        SetMvtType(ScrRect, rect.sizeDelta.y);
        //Btn
        if (btn == null)
        {
            Destroy(LastBtn);
        }
        else
        {
            Destroy(btn);
        }

    }
    void SetMvtType(ScrollRect ScrR, float ScrCont)
    {
        var scrollheight = ScrR.GetComponent<RectTransform>().sizeDelta.y;
        if (ScrCont > scrollheight)
        {
            ScrR.movementType = ScrollRect.MovementType.Elastic;
        }
        else
        {
            ScrR.movementType = ScrollRect.MovementType.Clamped;
        }
    }
}
