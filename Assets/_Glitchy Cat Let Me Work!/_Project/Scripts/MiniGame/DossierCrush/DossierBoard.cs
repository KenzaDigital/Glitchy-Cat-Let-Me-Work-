using UnityEngine;

public class DossierBoard : MonoBehaviour
{
    public int width = 8;
    public int height = 8;
    public float tileSize = 10f;

    public GameObject[] piecePrefabs; 

    private GameObject[,] pieces;

    void Start()
    {
        pieces = new GameObject[width, height];
        InitBoard();
    }

    void InitBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 spawnPos = new Vector2(x * tileSize, y * tileSize);
                GameObject piece = Instantiate(GetRandomPiece(), spawnPos, Quaternion.identity);
                piece.transform.parent = this.transform;
                pieces[x, y] = piece;
            }
        }
    }

    GameObject GetRandomPiece()
    {
        int index = Random.Range(0, piecePrefabs.Length);
        return piecePrefabs[index];
    }
}
