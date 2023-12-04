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


        if (canRandomize)
        {
            gameObject.transform.position += new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        }
    }
    public bool ActivatePressurePlate()
    {
        if (!activated)
        {
            //Debug.Log("Attivata pressure plate");
            activated = true;
            RoomController.PressPlate();
            StartCoroutine(plateCoroutine(true));
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
}
