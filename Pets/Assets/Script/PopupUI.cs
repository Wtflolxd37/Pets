using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

public class PopupUI : UIWindow
{
    #region Test

    [Button]
    private void ShowTest()
    {
        Show();
    }

    [Button]
    private void HideTest()
    {
        Hide();
    }


    #endregion
}
