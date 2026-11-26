using Dino.Utils;
using Dino.Utility.Audio;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private ParticleSystem clickParticles;

    [Header("Dinero")]
    [SerializeField] private TextMeshProUGUI money;

    [Header("Sistema de mascotas")]
    [SerializeField] private PetManager petManager;

    [Header("Textos de ganancias")]
    [SerializeField] private TextMeshProUGUI clickGainText;
    [SerializeField] private TextMeshProUGUI petGainText;

    private int _moneyCurrent;
    private TimerBase time;

    private void Start()
    {
        time = GetComponent<TimerBase>();

        if (time == null)
        {
            Debug.LogError("No se encontro TimerBase");
            enabled = false;
            return;
        }

        LoadProgress();

        time.OnTimerLoop += AddCoins;

        time.StartLoopTimer(
            petManager.GetTimerInterval(),
            "",
            true,
            1f
        );

        UpdateUI();
    }

    // GANANCIAS AUTOMATICAS
    private void AddCoins()
    {
        int income = petManager.GetTotalPassiveIncome();

        _moneyCurrent = SafeAdd(_moneyCurrent, income);

        SaveProgress();
        UpdateUI();
    }

    // GANANCIAS POR CLIC
    public void RecoletCoins()
    {
        int clickValue = petManager.GetClickValue();

        _moneyCurrent = SafeAdd(_moneyCurrent, clickValue);

        if (clickParticles != null)
        {
            clickParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            clickParticles.Play();
        }

        Debug.Log("CLICK DE RECOLECCIÓN");

        if (clickParticles == null)
        {
            Debug.LogError("NO ESTÁ ASIGNADO ClickParticles");
        }
        else
        {
            Debug.Log("PARTÍCULAS EJECUTÁNDOSE");

            clickParticles.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );

            clickParticles.Play();
        }
        AudioManager.Instance.PlaySound("Button");

        SaveProgress();
        UpdateUI();
    }

    // COMPRAR UTILIZANDO MONEDAS
    public bool TrySpendCoins(int amount)
    {
        if (amount <= 0 || _moneyCurrent < amount)
        {
            Debug.Log("No tienes suficientes monedas");
            return false;
        }

        _moneyCurrent -= amount;

        SaveProgress();
        UpdateUI();

        return true;
    }

    // CAMBIAR VELOCIDAD DEL TEMPORIZADOR
    public void RefreshTimer()
    {
        if (time != null && petManager != null)
        {
            time.ChangeLoopTimer((float)petManager.GetTimerInterval());
        }
    }

    // ACTUALIZAR INTERFAZ
    public void UpdateUI()
    {
        if (money != null)
            money.text = _moneyCurrent.ToString("N0");
        UpdateGainTexts();
    }

    private int SafeAdd(int current, int amount)
    {
        return (int)Mathf.Clamp(
            (float)current + amount,
            0f,
            int.MaxValue
        );
    }

    // GUARDAR DINERO
    private void SaveProgress()
    {
        PlayerPrefs.SetInt("Clicker_Money", _moneyCurrent);
        PlayerPrefs.Save();
    }

    // CARGAR DINERO
    private void LoadProgress()
    {
        _moneyCurrent = PlayerPrefs.GetInt(
            "Clicker_Money",
            0
        );
    }

    private void OnApplicationQuit()
    {
        SaveProgress();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveProgress();
    }

    private void OnDestroy()
    {
        if (time != null)
            time.OnTimerLoop -= AddCoins;
    }

    private void UpdateGainTexts()
    {
        if (petManager == null) return;

        if (clickGainText != null)
            clickGainText.text = "+" + petManager.GetClickValue() + " por clic";

        if (petGainText != null)
            petGainText.text = "+" + petManager.GetTotalPassiveIncome() + " por ciclo";
    }

    public void CambiarEscena(int nScenas)
    {
        SceneManager.LoadScene(nScenas);
        AudioManager.Instance.PlaySound("Button");
    }
}
