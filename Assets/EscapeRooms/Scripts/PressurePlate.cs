using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class PressurePlate: MonoBehaviour
{

    public RoomController RoomController;

    private float pressedYMovementQuantity = 1.2f;
    private float movementTime = 0.5f;

    private bool activated = false;

    private Vector3 startingPosition;

    private bool canRandomize;

    private void Start()
    {
        canRandomize = RoomController.canRandomize;
        startingPosition = transform.position;
    }

    public void ResetPressurePlate()
    {
        transform.position = startingPosition;
        activated = false;

        ActivatableObject actObj = GetComponentInChildren<ActivatableObject>();

        if (actObj)
        {
            actObj.ResetObject();
        }

    }
    public bool ActivatePressurePlate()
    {
        if (!activated)
        {
            //Debug.Log("Attivata pressure plate");
            activated = true;
            RoomController.PressPlate();
            ActivatableObject actObj = GetComponentInChildren<ActivatableObject>();

            if (actObj)
            {
                actObj.Activate();
            }
            //StartCoroutine(plateCoroutine(true));
            return true;
        }
        return false;

    }

    private IEnumerator plateCoroutine(bool pressed)
    {
        float timePassed = 0;
        Vector3 initialPosition = transform.position;
        int op = pressed ? 1 : -1;
        while (timePassed < movementTime)
        {
            transform.position = Vector3.Lerp(initialPosition, initialPosition + Vector3.down * op * pressedYMovementQuantity, timePassed / movementTime);
            timePassed += Time.deltaTime;
            yield return null;
        }
    }

    public void Randomize()
    {
        Randomize(-1, 1, -1, 1);
    }

    public void Randomize(float minX = -1, float maxX = 1, float minZ = -1, float maxZ = 1)
    {
        gameObject.transform.position += new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ));
    }

    public void Randomize(float minX = -1, float maxX = 1, float minY = -1, float maxY = 1, float minZ = -1, float maxZ = 1)
    {
        gameObject.transform.position += new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
    }

    public void Randomize(Vector3 minValues, Vector3 maxValues)
    {
        gameObject.transform.position += new Vector3(Random.Range(minValues.x, maxValues.x),
            Random.Range(minValues.y, maxValues.y),
            Random.Range(minValues.z, maxValues.z));
    }
}
