using UnityEngine;
using UnityEngine.UI; // Wymagane dla komponentu Image
using System.Collections;
using TMPro;

public class switchLVL : MonoBehaviour
{
    public GameObject uiToHide;
    public GameObject textToHide;
    [Header("Timer / Czas Etapu 1")]
    public float timeRemaining = 40f;
    public bool timerIsRunning = true;
    public TextMeshProUGUI timerText;

    [Header("Pozycje Kamery")]
    public Camera mainCamera;
    public CameraLvl1 cameraScript;
    
    [Tooltip("Współrzędne piwnicy (dokąd kamera najpierw jedzie)")]
    public Vector3 basementCameraPosition = new Vector3(0f, -10f, -10f);

    [Tooltip("Współrzędne Etapu 2 / Dnia 1 (gdzie ląduje po ściemnieniu)")]
    public Vector3 dayOneCameraPosition = new Vector3(20f, -10f, -10f);

    public float moveSpeed = 3f;

    [Header("UI / Fade Effect (Image)")]
    public Image fadeImage;
    public float fadeDuration = 1f;

    [Header("Sprawdzenie Itemów i Spawn")]
    public Inventory inventory;
    public spawnObjects spawnScript; // Przeciągnij tutaj obiekt posiadający skrypt spawnObjects

    private bool hasSwitched = false;

    void Update()
    {
        if (timerIsRunning && !hasSwitched)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerUI(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                UpdateTimerUI(0);
                
                StartCoroutine(TransitionToDayOne());
            }
        }
    }

    public void StartDayOne()
    {
        if (!hasSwitched)
        {
            StartCoroutine(TransitionToDayOne());
        }
    }

    IEnumerator TransitionToDayOne()
    {
        hasSwitched = true;

        // 1. Auto-Deposit: Przepisanie przedmiotów z tymczasowego EQ gracza do magazynu Inventory
        AutoDepositItems();

        // 2. Wyłączamy śledzenie gracza przez kamerę
        if (cameraScript != null)
        {
            cameraScript.enabled = false;
        }

        uiToHide.SetActive(false);
        Destroy(textToHide);

        float startSize = 5f;
        float targetSize = 2f;

        // 3. Ruch kamery do piwnicy + zoom do size = 2
        while (Vector3.Distance(mainCamera.transform.position, basementCameraPosition) > 0.1f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, basementCameraPosition, Time.deltaTime * moveSpeed);
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetSize, Time.deltaTime * moveSpeed);
            yield return null;
        }

        mainCamera.transform.position = basementCameraPosition;
        mainCamera.orthographicSize = targetSize;

        // 4. Ściemnienie ekranu (Fade Out do czerni)
        if (fadeImage != null)
        {
            float elapsedFade = 0f;
            Color c = fadeImage.color;
            while (elapsedFade < fadeDuration)
            {
                elapsedFade += Time.deltaTime;
                c.a = Mathf.Clamp01(elapsedFade / fadeDuration);
                fadeImage.color = c;
                yield return null;
            }
        }

        // 5. TELEPORT kamery na pozycję Dnia 1 + reset Size do 5
        if (mainCamera != null)
        {
            mainCamera.transform.position = dayOneCameraPosition;
            mainCamera.orthographicSize = startSize;
        }

        // 6. Odświeżenie widoczności przedmiotów na stole dokładnie w czerni
        if (spawnScript != null)
        {
            spawnScript.CheckAndHideObjects();
        }

        yield return new WaitForSeconds(0.2f); // Krótka pauza w czerni

        // 7. Rozjaśnienie ekranu (Fade In)
        if (fadeImage != null)
        {
            float elapsedFade = 0f;
            Color c = fadeImage.color;
            while (elapsedFade < fadeDuration)
            {
                elapsedFade += Time.deltaTime;
                c.a = 1f - Mathf.Clamp01(elapsedFade / fadeDuration);
                fadeImage.color = c;
                yield return null;
            }
        }

        CheckInventoryForDayOne();
        Debug.Log("[SWITCH LVL] Przejście zakończone sukcesem!");
    }

    void AutoDepositItems()
    {
        // Jeśli Twój skrypt ekwipunku gracza/postaci automatycznie zrzuca przedmioty do 'inventory',
        // wywołaj tutaj odpowiednią funkcję. Jeśli przypisujesz wartości bezpośrednio, możesz dopisać je tutaj.
        Debug.Log("[SWITCH LVL] Automatyczny depozyt nieodniesionych przedmiotów do piwnicy.");
    }

    void CheckInventoryForDayOne()
    {
        if (inventory != null)
        {
            Debug.Log($"Zasoby na starcie Dnia 1: Woda={inventory.water}, Jedzenie={inventory.food}, Siekiera={inventory.axe}, Radio={inventory.radio}");
        }
    }

    void UpdateTimerUI(float timeToDisplay)
    {
        if (timerText != null)
        {
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}