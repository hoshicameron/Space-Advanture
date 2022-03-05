using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperMethods
{
    public static Vector2 GetScreenBounds(Camera camera)
    {
       return camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, camera.transform.position.z));
    }

    public static void  GetObjectHeightAndWidth(SpriteRenderer renderer,out float width,out float height)
    {
        width= renderer.bounds.size.x * 0.5f;
        height = renderer.bounds.size.y * 0.5f;
    }
}
