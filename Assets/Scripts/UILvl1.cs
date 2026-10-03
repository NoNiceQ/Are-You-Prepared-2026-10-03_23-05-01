using UnityEngine;
using UnityEngine.UI; // Wymagane dla komponentu Image

public class UILvl1 : MonoBehaviour
{
    [Header("3 Sloty na UI")]
    public Image image0;
    public Image image1;
    public Image image2;

    // Tablica pomocnicza do łatwiejszego zarządzania slotami w pętli
    private Image[] slotImages;

    void Awake()
    {
        slotImages = new Image[] { image0, image1, image2 };
    }

    // Główna funkcja aktualizująca UI slotów na podstawie listy sprite'ów niesionych przedmiotów
    public void UiUpdate(Sprite[] carriedSprites)
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            if (slotImages[i] != null)
            {
                // Jeśli w tym slocie jest przedmiot (ma sprite)
                if (i < carriedSprites.Length && carriedSprites[i] != null)
                {
                    slotImages[i].sprite = carriedSprites[i];
                    SetAlpha(slotImages[i], 1f); // Pełna widoczność
                }
                else
                {
                    // Pusty slot - czyscimy i ustawiamy alpha na 0
                    slotImages[i].sprite = null;
                    SetAlpha(slotImages[i], 0f); // Niewidoczny
                }
            }
        }
    }

    // Pomocnicza funkcja do zmiany przezroczystości (alpha)
    private void SetAlpha(Image img, float alphaValue)
    {
        Color c = img.color;
        c.a = alphaValue;
        img.color = c;
    }
}