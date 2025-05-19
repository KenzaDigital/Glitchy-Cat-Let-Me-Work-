using UnityEngine;

public class Dossier : MonoBehaviour
{
    public int xIndex;
    public int yIndex;
    public bool isMatched;


    public void SetPosition(int x, int y)
    {
        xIndex = x;
        yIndex = y;
    }
}
// cette class represent les dossiers crush du jeux , il à une propriété pour sa position (xIndex et yIndex) et un boolean pour savoir si il est matcher