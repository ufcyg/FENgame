using UnityEngine;
using UnityEditor;

public static class DebugMenu
{
    [MenuItem("Debug/Print Global Position")]
    public static void PrintGlobalPosition()
    {
        if (Selection.activeGameObject != null)
        {
            Debug.Log(Selection.activeGameObject.name + " is at " + Selection.activeGameObject.transform.position);
        }
    }
    [MenuItem("Debug/Print Sprite Bounds Min Y")]
    public static void PrintSpriteBoundsMinY()
    {
        if (Selection.activeGameObject != null)
        {
            Debug.Log(Selection.activeGameObject.name + " is at " + Selection.activeGameObject.GetComponent<SpriteRenderer>().bounds.min.y);
        }
    }
}