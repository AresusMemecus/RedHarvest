using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondTerminal : MonoBehaviour
{
    public GridLayoutGroup PlanetGrid;
    public GameObject planetImagePrefab;
    public GameObject DenyMark, CloseLock, AcceptMark, OpenLock;
    public Sprite[] Karnix;  // Три правильные
    public Sprite[] Karmix;  // Три неправильные

    private HashSet<Sprite> correctPlanets = new HashSet<Sprite>();
    private HashSet<Sprite> selectedCorrect = new HashSet<Sprite>();

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
        foreach (Transform child in PlanetGrid.transform)
        {
            Destroy(child.gameObject);
        }

        // Сброс
        correctPlanets.Clear();
        selectedCorrect.Clear();

        List<Sprite> mixed = new List<Sprite>();

        // Берём 3 Karnix и сохраняем как правильные
        List<Sprite> karnixList = new List<Sprite>(Karnix);
        Shuffle(karnixList);
        karnixList = karnixList.GetRange(0, 3);
        correctPlanets.UnionWith(karnixList);
        mixed.AddRange(karnixList);

        // 3 Karmix
        List<Sprite> karmixList = new List<Sprite>(Karmix);
        Shuffle(karmixList);
        mixed.AddRange(karmixList.GetRange(0, 3));

        // 3 пустых (null)
        for (int i = 0; i < 3; i++)
        {
            mixed.Add(null);
        }

        Shuffle(mixed);

        foreach (Sprite sprite in mixed)
        {
            GameObject planetGO = Instantiate(planetImagePrefab, PlanetGrid.transform);
            Image img = planetGO.GetComponent<Image>();
            Button btn = planetGO.GetComponent<Button>();

            if (sprite == null)
            {
                img.enabled = false;
                btn.interactable = false;
                continue;
            }

            img.sprite = sprite;

            btn.onClick.AddListener(() => OnPlanetClicked(sprite, btn));
        }
    }

    void OnPlanetClicked(Sprite clickedSprite, Button btn)
    {
        if (isActivated || selectedCorrect.Contains(clickedSprite))
            return;

        if (correctPlanets.Contains(clickedSprite))
        {
            selectedCorrect.Add(clickedSprite);
            btn.interactable = false;

            if (selectedCorrect.Count == 3)
            {
                Activate();
            }
        }
        else
        {
            DisplayPlanets();
        }
    }

    void Activate()
    {
        DenyMark.SetActive(false);
        CloseLock.SetActive(false);
        AcceptMark.SetActive(true);
        OpenLock.SetActive(true);
        isActivated = true;
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
