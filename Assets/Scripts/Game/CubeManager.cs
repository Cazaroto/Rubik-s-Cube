using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Danilo Cazaroto de Oliviera
 * 10-12-2019
 */

public class CubeManager : MonoBehaviour
{
    [Header("Piece Prefab")]
    public GameObject piecePrefab;

    [HideInInspector]
    public List<RotationModel> undoRotations = new List<RotationModel>();

    [HideInInspector]
    public bool canRotate = true;

    [HideInInspector]
    public bool canShuffle = true;

    bool undoing;

    Transform cubeTransform;
    List<GameObject> pieces = new List<GameObject>();
    GameObject centerPiece;

    List<GameObject> UpPieces
    {
        get
        {
            return pieces.FindAll(x => Mathf.Round(x.transform.localPosition.y) == 0);
        }
    }

    List<GameObject> DownPieces
    {
        get
        {
            return pieces.FindAll(x => Mathf.Round(x.transform.localPosition.y) == -2);
        }
    }

    List<GameObject> FrontPieces
    {
        get
        {
            return pieces.FindAll(x => Mathf.Round(x.transform.localPosition.x) == 0);
        }
    }

    List<GameObject> BackPieces
    {
        get
        {
            return pieces.FindAll(x => Mathf.Round(x.transform.localPosition.x) == -2);
        }
    }

    List<GameObject> LeftPieces
    {
        get
        {
            return pieces.FindAll(x => Mathf.Round(x.transform.localPosition.z) == 0);
        }
    }

    List<GameObject> RightPieces
    {
        get
        {
            return pieces.FindAll(x => Mathf.Round(x.transform.localPosition.z) == 2);
        }
    }

    List<GameObject> UpHorizontalPieces
    {
        get
        {
            return pieces.FindAll(x => Mathf.Round(x.transform.localPosition.x) == -1);
        }
    }

    List<GameObject> UpVerticalPieces
    {
        get
        {
            return pieces.FindAll(x => Mathf.Round(x.transform.localPosition.z) == 1);
        }
    }

    List<GameObject> FrontHorizontalPieces
    {
        get
        {
            return pieces.FindAll(x => Mathf.Round(x.transform.localPosition.y) == -1);
        }
    }


    Vector3[] rotationVectors =
    {
        new Vector3(0,1,0), new Vector3(0,-1,0),
        new Vector3(0,0,-1), new Vector3(0,0,1),
        new Vector3(1,0,0), new Vector3(1,0,0)
    };

    // Start is called before the first frame update
    void Start()
    {
        cubeTransform = transform;
        CubeCreate();
    }

    // Update is called once per frame
    void Update()
    {
        if (canRotate)
            CheckInput();
    }

    #region Private Methods

