using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;

public static class ExtensionMethods
{
    private static readonly Vector3 _NorthEstVector3 = new Vector3(1, 0, 1);
    private static readonly Vector3 _NorthWestVector3 = new Vector3(-1, 0, 1);
    private static readonly Vector3 _SouthEstVector3 = new Vector3(1, 0, -1);
    private static readonly Vector3 _SouthWestVector3 = new Vector3(-1, 0, -1);

    public static Vector3 NorthEstVector3 { get { return _NorthEstVector3; } }
    public static Vector3 NorthWestVector3 { get { return _NorthWestVector3; } }
    public static Vector3 SouthEstVector3 { get { return _SouthEstVector3; } }
    public static Vector3 SouthWestVector3 { get { return _SouthWestVector3; } }


    public static void ResetTransformation(this Transform trans)
    {
        trans.position = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = new Vector3(1, 1, 1);
    }

    public static bool Contain(this LayerMask layermask, int layer)
    {
        return layermask == (layermask | (1 << layer));
    }

    public static float LerpOverTime(float start, float end, float startTime, float lerpDuration = 1)
    {
        float timeSinceStart = Time.time - startTime;
        float completePercentage = timeSinceStart / lerpDuration;
        float res = Mathf.Lerp(start, end, completePercentage);
        return res;
    }

    public static object GetPropValue(this object obj, string name)
    {
        foreach (string part in name.Split('.'))
        {
            if (obj == null) { return null; }

            Type type = obj.GetType();
            PropertyInfo info = type.GetProperty(part);
            if (info == null) { return null; }

            obj = info.GetValue(obj, null);
        }
        return obj;
    }

    public static T GetPropValue<T>(this object obj, string name)
    {
        object retval = GetPropValue(obj, name);
        if (retval == null) { return default(T); }

        // throws InvalidCastException if types are incompatible
        return (T)retval;
    }
}
