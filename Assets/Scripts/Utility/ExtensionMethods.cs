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

    public static List<Transform> GetChildren(this Transform transform)
    {
        List<Transform> children = new List<Transform>();
        for (int i = 0; i < transform.childCount; ++i)
            children.Add(transform.GetChild(i));
        return children;
    }

    public static void DelayAction(this MonoBehaviour monoBehaviour, float delay, Action action)
    {
        monoBehaviour.StartCoroutine(DelayEnumerator(delay, action));
    }

    private static IEnumerator DelayEnumerator(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        if (action != null)
            action();
    }

    public static IEnumerator MoveOverSeconds(this Transform objectToMove, Vector3 destination, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.position;
        while (elapsedTime < seconds)
        {
            objectToMove.position = Vector3.Lerp(startingPos, destination, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.position = destination;
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

    public static bool ApproximatelyByValue(this Vector3 me, Vector3 other, float allowedDifference)
    {
        var dx = me.x - other.x;
        if (Mathf.Abs(dx) > allowedDifference)
            return false;

        var dy = me.y - other.y;
        if (Mathf.Abs(dy) > allowedDifference)
            return false;

        var dz = me.z - other.z;

        return Mathf.Abs(dz) >= allowedDifference;
    }

    public static bool ApproximatelyByPercentage(this Vector3 me, Vector3 other, float percentage)
    {
        var dx = me.x - other.x;
        if (Mathf.Abs(dx) > me.x * percentage)
            return false;

        var dy = me.y - other.y;
        if (Mathf.Abs(dy) > me.y * percentage)
            return false;

        var dz = me.z - other.z;

        return Mathf.Abs(dz) >= me.z * percentage;
    }
}

public static class StringExt
{
    public static string Truncate(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) { return value; }
        return value.Substring(0, Math.Min(value.Length, maxLength));
    }

    public static TEnum ToEnum<TEnum>(this string strEnumValue, TEnum defaultValue)
    {
        if (!Enum.IsDefined(typeof(TEnum), strEnumValue))
            return defaultValue;

        return (TEnum)Enum.Parse(typeof(TEnum), strEnumValue);
    }
}

public static class List
{
    public static void AddIfNotNull<T>(this List<T> list, T item) where T : class
    {
        if (item != null)
            list.Add(item);
    }
}
