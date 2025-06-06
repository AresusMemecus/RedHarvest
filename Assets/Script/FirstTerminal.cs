using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using TMPro;

public class FirstTerminal : MonoBehaviour
{
    public GridLayoutGroup PlanetGrid;
    public GameObject TextChoosePlanet;
    public TMP_Text TextChoosePlanetText;
    public GameObject planetImagePrefab;
    public GameObject DenyMark, CloseLock, AcceptMark, OpenLock;
    public Sprite[] planetSprites;
    private Sprite correctPlanet;
    public bool isActivated = false;

    void Start()
    {
        if (!isActivated)
        {
            DisplayPlanets();
        }
    }

    void DisplayPlanets()
    {
        // Очищаем старые элементы (если есть)
        foreach (Transform child in PlanetGrid.transform)
        {
            Destroy(child.gameObject);
        }

        // Копируем спрайты и перемешиваем
        List<Sprite> randomized = new List<Sprite>(planetSprites);
        Shuffle(randomized);

        // Случайно выбрать правильную планету
        int correctIndex = Random.Range(0, randomized.Count);
        correctPlanet = randomized[correctIndex];

        if (TextChoosePlanetText != null)
        {
            TextChoosePlanetText.text = "Найдите: " + correctPlanet.name + "..?";
        }

        // Создаём Image+Button элементы
        foreach (Sprite sprite in randomized)
        {
            GameObject planetGO = Instantiate(planetImagePrefab, PlanetGrid.transform);
            UnityEngine.UI.Image img = planetGO.GetComponent<UnityEngine.UI.Image>();
            img.sprite = sprite;

            UnityEngine.UI.Button btn = planetGO.GetComponent<UnityEngine.UI.Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => OnPlanetClicked(sprite));
            }
        }
    }

    void OnPlanetClicked(Sprite clickedSprite)
    {
        if (isActivated)
        {
            return;
        }
        if (clickedSprite == correctPlanet)
        {
            DenyMark.SetActive(false);
            CloseLock.SetActive(false);
            AcceptMark.SetActive(true);
            OpenLock.SetActive(true);
            isActivated = true;
        }
        else
        {
            DisplayPlanets();
        }
    }

    void Shuffle(List<Sprite> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Sprite temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
