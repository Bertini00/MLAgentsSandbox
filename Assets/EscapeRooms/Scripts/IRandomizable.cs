using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRandomizable
{

    public void Randomize();
    public void Randomize(float minX = -1, float maxX = 1, float minZ = -1, float maxZ = 1);
    public void Randomize(float minX = -1, float maxX = 1, float minY = -1, float maxY = 1, float minZ = -1, float maxZ = 1);

    public void Randomize(Vector3 minValues, Vector3 maxValues);
}
