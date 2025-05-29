using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryCardSpawner : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform gridParent;
    public Sprite[] cardFronts;  // Liste des images uniques disponibles
    public Sprite cardBack;
    public int numberOfPairs = 6;

    private List<Sprite> deck = new List<Sprite>();

    void Start()
    {
        if (numberOfPairs > cardFronts.Length)
        {
            Debug.LogWarning("numberOfPairs > cardFronts.Length, ajustement automatique");
            numberOfPairs = cardFronts.Length;
        }

        CreateDeck();
        ShuffleDeck();
        SpawnCards();
    }

    void CreateDeck()
    {
        deck.Clear();

        // Ajoute chaque sprite 2 fois pour créer les paires
        for (int i = 0; i < numberOfPairs; i++)
        {
            deck.Add(cardFronts[i]);
            deck.Add(cardFronts[i]);
        }
    }

    void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Sprite temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    void SpawnCards()
    {
        // Supprime d'anciennes cartes
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        // Instancie une carte par sprite dans le deck
        foreach (Sprite sprite in deck)
        {
            GameObject cardObj = Instantiate(cardPrefab, gridParent);
            Card card = cardObj.GetComponent<Card>();

            if (card == null)
            {
                Debug.LogError("Le prefab doit avoir le script Card attaché !");
                continue;
            }

            card.frontSprite = sprite;
            card.backSprite = cardBack;

            // Affiche le dos au départ
            Image img = cardObj.GetComponent<Image>();
            if (img != null)
                img.sprite = cardBack;

            // Setup du bouton
            Button btn = cardObj.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => card.OnClick());
            }
            else
            {
                Debug.LogWarning("Le prefab carte doit contenir un Button !");
            }
        }

        Debug.Log("Cartes générées : " + deck.Count);
    }
}
