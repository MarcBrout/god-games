using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    void Awake()
    {
        //TODO REMOVE THIS
        //cInput.Clear();
        //----------------

        int joystickPluggedNumber = Input.GetJoystickNames().Length;

        if (joystickPluggedNumber <= 1)
        {
            //TODO: Change to Keyboard language and not System Language
            if (Application.systemLanguage == SystemLanguage.French)
            {
                cInput.SetKey("Up_P1", "Z", "Joy1 Axis 2-");
                cInput.SetKey("Left_P1", "Q", "Joy1 Axis 1-");
                cInput.SetKey("ThrowItem_P1", "A", "Joystick1Button2");
            }
            else
            {
                cInput.SetKey("Up_P1", "W", "Joy1 Axis 2-");
                cInput.SetKey("Left_P1", "A", "Joy1 Axis 1-");
                cInput.SetKey("ThrowItem_P1", "Q", "Joystick1Button2");
            }
            cInput.SetKey("Up_P2", "Joy2 Axis 2-", "UpArrow");
            cInput.SetKey("Down_P1", "S", "Joy1 Axis 2+");
            cInput.SetKey("Down_P2", "Joy2 Axis 2+", "DownArrow");
            cInput.SetKey("Left_P2", "Joy2 Axis 1-", "LeftArrow");
            cInput.SetKey("Right_P1", "D", "Joy1 Axis 1+");
            cInput.SetKey("Right_P2", "Joy2 Axis 1+", "RightArrow");
            cInput.SetKey("Jump_P1", "Space", "Joystick1Button0");
            cInput.SetKey("Jump_P2", "Joystick2Button0", "");
            cInput.SetKey("Dash_P1", "LeftShift", "Joystick1Button1");
            cInput.SetKey("Dash_P2", "Joystick2Button1", "RightShift");
            cInput.SetKey("ThrowItem_P2", "Joystick1Button2", "RightAlt");
            cInput.SetKey("UseItem_P1", "E", "Joystick1Button3");
            cInput.SetKey("UseItem_P2", "Joystick2Button3", "Minus");

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
            if (Application.systemLanguage == SystemLanguage.French)
            {
                cInput.SetKey("Up_P1", "Joy1 Axis 2-", "Z");
                cInput.SetKey("Left_P1", "Joy1 Axis 1-", "Q");
                cInput.SetKey("ThrowItem_P1", "Joystick1Button2", "A");
            }
            else
            {
                cInput.SetKey("Up_P1", "Joy1 Axis 2-", "W");
                cInput.SetKey("Left_P1", "Joy1 Axis 1-", "A");
                cInput.SetKey("ThrowItem_P1", "Joystick1Button2", "Q");
            }
            cInput.SetKey("Up_P2", "Joy2 Axis 2-", "UpArrow");
            cInput.SetKey("Down_P1", "Joy1 Axis 2+", "S");
            cInput.SetKey("Down_P2", "Joy2 Axis 2+", "DownArrow");
            cInput.SetKey("Left_P2", "Joy2 Axis 1-", "LeftArrow");
            cInput.SetKey("Right_P1", "Joy1 Axis 1+", "D");
            cInput.SetKey("Right_P2", "Joy2 Axis 1+", "RightArrow");
            cInput.SetKey("Jump_P1", "Joystick1Button0", "Space");
            cInput.SetKey("Jump_P2", "Joystick2Button0", "");
            cInput.SetKey("Dash_P1", "Joystick1Button1", "LeftShift");
            cInput.SetKey("Dash_P2", "Joystick2Button1", "RightShift");
            cInput.SetKey("ThrowItem_P2", "Joystick1Button2", "RightAlt");
            cInput.SetKey("UseItem_P1", "Joystick1Button3", "E");
            cInput.SetKey("UseItem_P2", "Joystick2Button3", "Minus");

            cInput.SetKey("LookUp_P1", "Joy1 Axis 5-", "Mouse Up");
            cInput.SetKey("LookUp_P2", "Joy2 Axis 5-");
            cInput.SetKey("LookDown_P1", "Joy1 Axis 5+", "Mouse Down");
            cInput.SetKey("LookDown_P2", "Joy2 Axis 5+");
            cInput.SetKey("LookLeft_P1", "Joy1 Axis 4-", "Mouse Left");
            cInput.SetKey("LookLeft_P2", "Joy2 Axis 4-");
            cInput.SetKey("LookRight_P1", "Joy1 Axis 4+", "Mouse Right");
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
