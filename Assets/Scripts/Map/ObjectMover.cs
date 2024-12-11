using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    private Camera _mainCamera;
    private bool _isDragging;
    private Vector3 _offset;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); 
            Ray ray = _mainCamera.ScreenPointToRay(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
                {
                    _isDragging = true;
                    _offset = transform.position - hit.point;
                }
            }
            else if (touch.phase == TouchPhase.Moved && _isDragging)
            {
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Vector3 newPosition = hit.point + _offset;
                    newPosition.y = transform.position.y; 
                    transform.position = newPosition;
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                _isDragging = false;
            }
        }
    }
}
