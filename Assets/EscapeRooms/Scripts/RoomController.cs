using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    // Public

    public List<PressurePlate> Plates;

    public Door Door;

    public EscapeRoomAgent agent;

    public bool canRandomize = false;



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
            if (canRandomize)
            {
                Plates[i].Randomize();
            }
        }

        Door.ResetDoor();

        if (numberOfPlates == 0)
        {
            Door.OpenDoor();
        }

        if (canRandomize)
        {
            agent.Randomize();
        }
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
