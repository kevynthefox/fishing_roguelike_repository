using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
//using UnityEngine.WSA;

public class Item_icon_screenshot_taker : MonoBehaviour
{
    public new Camera camera;
    public string Prefix;
    public string pathFolder;

    public List<GameObject> sceneObjects;
    public List<InventoryItemData> dataObjects;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    [ContextMenu("Screenshot")]
    private void ProcessScreenshots()
    {
        StartCoroutine(Screenshot());
    }

    public IEnumerator Screenshot()
    {
        for (int i = 0; i < sceneObjects.Count; i++)
        {
            GameObject obj = sceneObjects[i];
            InventoryItemData data = dataObjects[i];

            obj.gameObject.SetActive(true);

            yield return null;

            TakeScreenshot($"{Application.dataPath}/{pathFolder}/{data.id}_Icon.png");

            yield return null;
            obj.gameObject.SetActive(false);

            Sprite s = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/{pathFolder}/{data.id}_icon.png");
            if ( s != null )
            {
                data.icon = s;
                EditorUtility.SetDirty(data);
            }

            yield return null;
        }
    }    

    void TakeScreenshot(string fullPath)
    {
        if (camera == null)
        {
            camera = GetComponent<Camera>();
        }

        RenderTexture rt = new RenderTexture(256, 256, 24);
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(256, 256, TextureFormat.RGBA32, false);
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
        camera.targetTexture = null;
        RenderTexture.active = null;

        if (Application.isEditor)
        {
            DestroyImmediate(rt);
        }
        else
        {
            Destroy(rt);
        }

        byte[] bytes = screenShot.EncodeToPNG();
        System.IO.File.WriteAllBytes(fullPath, bytes);
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
}
