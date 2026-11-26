using UnityEngine;
using DG.Tweening;
using Dino.Utility.Audio;
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
        AudioManager.Instance.PlaySound("Button");
    }

    public void OffCredits()
    {
        Hide();
        AudioManager.Instance.PlaySound("Button");
    }
}
