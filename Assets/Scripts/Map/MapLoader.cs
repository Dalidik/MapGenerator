using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    [SerializeField] private List<GameObject> _availablePrefabs;
    [SerializeField] private Transform _objectsParent;

    void Start()
    {
        if (SceneParameters.LoadSavedMap)
        {
            LoadSavedMap();
        }
        else
        {
            Debug.Log("An empty map is loaded");
        }
    }

    private void LoadSavedMap()
    {
        string filePath = Application.persistentDataPath + "/" + SceneParameters.SavePath;

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            MapData mapData = JsonUtility.FromJson<MapData>(json);
            if (mapData != null)
            {
                Debug.Log($"The texture index has been loaded: {mapData.selectedTextureIndex}");
                
                foreach (ObjectData data in mapData.objects)
                {
                    GameObject prefab = _availablePrefabs.Find(p => p.name == data.prefabName);
                    if (prefab != null)
                    {
                        GameObject obj = Instantiate(prefab, data.position, data.rotation, _objectsParent);
                       // obj.AddComponent<ObjectMover>();
                        Debug.Log("Loaded object: " + prefab.name);
                    }
                    else
                    {
                        Debug.LogWarning("Prefab not found: " + data.prefabName);
                    }
                }

                Debug.Log($"Texture {mapData.selectedTextureIndex} loaded.");
            }
            else
            {
                Debug.LogError("Error loading card data: mapData is empty!");
            }
        }
        else
        {
            Debug.LogError("Saved map file not found: " + filePath);
        }
    }
}


