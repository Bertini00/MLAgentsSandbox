using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.ScrollRect;

public class ActivatableObject : MonoBehaviour
{

    private float pressedYMovementQuantity = 1.2f;
    private float movementTime = 0.5f;


    private Vector3 startingPos = Vector3.zero;
    private void Start()
    {
        startingPos = transform.position;
    }
    public void ResetObject()
    {
        transform.position = startingPos;
    }

    public void Activate()
    {
        StartCoroutine(ActivateCoroutine());
    }

    private IEnumerator ActivateCoroutine()
    {
        float timePassed = 0;
        Vector3 initialPosition = transform.position;
        while (timePassed < movementTime)
        {
            transform.position = Vector3.Lerp(initialPosition, initialPosition + Vector3.down * pressedYMovementQuantity, timePassed / movementTime);
            timePassed += Time.deltaTime;
            yield return null;
        }
    }
}
