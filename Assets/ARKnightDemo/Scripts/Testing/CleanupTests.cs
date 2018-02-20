#if UNITY_EDITOR
using UnityEngine;

public class CleanupTests
{
    /// <summary>
    /// Cleans up any game objects left over in the scene
    /// </summary>
    public static void Cleanup()
    {
        GameObject[] gameObjects = Object.FindObjectsOfType<GameObject>();
        for (int i = 0; i < gameObjects.Length; i++)
        {
            GameObject go = gameObjects[i];
            Component masterTestAsset = go.GetComponent("UnityEngine.TestTools.TestRunner.PlaymodeTestsController");
            if (masterTestAsset == null)
            {
                Object.DestroyImmediate(go);
            }
        }
    }
}
#endif