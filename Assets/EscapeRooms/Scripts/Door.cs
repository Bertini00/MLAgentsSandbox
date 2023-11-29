using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.ScrollRect;

public class Door : MonoBehaviour
{

    private float movementQuantity = 1f;
    private float movementDuration = 1.5f;

    private bool opened = false;
    private Vector3 startingPosition;

    private void Start()
    {
        startingPosition = transform.position;
    }


    [ContextMenu("Close")]
    public void ResetDoor()
    {
        //transform.position = startingPosition;

        if (opened) 
        { 
            StartCoroutine(OpenDoorCoroutine(false));
            opened = false;
        }
    }

    [ContextMenu("Open")]
    public void OpenDoor()
    {
        if(!opened)
        {
            opened = true;
            Debug.Log("Starting coroutine for door");
            StartCoroutine(OpenDoorCoroutine(true));
        }
    }

    private IEnumerator OpenDoorCoroutine(bool opening)
    {
        float timePassed = 0;
        Vector3 initialPosition = transform.position;
        int op = opening ? 1 : -1;
        while (timePassed < movementDuration)
        {
            transform.position = Vector3.Lerp(initialPosition, initialPosition + Vector3.up * op * movementQuantity, timePassed / movementDuration);
            timePassed += Time.deltaTime;
            yield return null;
        }

    }
}
