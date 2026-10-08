using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

public class MenuUI : UIWindow
{
    #region
    [Button] 

    private void ShowTest()
    {
        Show();
    }

    #endregion

    #region
    [Button]

    private void HideTest()
    {
        Hide();
    }

    #endregion
}
