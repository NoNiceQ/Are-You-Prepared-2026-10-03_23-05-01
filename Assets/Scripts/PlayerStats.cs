using UnityEngine;
using UnityEngine.UI; // Wymagane dla komponentu Image

public class PlayerStats : MonoBehaviour
{
    [Header("Statystyki Gracza")]
    public int day = 1;
    public int hydration = 80;
    public int hunder = 80;
    public int sanity = 80;

    public int maxValue = 100;

    [Header("Kolory Pasków")]
    public Color goodColor = Color.green; // Dobry stan (100%)
    public Color badColor = Color.red;    // Zły stan (0%)

    [Header("Referencje do Pasków UI (Image ze skryptem Filled)")]
    public Image hydrationBar;
    public Image hungerBar;
    public Image sanityBar;

    void Update()
    {
        UpdateUI();
    }

    // Odświeża długość i kolor pasków w UI
    public void UpdateUI()
    {
        UpdateBar(hydrationBar, hydration);
        UpdateBar(hungerBar, hunder);
        UpdateBar(sanityBar, sanity);
    }

    private void UpdateBar(Image bar, int currentVal)
    {
        if (bar == null) return;

        // Przeliczenie wartości na ułamek od 0.0 do 1.0
        float fillRatio = Mathf.Clamp01((float)currentVal / maxValue);

        // Ustawienie wypełnienia
        bar.fillAmount = fillRatio;

        // Płynny gradient koloru od czerwonego (0) do zielonego (1)
        bar.color = Color.Lerp(badColor, goodColor, fillRatio);
    }
}