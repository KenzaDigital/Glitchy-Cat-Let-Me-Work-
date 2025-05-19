using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DossierBoard : MonoBehaviour
{
    public int width = 8;
    public int height = 8;
    public float tileSize = 2f;

    public GameObject[] piecePrefabs;

    private FichierPiece[,] pieces;
    private FichierPiece selectedPiece;

    void Start()
    {
        pieces = new FichierPiece[width, height];
        InitBoard();
        StartCoroutine(CheckMatchesAfterInit());
    }

    void InitBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                SpawnPiece(x, y);
            }
        }
    }

    void SpawnPiece(int x, int y)
    {
        Vector2 spawnPos = new Vector2(x * tileSize, y * tileSize);
        GameObject obj = Instantiate(GetRandomPiece(), spawnPos, Quaternion.identity);
        obj.transform.parent = transform;

        FichierPiece piece = obj.GetComponent<FichierPiece>();
        piece.Init(x, y, this);
        pieces[x, y] = piece;
    }

    GameObject GetRandomPiece()
    {
        int index = Random.Range(0, piecePrefabs.Length);
        return piecePrefabs[index];
    }

    public void SelectPiece(FichierPiece piece)
    {
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
            else
            {
                selectedPiece = piece;
            }
        }
    }

    bool AreAdjacent(FichierPiece a, FichierPiece b)
    {
        return (Mathf.Abs(a.x - b.x) == 1 && a.y == b.y) ||
               (Mathf.Abs(a.y - b.y) == 1 && a.x == b.x);
    }

    IEnumerator SwapAndCheck(FichierPiece a, FichierPiece b)
    {
        SwapPieces(a, b);

        yield return new WaitForSeconds(0.2f);

        if (GetMatches().Count > 0)
        {
            ClearMatches();
        }
        else
        {
            // Pas de match, on annule le swap
            SwapPieces(a, b);
        }

        selectedPiece = null;
    }

    void SwapPieces(FichierPiece a, FichierPiece b)
    {
        // Swap dans la grille
        pieces[a.x, a.y] = b;
        pieces[b.x, b.y] = a;

        // Swap les coordonnées
        int tempX = a.x;
        int tempY = a.y;
        a.x = b.x;
        a.y = b.y;
        b.x = tempX;
        b.y = tempY;

        // Déplace visuellement
        Vector3 tempPos = a.transform.position;
        a.transform.position = b.transform.position;
        b.transform.position = tempPos;
    }

    List<FichierPiece> GetMatches()
    {
        List<FichierPiece> matches = new List<FichierPiece>();

        // Horizontaux
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width - 2; x++)
            {
                FichierPiece p1 = pieces[x, y];
                FichierPiece p2 = pieces[x + 1, y];
                FichierPiece p3 = pieces[x + 2, y];

                if (p1 != null && p2 != null && p3 != null &&
                    p1.tag == p2.tag && p2.tag == p3.tag)
                {
                    if (!matches.Contains(p1)) matches.Add(p1);
                    if (!matches.Contains(p2)) matches.Add(p2);
                    if (!matches.Contains(p3)) matches.Add(p3);
                }
            }
        }

        // Verticaux
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height - 2; y++)
            {
                FichierPiece p1 = pieces[x, y];
                FichierPiece p2 = pieces[x, y + 1];
                FichierPiece p3 = pieces[x, y + 2];

                if (p1 != null && p2 != null && p3 != null &&
                    p1.tag == p2.tag && p2.tag == p3.tag)
                {
                    if (!matches.Contains(p1)) matches.Add(p1);
                    if (!matches.Contains(p2)) matches.Add(p2);
                    if (!matches.Contains(p3)) matches.Add(p3);
                }
            }
        }

        return matches;
    }

    void ClearMatches()
    {
        List<FichierPiece> matches = GetMatches();

        foreach (FichierPiece piece in matches)
        {
            pieces[piece.x, piece.y] = null;
            Destroy(piece.gameObject);
        }

        if (matches.Count > 0)
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
                    int fall = 0;

                    for (int i = y - 1; i >= 0 && pieces[x, i] == null; i--)
                    {
                        fall++;
                    }

                    if (fall > 0)
                    {
                        FichierPiece piece = pieces[x, y];
                        pieces[x, y] = null;
                        pieces[x, y - fall] = piece;
                        piece.y -= fall;

                        Vector3 targetPos = new Vector3(x * tileSize, (y - fall) * tileSize, 0);
                        StartCoroutine(MovePiece(piece, targetPos, 0.15f));
                    }
                }
            }
        }

        yield return new WaitForSeconds(0.3f);

        yield return StartCoroutine(RefillBoard());
        yield return new WaitForSeconds(0.2f);

        ClearMatches();
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

    IEnumerator CheckMatchesAfterInit()
    {
        yield return new WaitForSeconds(0.5f);
        ClearMatches();
    }
}
