using UnityEngine;


[RequireComponent(typeof(Rigidbody))]

public class PlayerMoveController : MonoBehaviour {
	
	[SerializeField] private SimpleTouchController _leftController;
	[SerializeField] private SimpleTouchController _rightController;
	[SerializeField] private float _speedProgressiveLook = 3000f;
	[SerializeField] bool _continuousRightController = true;
	[SerializeField] private float _speedMovements = 5f;
	[SerializeField] private Transform _headTransform;
	
	private Rigidbody _rigidbody;
	

	void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_rightController.TouchEvent += RightController_TouchEvent;
	}

	public bool ContinuousRightController
	{
		set{_continuousRightController = value;}
	}

	void RightController_TouchEvent (Vector2 value)
	{
		if(!_continuousRightController)
		{
			UpdateAim(value);
		}
	}

	void Update()
	{
		_rigidbody.MovePosition(transform.position + (transform.forward * _leftController.GetTouchPosition.y * Time.deltaTime * _speedMovements) +
			(transform.right * _leftController.GetTouchPosition.x * Time.deltaTime * _speedMovements) );

		if(_continuousRightController)
		{
			UpdateAim(_rightController.GetTouchPosition);
		}
	}

	private void UpdateAim(Vector2 value)
	{
		if (_headTransform != null)
		{
			var rot = Quaternion.Euler(0f, transform.localEulerAngles.y - value.x * Time.deltaTime * -_speedProgressiveLook, 0f);
			_rigidbody.MoveRotation(rot);
			rot = Quaternion.Euler(_headTransform.localEulerAngles.x - value.y * Time.deltaTime * _speedProgressiveLook, 0f, 0f);
			_headTransform.localRotation = rot;
		}
		else
		{
			var rot = Quaternion.Euler(transform.localEulerAngles.x - value.y * Time.deltaTime * _speedProgressiveLook, 
				transform.localEulerAngles.y + value.x * Time.deltaTime * _speedProgressiveLook, 0f);

			_rigidbody.MoveRotation(rot);
		}
	}

	void OnDestroy()
	{
		_rightController.TouchEvent -= RightController_TouchEvent;
	}

}
