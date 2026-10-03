using UnityEngine;

public class MouseItemPicker : MonoBehaviour
{
    public float maxClickDistance = 3f; // Promień poziomy (szerokość)
    public Transform playerTransform;   // Przeciągnij tutaj transform gracza
    public float heightMultiplier = 2f; // Wysokość elipsy (jajowate Gizmo)

    public Inventory inv;

    void Update()
    {
        // Kiedy klikniesz lewym przyciskiem myszy
        if (Input.GetMouseButtonDown(0))
        {
            // Pobieramy pozycję kliknięcia myszką w świecie gry (2D)
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Strzelamy promieniem w punkt kliknięcia myszką
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                // Sprawdzamy czy kliknięty obiekt ma nasz komponent przedmiotu
                ItemsPickUpLVL1 item = hit.collider.GetComponent<ItemsPickUpLVL1>();

                if (item != null)
                {
                    Transform target = playerTransform != null ? playerTransform : transform;
                    float distance = Vector2.Distance(target.position, hit.transform.position);
                    
                    // Sprawdzamy czy gracz jest w zasięgu
                    if (distance <= maxClickDistance)
                    {
                        // Wywołujemy funkcję interakcji
                        Interact(item, hit.collider.gameObject);
                    }
                    else
                    {
                        // ZA DALEKO
                        Debug.Log($"[ZA DALEKO] Przedmiot '{item.itemName}' (ID: {item.iD}) jest za daleko! Dystans: {distance:F2}m.");
                    }
                }
            }
        }
    }

    // Funkcja interakcji (podnoszenie, niszczenie i miejsce na Twoją logikę)
    // Funkcja interakcji (podnoszenie, niszczenie i dodawanie do ekwipunku po ID)
    void Interact(ItemsPickUpLVL1 item, GameObject itemObject)
    {
        Debug.Log($"[INTERAKCJA] Podniesiono: '{item.itemName}' | ID: {item.iD}");

        // Sprawdzamy czy masz przypisany skrypt Inventory w inspektorze
        if (inv != null)
        {
            // Switch po ID przedmiotu z poziomu skryptu ItemsPickUpLVL1
            switch (item.iD)
            {
                case 0:
                    inv.water++;
                    Debug.Log("Dodano wodę! Stan: " + inv.water);
                    break;

                case 1:
                    inv.food++;
                    Debug.Log("Dodano jedzenie/konserwę! Stan: " + inv.food);
                    break;

                case 2:
                    inv.axe++;
                    Debug.Log("Dodano siekierę! Stan: " + inv.axe);
                    break;

                case 3:
                    inv.flashlight++;
                    Debug.Log("Dodano latarkę! Stan: " + inv.flashlight);
                    break;

                case 4:
                    inv.backpack++;
                    Debug.Log("Dodano plecak! Stan: " + inv.backpack);
                    break;

                case 5:
                    inv.medkit++;
                    Debug.Log("Dodano apteczkę! Stan: " + inv.medkit);
                    break;

                case 6:
                    inv.radio++;
                    Debug.Log("Dodano radio! Stan: " + inv.radio);
                    break;

                case 7:
                    inv.tape++;
                    Debug.Log("Dodano taśmę! Stan: " + inv.tape);
                    break;

                default:
                    Debug.LogWarning($"Nieznane ID przedmiotu: {item.iD} (Może to śmieć / pułapka?)");
                    break;
            }
        }
        else
        {
            Debug.LogError("Brak przypisanego Inventory w MouseItemPicker!");
        }

        // Niszczenie fizycznego obiektu z mapy
        Destroy(itemObject);
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