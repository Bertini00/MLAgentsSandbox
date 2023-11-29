using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    // Public

    public List<PressurePlate> Plates;

    public Door Door;



    // Privates
    private int numberOfPlates;
    private int platesPressed;

    void Start()
    {

        numberOfPlates = Plates.Count;
        platesPressed = 0;
    }

    public void ResetRoom()
    {
        platesPressed = 0;
        for (int i = 0; i < Plates.Count; i++)
        {
            Plates[i].ResetPressurePlate();
        }

        Door.ResetDoor();
    }

    public void PressPlate()
    {
        platesPressed++;
        //Debug.Log("Plate pressed from controller");
        if (platesPressed >= numberOfPlates) 
        {
            Door.OpenDoor();
        }
    }
}
