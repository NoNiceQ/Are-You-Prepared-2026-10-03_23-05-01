using UnityEngine;

public class spawnObjects : MonoBehaviour
{
    public Inventory inv;

    [Header("Obiekty Unikalne (Scene2)")]
    public GameObject axeObject;
    public GameObject medkitObject;
    public GameObject flashlightObject;
    public GameObject tapeObject;
    public GameObject radioObject;

    [Header("Obiekty Wody (Do 4 pozycji)")]
    public GameObject water1;
    public GameObject water2;
    public GameObject water3;
    public GameObject water4;

    [Header("Obiekty Jedzenia (Do 4 pozycji)")]
    public GameObject food1;
    public GameObject food2;
    public GameObject food3;
    public GameObject food4;

    void Start()
    {
        CheckAndHideObjects();
    }

    public void CheckAndHideObjects()
    {
        if (inv == null) return;

        // --- Przedmioty unikalne (Pojawiają się gdy zapas > 0) ---
        if (axeObject != null) axeObject.SetActive(inv.axe > 0);
        if (medkitObject != null) medkitObject.SetActive(inv.medkit > 0);
        if (flashlightObject != null) flashlightObject.SetActive(inv.flashlight > 0);
        if (tapeObject != null) tapeObject.SetActive(inv.tape > 0);
        if (radioObject != null) radioObject.SetActive(inv.radio > 0);

        // --- Woda (Pojawia się do 4 sztuk na stole) ---
        if (water1 != null) water1.SetActive(inv.water >= 1);
        if (water2 != null) water2.SetActive(inv.water >= 2);
        if (water3 != null) water3.SetActive(inv.water >= 3);
        if (water4 != null) water4.SetActive(inv.water >= 4);

        // --- Jedzenie (Pojawia się do 4 sztuk na stole) ---
        if (food1 != null) food1.SetActive(inv.food >= 1);
        if (food2 != null) food2.SetActive(inv.food >= 2);
        if (food3 != null) food3.SetActive(inv.food >= 3);
        if (food4 != null) food4.SetActive(inv.food >= 4);
    }
}