using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetManager : MonoBehaviour
{
    private const int TOTAL_CATEGORIES = 7;
    private const int PETS_PER_CATEGORY = 7;
    private const int MAX_UPGRADE_LEVEL = 5;

    [Serializable]
    public class CategoryUI
    {
        [Header("Interfaz de categoria")]
        public GameObject lockOverlay;
        public Button enterButton;

        [Header("Compra de mascotas")]
        public Button buyPetButton;
        public TextMeshProUGUI petProgressText;
        public TextMeshProUGUI petPriceText;

        [Header("Mejoras")]
        public Button speedButton;
        public Button clickButton;
        public Button incomeButton;

        public TextMeshProUGUI speedText;
        public TextMeshProUGUI clickText;
        public TextMeshProUGUI incomeText;

        [Header("Imagenes de mejoras")]
        public Image speedImage;
        public Image clickImage;
        public Image incomeImage;

        [Header("Rombos de mejoras")]
        public Image[] speedDiamonds = new Image[5];
        public Image[] clickDiamonds = new Image[5];
        public Image[] incomeDiamonds = new Image[5];

        [Header("Mascotas visuales")]
        public GameObject[] petObjects = new GameObject[7];
    }

    [Header("Referencias")]
    [SerializeField] private Game game;

    [Header("Categorias")]
    [SerializeField]
    private CategoryUI[] categories =
        new CategoryUI[TOTAL_CATEGORIES];

    [Header("Colores de mejoras")]
    [SerializeField]
    private Color speedColor =
        new Color(0.15f, 0.48f, 0.90f);

    [SerializeField]
    private Color clickColor =
        new Color(0.90f, 0.52f, 0.10f);

    [SerializeField]
    private Color incomeColor =
        new Color(0.12f, 0.65f, 0.38f);

    [SerializeField]
    private Color inactiveDiamondColor =
        Color.gray;

    [Header("Colores de categorias")]
    [SerializeField] private Color unlockedColor = Color.white;
    [SerializeField] private Color lockedColor = Color.gray;
    [SerializeField] private Color selectedColor = Color.green;

    private int[] petsBought = new int[TOTAL_CATEGORIES];
    private int[] speedLevels = new int[TOTAL_CATEGORIES];
    private int[] clickLevels = new int[TOTAL_CATEGORIES];
    private int[] incomeLevels = new int[TOTAL_CATEGORIES];

    private int selectedCategory = 0;
    private int unlockedCategories = 1;

    private void Awake()
    {
        PrepareCategories();
        LoadProgress();
    }

    private void Start()
    {
        SetupButtons();
        UpdateAllCategories();
    }

    // Preparar las siete categorias.
    private void PrepareCategories()
    {
        if (categories == null)
            categories = new CategoryUI[TOTAL_CATEGORIES];

        if (categories.Length != TOTAL_CATEGORIES)
            Array.Resize(ref categories, TOTAL_CATEGORIES);

        for (int i = 0; i < TOTAL_CATEGORIES; i++)
        {
            if (categories[i] == null)
                categories[i] = new CategoryUI();

            PrepareArray(ref categories[i].petObjects, 7);
            PrepareArray(ref categories[i].speedDiamonds, 5);
            PrepareArray(ref categories[i].clickDiamonds, 5);
            PrepareArray(ref categories[i].incomeDiamonds, 5);
        }
    }

    private void PrepareArray<T>(ref T[] array, int size)
    {
        if (array == null)
            array = new T[size];

        if (array.Length != size)
            Array.Resize(ref array, size);
    }

    // Conectar los botones de todas las categorias.
    private void SetupButtons()
    {
        for (int i = 0; i < TOTAL_CATEGORIES; i++)
        {
            int category = i;
            CategoryUI ui = categories[i];

            if (ui.enterButton != null)
            {
                ui.enterButton.onClick.AddListener(
                    () => SelectCategory(category)
                );
            }

            if (ui.buyPetButton != null)
            {
                ui.buyPetButton.onClick.AddListener(
                    () => BuyNextPet(category)
                );
            }

            if (ui.speedButton != null)
            {
                ui.speedButton.onClick.AddListener(
                    () => UpgradeSpeed(category)
                );
            }

            if (ui.clickButton != null)
            {
                ui.clickButton.onClick.AddListener(
                    () => UpgradeClick(category)
                );
            }

            if (ui.incomeButton != null)
            {
                ui.incomeButton.onClick.AddListener(
                    () => UpgradeIncome(category)
                );
            }
        }
    }

    // Entrar a una categoria.
    public void SelectCategory(int category)
    {
        if (!IsValidCategory(category))
            return;

        if (category >= unlockedCategories)
            return;

        selectedCategory = category;

        SaveProgress();
        UpdateAllCategories();
    }

    private bool IsValidCategory(int category)
    {
        return category >= 0 && category < TOTAL_CATEGORIES;
    }

    // Comprar la siguiente mascota de una categoria.
    public void BuyNextPet(int category)
    {
        if (!IsValidCategory(category))
            return;

        if (category != selectedCategory)
            return;

        if (category >= unlockedCategories)
            return;

        if (petsBought[category] >= PETS_PER_CATEGORY)
            return;

        if (game == null)
        {
            Debug.LogError("Falta asignar Game en PetManager.");
            return;
        }

        int cost = GetPetCost(category, petsBought[category]);

        if (!game.TrySpendCoins(cost))
            return;

        petsBought[category]++;

        // Desbloquear solamente la siguiente categoria.
        if (petsBought[category] == PETS_PER_CATEGORY &&
            category < TOTAL_CATEGORIES - 1)
        {
            unlockedCategories = category + 2;

            Debug.Log(
                "¡Se ha desbloqueado la categoria " +
                (category + 2) + "!"
            );
        }

        SaveProgress();
        UpdateAllCategories();
        game.UpdateUI();
    }

    // Mejora de velocidad.
    public void UpgradeSpeed(int category)
    {
        if (!CanUpgrade(category, speedLevels))
            return;

        int cost = GetUpgradeCost(
            category,
            speedLevels[category],
            100
        );

        if (game == null || !game.TrySpendCoins(cost))
            return;

        speedLevels[category]++;

        SaveProgress();
        UpdateAllCategories();
        game.RefreshTimer();
    }

    // Mejora de clic.
    public void UpgradeClick(int category)
    {
        if (!CanUpgrade(category, clickLevels))
            return;

        int cost = GetUpgradeCost(
            category,
            clickLevels[category],
            75
        );

        if (game == null || !game.TrySpendCoins(cost))
            return;

        clickLevels[category]++;

        SaveProgress();
        UpdateAllCategories();
    }

    // Mejora de ingresos.
    public void UpgradeIncome(int category)
    {
        if (!CanUpgrade(category, incomeLevels))
            return;

        int cost = GetUpgradeCost(
            category,
            incomeLevels[category],
            100
        );

        if (game == null || !game.TrySpendCoins(cost))
            return;

        incomeLevels[category]++;

        SaveProgress();
        UpdateAllCategories();
    }

    private bool CanUpgrade(int category, int[] levels)
    {
        if (!IsValidCategory(category))
            return false;

        if (category != selectedCategory)
            return false;

        if (category >= unlockedCategories)
            return false;

        if (levels[category] >= MAX_UPGRADE_LEVEL)
            return false;

        return game != null;
    }

    // Costos de mascotas.
    private int GetPetCost(int category, int pet)
    {
        double cost =
            50.0 *
            Math.Pow(3, category) *
            Math.Pow(2, pet);

        return SafeValue(cost);
    }

    // Costos de mejoras.
    private int GetUpgradeCost(int category, int level, int baseCost)
    {
        double cost =
            baseCost *
            Math.Pow(10, category) *
            Math.Pow(1.8, level);

        return SafeValue(cost);
    }

    // Velocidad global.
    public float GetTimerInterval()
    {
        int totalLevels = 0;

        for (int i = 0; i < TOTAL_CATEGORIES; i++)
            totalLevels += speedLevels[i];

        return Mathf.Max(0.1f, 1f - totalLevels * 0.05f);
    }

    // Valor global del clic.
    public int GetClickValue()
    {
        double total = 1;

        for (int i = 0; i < TOTAL_CATEGORIES; i++)
            total += clickLevels[i] * (i + 1);

        return SafeValue(total);
    }

    // Ingreso pasivo global.
    public int GetTotalPassiveIncome()
    {
        double total = 1;

        for (int i = 0; i < TOTAL_CATEGORIES; i++)
        {
            total += petsBought[i] * (i + 1) * 2;
            total += incomeLevels[i] * (i + 1) * 2;
        }

        return SafeValue(total);
    }

    // Actualizar todas las categorias.
    private void UpdateAllCategories()
    {
        for (int i = 0; i < TOTAL_CATEGORIES; i++)
        {
            UpdateCategory(i);
        }
    }

    private void UpdateCategory(int category)
    {
        CategoryUI ui = categories[category];

        bool unlocked = category < unlockedCategories;
        bool selected = category == selectedCategory;

        // Overlay oscuro.
        if (ui.lockOverlay != null)
            ui.lockOverlay.SetActive(!unlocked);

        // Boton para entrar.
        if (ui.enterButton != null)
        {
            ui.enterButton.interactable = unlocked;

            Image buttonImage = ui.enterButton.image;

            if (buttonImage != null)
            {
                buttonImage.color =
                    !unlocked ? lockedColor :
                    selected ? selectedColor :
                    unlockedColor;
            }
        }

        // Los botones de compra y mejoras solo funcionan
        // en la categoria desbloqueada que esta seleccionada.
        bool canUse = unlocked && selected;

        SetButtonState(ui.buyPetButton,
            canUse && petsBought[category] < PETS_PER_CATEGORY);

        SetButtonState(ui.speedButton,
            canUse && speedLevels[category] < MAX_UPGRADE_LEVEL);

        SetButtonState(ui.clickButton,
            canUse && clickLevels[category] < MAX_UPGRADE_LEVEL);

        SetButtonState(ui.incomeButton,
            canUse && incomeLevels[category] < MAX_UPGRADE_LEVEL);

        // Textos de mascotas.
        if (ui.petProgressText != null)
        {
            ui.petProgressText.text =
                "Mascotas: " + petsBought[category] + " / 7";
        }

        if (ui.petPriceText != null)
        {
            ui.petPriceText.text =
                petsBought[category] >= PETS_PER_CATEGORY
                ? "Categoria completada"
                : "Precio: " +
                  GetPetCost(category, petsBought[category]);
        }

        // Textos de mejoras.
        UpdateUpgradeText(
            ui.speedText, speedLevels[category], category, 100);

        UpdateUpgradeText(
            ui.clickText, clickLevels[category], category, 75);

        UpdateUpgradeText(
            ui.incomeText, incomeLevels[category], category, 100);

        // Colores de las imagenes.
        if (ui.speedImage != null)
            ui.speedImage.color = speedColor;

        if (ui.clickImage != null)
            ui.clickImage.color = clickColor;

        if (ui.incomeImage != null)
            ui.incomeImage.color = incomeColor;

        // Rombos.
        UpdateDiamonds(
            ui.speedDiamonds, speedLevels[category], speedColor);

        UpdateDiamonds(
            ui.clickDiamonds, clickLevels[category], clickColor);

        UpdateDiamonds(
            ui.incomeDiamonds, incomeLevels[category], incomeColor);

        // Mascotas visuales.
        for (int pet = 0; pet < PETS_PER_CATEGORY; pet++)
        {
            GameObject petObject = ui.petObjects[pet];

            if (petObject == null)
                continue;

            bool shouldBeActive =
                selected && pet < petsBought[category];

            if (petObject.activeSelf != shouldBeActive)
                petObject.SetActive(shouldBeActive);
        }
    }

    private void SetButtonState(Button button, bool state)
    {
        if (button != null)
            button.interactable = state;
    }

    private void UpdateUpgradeText(
        TextMeshProUGUI text,
        int level,
        int category,
        int baseCost)
    {
        if (text == null)
            return;

        if (level >= MAX_UPGRADE_LEVEL)
        {
            text.text = "Nivel MAX (5/5)";
            return;
        }

        text.text =
            "Nivel " + level + "/5\nCosto: " +
            GetUpgradeCost(category, level, baseCost);
    }

    private void UpdateDiamonds(
        Image[] diamonds,
        int level,
        Color activeColor)
    {
        if (diamonds == null)
            return;

        for (int i = 0; i < diamonds.Length; i++)
        {
            if (diamonds[i] == null)
                continue;

            diamonds[i].color =
                i < level ? activeColor : inactiveDiamondColor;
        }
    }

    // Guardar progreso.
    private void SaveProgress()
    {
        for (int i = 0; i < TOTAL_CATEGORIES; i++)
        {
            PlayerPrefs.SetInt("Pets_" + i, petsBought[i]);
            PlayerPrefs.SetInt("Speed_" + i, speedLevels[i]);
            PlayerPrefs.SetInt("Click_" + i, clickLevels[i]);
            PlayerPrefs.SetInt("Income_" + i, incomeLevels[i]);
        }

        PlayerPrefs.SetInt("SelectedCategory", selectedCategory);
        PlayerPrefs.SetInt("UnlockedCategories", unlockedCategories);

        PlayerPrefs.Save();
    }

    // Cargar progreso.
    private void LoadProgress()
    {
        for (int i = 0; i < TOTAL_CATEGORIES; i++)
        {
            petsBought[i] = Mathf.Clamp(
                PlayerPrefs.GetInt("Pets_" + i, 0), 0, 7);

            speedLevels[i] = Mathf.Clamp(
                PlayerPrefs.GetInt("Speed_" + i, 0), 0, 5);

            clickLevels[i] = Mathf.Clamp(
                PlayerPrefs.GetInt("Click_" + i, 0), 0, 5);

            incomeLevels[i] = Mathf.Clamp(
                PlayerPrefs.GetInt("Income_" + i, 0), 0, 5);
        }

        // Calcular desbloqueos en orden.
        unlockedCategories = 1;

        for (int i = 0; i < TOTAL_CATEGORIES - 1; i++)
        {
            if (petsBought[i] < PETS_PER_CATEGORY)
                break;

            unlockedCategories = i + 2;
        }

        selectedCategory = Mathf.Clamp(
            PlayerPrefs.GetInt("SelectedCategory", 0),
            0,
            unlockedCategories - 1
        );

        SaveProgress();
    }

    private int SafeValue(double value)
    {
        if (double.IsNaN(value) || value <= 0)
            return 0;

        if (value >= int.MaxValue)
            return int.MaxValue;

        return (int)Math.Ceiling(value);
    }
}
