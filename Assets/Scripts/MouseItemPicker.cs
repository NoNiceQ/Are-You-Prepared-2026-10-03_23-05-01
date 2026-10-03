using UnityEngine;
using TMPro;

public class MouseItemPicker : MonoBehaviour
{
    [Header("Ustawienia Zasięgu")]
    public float maxClickDistance = 3f; // Promień poziomy (szerokość)
    public Transform playerTransform;   // Przeciągnij tutaj transform gracza
    public float heightMultiplier = 2f; // Wysokość elipsy (jajowate Gizmo)

    [Header("Referencje i Slot")]
    public Inventory inv;
    public UILvl1 uiLvl1;               // Przypisz tutaj obiekt z komponentem UILvl1
    public int carriedItemsCount = 0;   // Licznik aktualnie niesionych przedmiotów
    public int maxSlots = 3;            // Maksymalnie 3 sloty w rękach

    // Tablica przechowująca sprite'y niesionych przedmiotów
    private Sprite[] carriedSprites = new Sprite[3];

    [Header("UI / Komunikaty (World Space)")]
    public TextMeshProUGUI promptText;  // Przeciągnij tutaj swój tekst World Space TMP
    public Vector3 textOffset = new Vector3(0, 1.2f, 0); // Przesunięcie napisu nad przedmiotem

    void Update()
    {
        // 1. Sprawdzamy co jest pod kursorem (HOVER)
        HandleHover();

        // 2. Kiedy klikniesz lewym przyciskiem myszy
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                ItemsPickUpLVL1 item = hit.collider.GetComponent<ItemsPickUpLVL1>();

                if (item != null)
                {
                    Transform target = playerTransform != null ? playerTransform : transform;
                    float distance = Vector2.Distance(target.position, hit.transform.position);
                    
                    if (distance <= maxClickDistance)
                    {
                        if (item.iD == -1 || carriedItemsCount < maxSlots)
                        {
                            Interact(item, hit.collider.gameObject);
                        }
                        else
                        {
                            Debug.Log("[PLECAK PEŁNY] Masz już 3 przedmioty w rękach!");
                        }
                    }
                    else
                    {
                        Debug.Log($"[ZA DALEKO] Przedmiot '{item.itemName}' jest za daleko!");
                    }
                }
            }
        }
    }

    // Obsługa najeżdżania myszką (Hover) i pozycjonowania tekstu w World Space
    void HandleHover()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            ItemsPickUpLVL1 item = hit.collider.GetComponent<ItemsPickUpLVL1>();

            if (item != null)
            {
                Transform target = playerTransform != null ? playerTransform : transform;
                float distance = Vector2.Distance(target.position, hit.transform.position);

                string actionStatus = "";

                // Określamy stan
                if (distance > maxClickDistance)
                {
                    actionStatus = "to far";
                }
                else if (item.iD != -1 && carriedItemsCount >= maxSlots)
                {
                    actionStatus = "no more space";
                }
                else
                {
                    actionStatus = "Interact";
                }

                // Formatowanie tekstu
                string displayText = $"{item.itemName}\n- {actionStatus}";

                if (promptText != null)
                {
                    // Ustawiamy pozycję tekstu bezpośrednio w świecie gry + offset
                    promptText.transform.position = hit.collider.transform.position + textOffset;

                    promptText.text = displayText;
                    promptText.gameObject.SetActive(true);
                }
                return;
            }
        }

        // Jeśli kursor nie jest nad przedmiotem, wyłączamy tekst
        if (promptText != null)
        {
            promptText.text = "";
            promptText.gameObject.SetActive(false);
        }
    }

    // Funkcja interakcji (obsługa ekwipunku, slotów, depozytu i UI)
    void Interact(ItemsPickUpLVL1 item, GameObject itemObject)
    {
        Debug.Log($"[INTERAKCJA] Kliknięto: '{item.itemName}' | ID: {item.iD}");

        if (inv != null)
        {
            switch (item.iD)
            {
                case -1:
                    Debug.Log("[PIWNICA] Zdeponowano zasoby! Opróżniono sloty.");
                    carriedItemsCount = 0;
                    System.Array.Clear(carriedSprites, 0, carriedSprites.Length); // Czścimy tablicę sprite'ów
                    
                    if (uiLvl1 != null) uiLvl1.UiUpdate(carriedSprites);
                    break;

                case 0: 
                    inv.water++; 
                    AddCarryItem(itemObject);
                    break;
                case 1: 
                    inv.food++; 
                    AddCarryItem(itemObject);
                    break;
                case 2: 
                    inv.axe++; 
                    AddCarryItem(itemObject);
                    break;
                case 3: 
                    inv.flashlight++; 
                    AddCarryItem(itemObject);
                    break;
                case 4: 
                    inv.backpack++; 
                    AddCarryItem(itemObject);
                    break;
                case 5: 
                    inv.medkit++; 
                    AddCarryItem(itemObject);
                    break;
                case 6: 
                    inv.radio++; 
                    AddCarryItem(itemObject);
                    break;
                case 7: 
                    inv.tape++; 
                    AddCarryItem(itemObject);
                    break;

                default:
                    Debug.LogWarning($"Nieznane ID przedmiotu: {item.iD}");
                    break;
            }
        }
        else
        {
            Debug.LogError("Brak przypisanego Inventory w MouseItemPicker!");
        }

        // Niszczymy obiekt z mapy TYLKO JEŚLI TO NIE JEST DEPOZYT (-1)
        if (item.iD != -1)
        {
            Destroy(itemObject);
        }
    }

    // Pomocnicza metoda do pobierania sprite'a z podnoszonego obiektu i aktualizacji UI
    void AddCarryItem(GameObject itemObject)
    {
        SpriteRenderer sr = itemObject.GetComponent<SpriteRenderer>();
        Sprite itemSprite = sr != null ? sr.sprite : null;

        if (carriedItemsCount < maxSlots)
        {
            carriedSprites[carriedItemsCount] = itemSprite;
            carriedItemsCount++;

            if (uiLvl1 != null)
            {
                uiLvl1.UiUpdate(carriedSprites);
            }
        }
    }

    // Rysuje jajowate Gizmo w oknie Scene
    void OnDrawGizmosSelected()
    {
        Transform target = playerTransform != null ? playerTransform : transform;
        Vector3 center = target.position;

        Gizmos.color = Color.green;

        int segments = 30;
        Vector3 prevPoint = Vector3.zero;

        for (int i = 0; i <= segments; i++)
        {
            float angle = (float)i / segments * 2 * Mathf.PI;
            float x = Mathf.Cos(angle) * maxClickDistance;
            float y = Mathf.Sin(angle) * maxClickDistance * heightMultiplier; 

            Vector3 currentPoint = center + new Vector3(x, y, 0f);

            if (i > 0)
            {
                Gizmos.DrawLine(prevPoint, currentPoint);
            }

            prevPoint = currentPoint;
        }
    }
}