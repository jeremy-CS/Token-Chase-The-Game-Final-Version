//Code made with help from URL: https://github.com/SebLague/Boids/blob/master/Assets/Scripts/BoidHelper.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHelper
{
    const int numViewDirections = 300;
    public static readonly Vector3[] directions;

    static CollisionHelper()
    {
        directions = new Vector3[CollisionHelper.numViewDirections];

        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        float angleIncrement = Mathf.PI * 2 * goldenRatio;

        for (int i = 0; i < numViewDirections; i++)
        {
            float t = (float)i / numViewDirections;
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimuth = angleIncrement * i;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = 0f; //Mathf.Sin(inclination) * Mathf.Sin(azimuth); //Use the commented part for 3D movement (in y axis as well)
            float z = Mathf.Cos(inclination);
            directions[i] = new Vector3(x, y, z);
        }
    }
}
