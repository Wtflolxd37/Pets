using UnityEngine;
using DG.Tweening;
using Dino.Utility.Audio;
using NaughtyAttributes;
using UnityEngine.SceneManagement;

public class MenuUI : UIWindow
{
    [SerializeField] private UIWindow creditsWindow;
    [SerializeField] private UIWindow settingsWindow;

    #region
    [Button] 

    public void ShowTest()
    {
        Show();
        AudioManager.Instance.PlaySound("Button");
    }

    #endregion

    #region
    [Button]

    public void HideTest()
    {
        Hide();
        AudioManager.Instance.PlaySound("Button");
    }

    #endregion

    public void IniciarEscena(int nSccene)
    {
        SceneManager.LoadScene(nSccene);
        AudioManager.Instance.PlaySound("Button");
    }

    public void Quit()
    {
        Application.Quit();
        AudioManager.Instance.PlaySound("Button");
    }
}
