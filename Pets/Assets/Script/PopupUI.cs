using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

public class PopupUI : UIWindow
{
    #region Test

    [Button]
    public void ShowTest()
    {
        Show();
    }

    [Button]
    public void HideTest()
    {
        Hide();
    }


    #endregion

    public void OnCrdits()
    {
        Show();
    }

    public void OffCredits()
    {
        Hide();
    }
}
