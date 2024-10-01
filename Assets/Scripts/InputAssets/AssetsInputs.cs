using UnityEngine;
using UnityEngine.Events;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class AssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		[SerializeField]private Vector2 rotate;
		[SerializeField]private float _rotateSpeed;
		private Vector2 _rotateDelta;
		private Vector2 _screenPosition;
		private UnityAction<bool> _onPressScreen;

		public Vector2 rotateDelta
		{
			get => _rotateDelta;
			set => _rotateDelta = value;
		}

		public Vector2 screenPosition
		{
			get => _screenPosition;
			set => _screenPosition = value;
		}

		public UnityAction<bool> onPressScreen
		{
			get => _onPressScreen;
			set => _onPressScreen = value;
		}

#if ENABLE_INPUT_SYSTEM
		public void OnRotate(InputValue value)
		{
			RotateInput(value.Get<Vector2>());
			
		}

		public void OnScreenPosition(InputValue value)
		{
			_screenPosition = value.Get<Vector2>();
		}

		public void OnPressScreen(InputValue value)
		{
			_onPressScreen?.Invoke(value.isPressed);
		}
#endif


		private void RotateInput(Vector2 valueRotate)
		{
			_rotateDelta = valueRotate * rotate * _rotateSpeed;
			//print($"Input Value is : {_rotateDelta}");
		} 
	}
	
}