using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public static class SceneParameters
{
    public static bool LoadSavedMap = false; 
    public static string SavePath = "/SavedMap.json";
}

public class MapEditor : MonoBehaviour
{
    [Header("Objects to place")]
    [SerializeField] private List<GameObject> _objectsToPlace;
    
    [Header("Restricted Touch Zones")]
    [SerializeField] private List<RectTransform> _restrictedZones;

    [Header("UI elements")]
    [SerializeField] private Transform _scrollViewContent;
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private Transform _objectsParent;
    [SerializeField] private Button _saveButton;
    
    private GameObject _selectedObject = null;

    private void OnEnable()
    {
        _saveButton.onClick.AddListener(SaveMap);
    }

    void Start()
    {
        PopulateScrollView();
    }

    void Update()
    {
        if (Input.touchCount > 0 && _selectedObject != null)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (IsTouchInRestrictedZone(touch.position))
                {
                    return; 
                }

                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    GameObject placedObject = Instantiate(_selectedObject, hit.point, Quaternion.identity);
                    placedObject.transform.parent = _objectsParent;
                    placedObject.AddComponent<ObjectMover>();
                }
            }
            
            RotateObject();
        }
    }
    
    private bool IsTouchInRestrictedZone(Vector2 touchPosition)
    {
        foreach (RectTransform zone in _restrictedZones)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(zone, touchPosition))
            {
                return true; 
            }
        }
        return false; 
    }
    
    public void SaveMap()
    {
        if (_objectsParent.childCount == 0)
        {
            Debug.LogWarning("No objects to save!");
            return;
        }
        
        MapData mapData = new MapData { };
        
        foreach (Transform obj in _objectsParent)
        {
            ObjectData objectData = new ObjectData
            {
                prefabName = obj.name.Replace("(Clone)", ""),
                position = obj.position,
                rotation = obj.rotation
            };

            mapData.objects.Add(objectData);
        }
        
        string json = JsonUtility.ToJson(mapData, true);
        string path = Application.persistentDataPath + SceneParameters.SavePath;
        File.WriteAllText(path, json);

        Debug.Log("The map has been saved: " + path);
    }

    private void PopulateScrollView()
    {
        foreach (GameObject obj in _objectsToPlace)
        {
            GameObject buttonInstance = Instantiate(_buttonPrefab, _scrollViewContent);
            buttonInstance.GetComponentInChildren<TextMeshProUGUI>().text = obj.name;
            buttonInstance.GetComponent<Button>().onClick.AddListener(() => SelectObject(obj));
        }
    }

    private void SelectObject(GameObject obj)
    {
        _selectedObject = obj;
        Debug.Log("An accommodation facility has been selected: " + obj.name);
    }

    private void ClearSelectedObject()
    {
        _selectedObject = null;
        Debug.Log("The hand is empty - you can move objects.");
    }

    private void RotateObject()
    {
        if (Input.touchCount == 2)
        {
            var touch1 = Input.GetTouch(0);
            var touch2 = Input.GetTouch(1);

            var prevDir = touch1.position - touch2.position;
            var currDir = touch1.position - touch2.position;

            var angle = Vector2.SignedAngle(prevDir, currDir);
            _selectedObject.transform.Rotate(Vector3.up, angle);
        }
    }
}

