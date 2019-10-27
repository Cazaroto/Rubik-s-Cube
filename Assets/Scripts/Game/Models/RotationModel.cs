using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RotationModel
{
    public List<GameObject> pieces;
    public Vector3 rotation;

    public RotationModel(List<GameObject> pieces, Vector3 rotation)
    {
        this.pieces = pieces;
        this.rotation = rotation;
    }
}
