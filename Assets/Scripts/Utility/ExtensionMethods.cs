using System.Collections;
using System.Collections.Generic;
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
}
