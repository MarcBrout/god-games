using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    void Awake()
    {
        //TODO REMOVE THIS
        cInput.Clear();
        //----------------

        int joystickPluggedNumber = Input.GetJoystickNames().Length;

        if (joystickPluggedNumber <= 1)
        {
            cInput.SetKey("Up_P1", "Z");
            cInput.SetKey("Up_P2", "Joy1 Axis 2-");
            cInput.SetKey("Down_P1", "S");
            cInput.SetKey("Down_P2", "Joy1 Axis 2+");
            cInput.SetKey("Left_P1", "Q");
            cInput.SetKey("Left_P2", "Joy1 Axis 1-");
            cInput.SetKey("Right_P1", "D");
            cInput.SetKey("Right_P2", "Joy1 Axis 1+");
            cInput.SetKey("Jump_P1", "Space");
            cInput.SetKey("Jump_P2", "Joystick1Button0");
            cInput.SetKey("Dash_P1", "LeftShift");
            cInput.SetKey("Dash_P2", "Joystick1Button1");

            cInput.SetKey("LookUp_P1", "Mouse Up");
            cInput.SetKey("LookUp_P2", "Joy1 Axis 5-");
            cInput.SetKey("LookDown_P1", "Mouse Down");
            cInput.SetKey("LookDown_P2", "Joy1 Axis 5+");
            cInput.SetKey("LookLeft_P1", "Mouse Left");
            cInput.SetKey("LookLeft_P2", "Joy1 Axis 4-");
            cInput.SetKey("LookRight_P1", "Mouse Right");
            cInput.SetKey("LookRight_P2", "Joy1 Axis 4+");

        }
        else if (joystickPluggedNumber > 1)
        {
            cInput.SetKey("Up_P1", "Joy1 Axis 2-");
            cInput.SetKey("Up_P2", "Joy2 Axis 2-");
            cInput.SetKey("Down_P1", "Joy1 Axis 2+");
            cInput.SetKey("Down_P2", "Joy2 Axis 2+");
            cInput.SetKey("Left_P1", "Joy1 Axis 1- ");
            cInput.SetKey("Left_P2", "Joy2 Axis 1-");
            cInput.SetKey("Right_P1", "Joy1 Axis 1+");
            cInput.SetKey("Right_P2", "Joy2 Axis 1+");
            cInput.SetKey("Jump_P1", "Joystick1Button0");
            cInput.SetKey("Jump_P2", "Joystick2Button0");
            cInput.SetKey("Dash_P1", "Joystick1Button1");
            cInput.SetKey("Dash_P2", "Joystick2Button1");

            cInput.SetKey("LookUp_P1", "Joy1 Axis 5-");
            cInput.SetKey("LookUp_P2", "Joy2 Axis 5-");
            cInput.SetKey("LookDown_P1", "Joy1 Axis 5+");
            cInput.SetKey("LookDown_P2", "Joy2 Axis 5+");
            cInput.SetKey("LookLeft_P1", "Joy1 Axis 4-");
            cInput.SetKey("LookLeft_P2", "Joy2 Axis 4-");
            cInput.SetKey("LookRight_P1", "Joy1 Axis 4+");
            cInput.SetKey("LookRight_P2", "Joy2 Axis 4+");
        }

        cInput.SetAxis("Horizontal_P1", "Left_P1", "Right_P1");
        cInput.SetAxis("Horizontal_P2", "Left_P2", "Right_P2");
        cInput.SetAxis("Vertical_P1", "Down_P1", "Up_P1");
        cInput.SetAxis("Vertical_P2", "Down_P2", "Up_P2");
        cInput.SetAxis("RHorizontal_P1", "LookLeft_P1", "LookRight_P1");
        cInput.SetAxis("RHorizontal_P2", "LookLeft_P2", "LookRight_P2");
        cInput.SetAxis("RVertical_P1", "LookDown_P1", "LookUp_P1");
        cInput.SetAxis("RVertical_P2", "LookDown_P2", "LookUp_P2");
    }
}
