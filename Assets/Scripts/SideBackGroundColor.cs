using UnityEngine;


public class SideBackGroundColor : MonoBehaviour
{
    [SerializeField]Color BothSideBackGroundColor;
    [Header("Object Reference")]
    [SerializeField]GameObject RightBGBlocker;
    [SerializeField]GameObject leftBGBlocker;
    void Awake()
    {
        RightBGBlocker.GetComponent<SpriteRenderer>().color = BothSideBackGroundColor;
        leftBGBlocker.GetComponent<SpriteRenderer>().color = BothSideBackGroundColor;
    }

}
