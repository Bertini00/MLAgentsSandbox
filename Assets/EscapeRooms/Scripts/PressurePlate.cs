using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class PressurePlate: MonoBehaviour
{

    public RoomController RoomController;

    private float pressedYMovementQuantity = 0.2f;
    private float movementTime = 1.5f;

    private bool activated = false;

    private Vector3 startingPosition;

    private void Start()
    {
        startingPosition = transform.position;
    }

    public void ResetPressurePlate()
    {
        if (activated)
        {
            StartCoroutine(plateCoroutine(false));
            activated = false;

        }
    }
    public void ActivatePressurePlate()
    {
        if (!activated)
        {
            //Debug.Log("Attivata pressure plate");
            RoomController.PressPlate();
            activated = true;
            StartCoroutine(plateCoroutine(true));
        }

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
