using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DossierBoard : MonoBehaviour
{
    [Header("Board Settings")]
    public int width = 8;
    public int height = 8;
    public float tileSize = 2f;
    public GameObject[] piecePrefabs;

    private FichierPiece[,] pieces;
    private FichierPiece selectedPiece;
    private bool isSwapping = false;
    private bool boardReady = false;

    [Header("Timer Settings")]
    public float timeLimit = 60f;
    private float timer;
    private bool gameEnded = false;

    [Header("UI")]
    public TextMeshProUGUI timerText;

    [Header ("score")]
    public TextMeshProUGUI scoreText;
    private int score = 0;
    public int scoreToWin = 50; //  50 dossiers détruits


    void Start()
    {
        MiniGameManager.Instance.SetCurrentMiniGame(MiniGameType.FichierCrush);
        pieces = new FichierPiece[width, height];
        timer = timeLimit;
        InitBoardWithoutMatches();
    }

    void Update()
    {
        if (gameEnded) return;

        timer -= Time.deltaTime;

        if (timerText != null)
            timerText.text = Mathf.CeilToInt(timer).ToString() + "s";

        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (timer <= 0f)
        {
            timer = 0f;
            GameOver(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("MainScene");
    }
    void InitBoardWithoutMatches()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                do
                {
                    if (pieces[x, y] != null)
                        Destroy(pieces[x, y].gameObject);

                    SpawnPiece(x, y);
                }
                while (HasMatchAt(x, y));
            }
        }

        boardReady = true;
    }

    void SpawnPiece(int x, int y)
    {
        Vector2 spawnPos = new Vector2(x * tileSize, y * tileSize);
        GameObject pieceObj = Instantiate(GetRandomPiece(), spawnPos, Quaternion.identity, this.transform);

        FichierPiece fichier = pieceObj.GetComponent<FichierPiece>();
        fichier.Init(x, y, this);

        SpriteRenderer sr = pieceObj.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.sortingOrder = height - y;

        pieces[x, y] = fichier;
    }

    GameObject GetRandomPiece()
    {
        return piecePrefabs[Random.Range(0, piecePrefabs.Length)];
    }

    public void SelectPiece(FichierPiece piece)
    {
        if (!boardReady || isSwapping) return;

        if (selectedPiece == null)
        {
            selectedPiece = piece;
        }
        else
        {
            if (AreAdjacent(selectedPiece, piece))
            {
                StartCoroutine(SwapAndCheck(selectedPiece, piece));
            }

            selectedPiece = null;
        }
    }

    bool AreAdjacent(FichierPiece a, FichierPiece b)
    {
        return (Mathf.Abs(a.x - b.x) == 1 && a.y == b.y) ||
               (Mathf.Abs(a.y - b.y) == 1 && a.x == b.x);
    }

    IEnumerator SwapAndCheck(FichierPiece a, FichierPiece b)
    {
        isSwapping = true;
        boardReady = false;

        yield return StartCoroutine(AnimateSwap(a, b));
        SwapInGrid(a, b);

        yield return new WaitForSeconds(0.1f);

        List<FichierPiece> matches = GetMatches();

        if (matches.Count > 0)
        {
            ClearMatches(matches);
        }
        else
        {
            yield return StartCoroutine(AnimateSwap(a, b));
            SwapInGrid(a, b);
        }

        yield return new WaitForSeconds(0.1f);

        isSwapping = false;
        boardReady = true;
    }

    IEnumerator AnimateSwap(FichierPiece a, FichierPiece b)
    {
        Vector3 posA = a.transform.position;
        Vector3 posB = b.transform.position;
        float duration = 0.2f;
        float time = 0f;

        while (time < duration)
        {
            a.transform.position = Vector3.Lerp(posA, posB, time / duration);
            b.transform.position = Vector3.Lerp(posB, posA, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        a.transform.position = posB;
        b.transform.position = posA;
    }

    void SwapInGrid(FichierPiece a, FichierPiece b)
    {
        pieces[a.x, a.y] = b;
        pieces[b.x, b.y] = a;

        (a.x, b.x) = (b.x, a.x);
        (a.y, b.y) = (b.y, a.y);
    }

    List<FichierPiece> GetMatches()
    {
        List<FichierPiece> matches = new List<FichierPiece>();

        // Horizontal
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width - 2; x++)
            {
                FichierPiece p1 = pieces[x, y];
                FichierPiece p2 = pieces[x + 1, y];
                FichierPiece p3 = pieces[x + 2, y];

                if (p1 && p2 && p3 && p1.tag == p2.tag && p2.tag == p3.tag)
                {
                    if (!matches.Contains(p1)) matches.Add(p1);
                    if (!matches.Contains(p2)) matches.Add(p2);
                    if (!matches.Contains(p3)) matches.Add(p3);
                }
            }
        }

        // Vertical
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height - 2; y++)
            {
                FichierPiece p1 = pieces[x, y];
                FichierPiece p2 = pieces[x, y + 1];
                FichierPiece p3 = pieces[x, y + 2];

                if (p1 && p2 && p3 && p1.tag == p2.tag && p2.tag == p3.tag)
                {
                    if (!matches.Contains(p1)) matches.Add(p1);
                    if (!matches.Contains(p2)) matches.Add(p2);
                    if (!matches.Contains(p3)) matches.Add(p3);
                }
            }
        }

        return matches;
    }

    void ClearMatches(List<FichierPiece> matches)
    {
        foreach (FichierPiece piece in matches)
        {
            if (piece != null)
            {
                pieces[piece.x, piece.y] = null;
                Destroy(piece.gameObject);

                score += 5;  // +5 par dossier détruit
            }
        }

        if (score >= scoreToWin)
        {
            GameOver(true); // Victoire si score atteint
        }
        else
        {
            StartCoroutine(FillBoard());
        }
    }



    IEnumerator FillBoard()
    {
        yield return new WaitForSeconds(0.2f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 1; y < height; y++)
            {
                if (pieces[x, y] != null)
                {
                    int fallDistance = 0;
                    for (int i = y - 1; i >= 0 && pieces[x, i] == null; i--)
                        fallDistance++;

                    if (fallDistance > 0)
                    {
                        FichierPiece piece = pieces[x, y];
                        pieces[x, y - fallDistance] = piece;
                        pieces[x, y] = null;
                        piece.y -= fallDistance;

                        Vector3 newPos = new Vector3(x * tileSize, piece.y * tileSize, 0);
                        StartCoroutine(MovePiece(piece, newPos, 0.15f));
                    }
                }
            }
        }

        yield return new WaitForSeconds(0.3f);
        StartCoroutine(RefillBoard());
    }

    IEnumerator RefillBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (pieces[x, y] == null)
                {
                    SpawnPiece(x, y);
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }

        yield return new WaitForSeconds(0.3f);

        List<FichierPiece> newMatches = GetMatches();
        if (newMatches.Count > 0)
        {
            ClearMatches(newMatches);
        }
    }

    IEnumerator MovePiece(FichierPiece piece, Vector3 target, float duration)
    {
        Vector3 start = piece.transform.position;
        float time = 0f;

        while (time < duration)
        {
            piece.transform.position = Vector3.Lerp(start, target, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        piece.transform.position = target;
    }

    bool HasMatchAt(int x, int y)
    {
        FichierPiece current = pieces[x, y];
        if (current == null) return false;

        // Horizontal check
        if (x >= 2 &&
            pieces[x - 1, y] && pieces[x - 2, y] &&
            pieces[x - 1, y].tag == current.tag &&
            pieces[x - 2, y].tag == current.tag)
            return true;

        // Vertical check
        if (y >= 2 &&
            pieces[x, y - 1] && pieces[x, y - 2] &&
            pieces[x, y - 1].tag == current.tag &&
            pieces[x, y - 2].tag == current.tag)
            return true;

        return false;
    }

    void GameOver(bool victory)
    {
        gameEnded = true;
        boardReady = false;

        if (victory)
        {
            Debug.Log("✅ Victoire dans FichierCrush");

            // Enregistre le mini-jeu courant
            MiniGameManager.Instance?.SetCurrentMiniGame(MiniGameType.FichierCrush);

            // Marque la tâche comme terminée
            ToDoListManager.Instance?.MarkTaskCompletedByName("Classe les Fichier");
            ToDoListManager.Instance?.SaveCompletedTasks();

            // Donne la productivité au joueur
            ProductivityManager.Instance?.AddProductivity(10);

            // Retour au menu
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            Debug.Log("❌ Temps écoulé ! Game Over");
            SceneManager.LoadScene("GameOverScene");
        }
    }



}
