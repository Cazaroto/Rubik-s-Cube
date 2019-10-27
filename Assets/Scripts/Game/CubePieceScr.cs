using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePieceScr : MonoBehaviour
{
    //public GameObject upFace, downFace, leftFace, rightFace, frontFace, backFace;
    public List<GameObject> faces = new List<GameObject>();

    #region Public Methods

    public void SetColor(int x, int y, int z)
    {
        if (y == 0)
            faces[0].SetActive(true);
        else if (y == -2)
            faces[1].SetActive(true);

        if (z == 0)
            faces[2].SetActive(true);
        else if (z == 2)
            faces[3].SetActive(true);

        if(x == 0)
            faces[4].SetActive(true);
        else if (x == -2)
            faces[5].SetActive(true);
    }

    #endregion
}