    void CubeCreate()
    {
        //Destroys all pieces of the cube before creation.
        foreach (GameObject piece in pieces)
        {
            DestroyImmediate(piece);
        }

        pieces.Clear();

        //Dinamicly creates de Cube.
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int z = 0; z < 3; z++)
                {
                    GameObject piece = Instantiate(piecePrefab, cubeTransform, false);
                    piece.transform.localPosition = new Vector3(-x, -y, z);
                    piece.GetComponent<CubePieceScr>().SetColor(-x, -y, z);
                    pieces.Add(piece);
                }
            }
        }

        centerPiece = pieces[13];

        //Shuffle the Cube;
        StartCoroutine(Shuffle());
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
            StartCoroutine(RotateVector(UpPieces, new Vector3(0, 1, 0)));
        else if (Input.GetKeyDown(KeyCode.S))
            StartCoroutine(RotateVector(DownPieces, new Vector3(0, -1, 0)));
        else if (Input.GetKeyDown(KeyCode.A))
            StartCoroutine(RotateVector(LeftPieces, new Vector3(0, 0, -1)));
        else if (Input.GetKeyDown(KeyCode.D))
            StartCoroutine(RotateVector(RightPieces, new Vector3(0, 0, 1)));
        else if (Input.GetKeyDown(KeyCode.Q))
            StartCoroutine(RotateVector(FrontPieces, new Vector3(1, 0, 0)));
        else if (Input.GetKeyDown(KeyCode.E))
            StartCoroutine(RotateVector(BackPieces, new Vector3(-1, 0, 0)));
        else if (Input.GetKeyDown(KeyCode.F) && canShuffle)
            StartCoroutine(Shuffle());
        else if (Input.GetKeyDown(KeyCode.C) && canShuffle)
            CubeCreate();

    }

    //Auto explainable
    void CheckIsComplete()
    {
        if (IsSideComplete(UpPieces) &&
           IsSideComplete(DownPieces) &&
           IsSideComplete(FrontPieces) &&
           IsSideComplete(BackPieces) &&
           IsSideComplete(LeftPieces) &&
           IsSideComplete(RightPieces))
        {
            Debug.Log("Is complete.");
        }
    }

    bool DetectSide(List<GameObject> planes, Vector3 fDirection, Vector3 sDirection, List<GameObject> side)
    {
        GameObject centerPiece = side.Find(x => x.GetComponent<CubePieceScr>().faces.FindAll(y => y.activeInHierarchy).Count == 1);
        List<RaycastHit> hit1 = new List<RaycastHit>(Physics.RaycastAll(planes[1].transform.position, fDirection)),
                         hit2 = new List<RaycastHit>(Physics.RaycastAll(planes[0].transform.position, fDirection)),
                         hit1_ = new List<RaycastHit>(Physics.RaycastAll(planes[1].transform.position, -fDirection)),
                         hit2_ = new List<RaycastHit>(Physics.RaycastAll(planes[0].transform.position, -fDirection)),

                         hit3 = new List<RaycastHit>(Physics.RaycastAll(planes[1].transform.position, sDirection)),
                         hit4 = new List<RaycastHit>(Physics.RaycastAll(planes[0].transform.position, sDirection)),
                         hit3_ = new List<RaycastHit>(Physics.RaycastAll(planes[1].transform.position, -sDirection)),
                         hit4_ = new List<RaycastHit>(Physics.RaycastAll(planes[0].transform.position, -sDirection));

        return hit1.Exists(x => x.collider.gameObject == centerPiece) ||
               hit2.Exists(x => x.collider.gameObject == centerPiece) ||
               hit1_.Exists(x => x.collider.gameObject == centerPiece) ||
               hit2_.Exists(x => x.collider.gameObject == centerPiece) ||

               hit3.Exists(x => x.collider.gameObject == centerPiece) ||
               hit4.Exists(x => x.collider.gameObject == centerPiece) ||
               hit3_.Exists(x => x.collider.gameObject == centerPiece) ||
               hit4_.Exists(x => x.collider.gameObject == centerPiece);
    }

    float DetectLeftMiddleRightSign(List<GameObject> pieces)
    {
        float sign = 0;

        if (Mathf.Round(pieces[1].transform.position.y) != Mathf.Round(pieces[0].transform.position.y))
        {
            if (Mathf.Round(pieces[0].transform.position.x) == -2)
                sign = Mathf.Round(pieces[0].transform.position.y) - Mathf.Round(pieces[1].transform.position.y);
            else
                sign = Mathf.Round(pieces[1].transform.position.y) - Mathf.Round(pieces[0].transform.position.y);
        }
        else
        {
            if (Mathf.Round(pieces[0].transform.position.y) == -2)
                sign = Mathf.Round(pieces[0].transform.position.x) - Mathf.Round(pieces[1].transform.position.x);
            else
                sign = Mathf.Round(pieces[1].transform.position.x) - Mathf.Round(pieces[0].transform.position.x);
        }

        return sign;
    }

    float DetectFrontMiddleBackSign(List<GameObject> pieces)
    {
        float sign = 0;

        if (Mathf.Round(pieces[1].transform.position.z) != Mathf.Round(pieces[0].transform.position.z))
        {
            if (Mathf.Round(pieces[0].transform.position.y) == 0)
                sign = Mathf.Round(pieces[1].transform.position.z) - Mathf.Round(pieces[0].transform.position.z);
            else
                sign = Mathf.Round(pieces[0].transform.position.z) - Mathf.Round(pieces[1].transform.position.z);
        }
        else
        {
            if (Mathf.Round(pieces[0].transform.position.z) == 0)
                sign = Mathf.Round(pieces[1].transform.position.y) - Mathf.Round(pieces[0].transform.position.y);
            else
                sign = Mathf.Round(pieces[0].transform.position.y) - Mathf.Round(pieces[1].transform.position.y);
        }

        return sign;
    }

    float DetectUpMiddleDownSign(List<GameObject> pieces)
    {
        float sign = 0;

        if (Mathf.Round(pieces[1].transform.position.z) != Mathf.Round(pieces[0].transform.position.z))
        {
            if (Mathf.Round(pieces[0].transform.position.x) == 0)
                sign = Mathf.Round(pieces[1].transform.position.z) - Mathf.Round(pieces[0].transform.position.z);
            else
                sign = Mathf.Round(pieces[0].transform.position.z) - Mathf.Round(pieces[1].transform.position.z);
        }
        else
        {
            if (Mathf.Round(pieces[0].transform.position.z) == 0)
                sign = Mathf.Round(pieces[0].transform.position.x) - Mathf.Round(pieces[1].transform.position.x);
            else
                sign = Mathf.Round(pieces[1].transform.position.x) - Mathf.Round(pieces[0].transform.position.x);
        }

        return sign;
    }

    bool IsSideComplete(List<GameObject> pieces)
    {
        int mainPlaneIndex = pieces[4].GetComponent<CubePieceScr>().faces.FindIndex(x => x.activeInHierarchy);

        for (int i = 0; i < pieces.Count; i++)
        {
            if (!pieces[i].GetComponent<CubePieceScr>().faces[mainPlaneIndex].activeInHierarchy ||
                pieces[i].GetComponent<CubePieceScr>().faces[mainPlaneIndex].GetComponent<Renderer>().material.color !=
                pieces[4].GetComponent<CubePieceScr>().faces[mainPlaneIndex].GetComponent<Renderer>().material.color)
            {
                return false;
            }
        }

        return true;
    }

    IEnumerator Shuffle()
    {
        canShuffle = false;

        yield return new WaitForSeconds(.1f);

        for (int moves = Random.Range(15, 30); moves >= 0; moves--)
        {
            int edge = Random.Range(0, 6);
            List<GameObject> edgePieces = new List<GameObject>();

            switch (edge)
            {
                case 0: edgePieces = UpPieces; break;
                case 1: edgePieces = DownPieces; break;
                case 2: edgePieces = LeftPieces; break;
                case 3: edgePieces = RightPieces; break;
                case 4: edgePieces = FrontPieces; break;
                case 5: edgePieces = BackPieces; break;
                default: Debug.LogWarning("Shuffle - Case not satisfied."); break;
            }

            StartCoroutine(RotateVector(edgePieces, rotationVectors[edge], 15));
            yield return new WaitForSeconds(.3f);

            //Debug.Log("Vector Negative: " + (rotationVectors * -1).ToString());
        }

        canShuffle = true;
    }

    IEnumerator RotateVector(List<GameObject> pieces, Vector3 rotationVec, int speed = 5)
    {
        canRotate = false;
        int angle = 0;

        while (angle < 90)
        {
            foreach (GameObject piece in pieces)
            {
                piece.transform.RotateAround(centerPiece.transform.position, rotationVec, speed);
            }

            angle += speed;
            yield return null;
        }

        //If the game is not undoing or in shuffle moves, add this move to the undo list.
        if (canShuffle)
        {
            if (!undoing)
            {
                //Add to list of undos the Pieces and Opposite Rotation.
                undoRotations.Add(new RotationModel(pieces, rotationVec * -1));
                
            }
        }

        
        CheckIsComplete();

        undoing = false;
        canRotate = true;
    }

    #endregion

    #region Public Methods

    public void DetectRotation(List<GameObject> pieces, List<GameObject> planes)
    {
        if (!canRotate || !canShuffle)
            return;

        if (UpVerticalPieces.Exists(x => x == pieces[0]) &&
            UpVerticalPieces.Exists(x => x == pieces[1]))
            StartCoroutine(RotateVector(UpVerticalPieces, new Vector3(0, 0, 1 * DetectLeftMiddleRightSign(pieces))));

        else if (UpHorizontalPieces.Exists(x => x == pieces[0]) &&
            UpHorizontalPieces.Exists(x => x == pieces[1]))
            StartCoroutine(RotateVector(UpHorizontalPieces, new Vector3(1 * DetectLeftMiddleRightSign(pieces), 0, 0)));

        else if (FrontHorizontalPieces.Exists(x => x == pieces[0]) &&
            FrontHorizontalPieces.Exists(x => x == pieces[1]))
            StartCoroutine(RotateVector(FrontHorizontalPieces, new Vector3(0, 1 * DetectUpMiddleDownSign(pieces), 0)));

        else if (DetectSide(planes, new Vector3(1, 0, 0), new Vector3(0, 0, 1), UpPieces))
            StartCoroutine(RotateVector(UpPieces, new Vector3(0, 1 * DetectUpMiddleDownSign(pieces), 0)));

        else if (DetectSide(planes, new Vector3(1, 0, 0), new Vector3(0, 0, 1), DownPieces))
            StartCoroutine(RotateVector(DownPieces, new Vector3(0, 1 * DetectUpMiddleDownSign(pieces), 0)));

        else if (DetectSide(planes, new Vector3(0, 0, 1), new Vector3(0, 1, 0), FrontPieces))
            StartCoroutine(RotateVector(FrontPieces, new Vector3(1 * DetectFrontMiddleBackSign(pieces), 0, 0)));

        else if (DetectSide(planes, new Vector3(0, 0, 1), new Vector3(0, 1, 0), BackPieces))
            StartCoroutine(RotateVector(BackPieces, new Vector3(1 * DetectFrontMiddleBackSign(pieces), 0, 0)));

        else if (DetectSide(planes, new Vector3(1, 0, 0), new Vector3(0, 1, 0), LeftPieces))
            StartCoroutine(RotateVector(LeftPieces, new Vector3(0, 0, 1 * DetectLeftMiddleRightSign(pieces))));

        else if (DetectSide(planes, new Vector3(1, 0, 0), new Vector3(0, 1, 0), RightPieces))
            StartCoroutine(RotateVector(RightPieces, new Vector3(0, 0, 1 * DetectLeftMiddleRightSign(pieces))));
    }

    public void UndoRotation()
    {
        if(undoRotations.Count > 0)
        {
            RotationModel rotationModel;

            undoing = true;

            rotationModel = undoRotations[undoRotations.Count - 1];
            StartCoroutine(RotateVector(rotationModel.pieces, rotationModel.rotation));

            undoRotations.RemoveAt(undoRotations.Count - 1);
        }
    }

    #endregion
}
