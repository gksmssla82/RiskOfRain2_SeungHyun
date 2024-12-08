using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		

		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool m_NormalShoot;
		public bool m_M2Shoot;
		public bool m_RShoot;
		public bool m_Sliding;
		private bool m_SprintToggle = false;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			if (value.isPressed)
			{
				// 스프린트키를 눌렀을경우 반대로 설정해줘 On/Off가 가능해짐
				m_SprintToggle = !m_SprintToggle;
				// 이 반대로 설정한 Toggle을 Sprint 변수에 적용
				SprintInput(m_SprintToggle);
			}
		}

		public void OnShoot_Normal(InputValue value)
		{
			NoramalShootInput(value.isPressed);
		}

		public void OnShoot_M2(InputValue value)
        {
			M2ShootInput(value.isPressed);
        }

		public void OnShoot_R(InputValue value)
        {
			RShootInput(value.isPressed);
        }

		public void OnSliding(InputValue value)
        {
			SlidingInput(value.isPressed);
        }
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void NoramalShootInput(bool newNoramlShootState)
		{
			m_NormalShoot = newNoramlShootState;
		}

		public void M2ShootInput(bool newM2ShootState)
        {
			m_M2Shoot = newM2ShootState;
        }

		public void RShootInput(bool newRShootState)
        {
			m_RShoot = newRShootState;
        }

		public void SlidingInput(bool newSlidingState)
        {
			m_Sliding = newSlidingState;
        }

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}