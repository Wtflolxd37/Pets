using UnityEngine;
using DG.Tweening;
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
    }

    #endregion

    #region
    [Button]

    public void HideTest()
    {
        Hide();
    }

    #endregion

    public void IniciarEscena(int nSccene)
    {
        SceneManager.LoadScene(nSccene);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
