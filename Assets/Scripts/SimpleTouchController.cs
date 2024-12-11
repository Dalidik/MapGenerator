using UnityEngine;


public class SimpleTouchController : MonoBehaviour {

	
	public delegate void TouchDelegate(Vector2 value);
	public event TouchDelegate TouchEvent;

	public delegate void TouchStateDelegate(bool touchPresent);
	public event TouchStateDelegate TouchStateEvent;

	
	[SerializeField]
	private RectTransform _joystickArea;
	private bool _touchPresent = false;
	private Vector2 _movementVector;


	public Vector2 GetTouchPosition
	{
		get { return _movementVector;}
	}


	public void BeginDrag()
	{
		_touchPresent = true;
		if(TouchStateEvent != null)
			TouchStateEvent(_touchPresent);
	}

	public void EndDrag()
	{
		_touchPresent = false;
		_movementVector = _joystickArea.anchoredPosition = Vector2.zero;

		if(TouchStateEvent != null)
			TouchStateEvent(_touchPresent);

	}

	public void OnValueChanged(Vector2 value)
	{
		if(_touchPresent)
		{
			_movementVector.x = ((1 - value.x) - 0.5f) * 2f;
			_movementVector.y = ((1 - value.y) - 0.5f) * 2f;

			if(TouchEvent != null)
			{
				TouchEvent(_movementVector);
			}
		}

	}

}
