using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {


    public static float hAxis;
    public static float vAxis;

    private static string h = "Horizontal";
    private static string v = "Vertical";

	private void Update ()
    {
        hAxis = Input.GetAxis(h);
        vAxis = Input.GetAxis(v);
    }

    public static bool PressNextLevel()
    {
        return Input.GetKeyDown(KeyCode.Keypad6);
    }

    public static bool PressPrevLevel()
    {
        return Input.GetKeyDown(KeyCode.Keypad4);
    }

//    public static bool ReleaseArrow()
//    {
//        if (Input.GetKeyUp(KeyCode.RightArrow)) return true;
//        else if (Input.GetKeyUp(KeyCode.LeftArrow)) return true;
//        else return false;
//    }

    public static bool PressSpace()
    {
        return Input.GetButtonDown("Jump");
    }

    public static bool PressUp()
    {
        return Input.GetButtonDown("Interaction");
    }

    public static bool PressLeftCtrl()
    {
        return Input.GetButton("NoTrash");
    }

    public static bool PressRKey()
    {
        return Input.GetButtonDown("Restart");
    }

    public static bool PressPause()
    {
        return Input.GetButtonDown("Pause");
    }
}
