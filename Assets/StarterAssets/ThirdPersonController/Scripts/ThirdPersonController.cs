 using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour, IMove
    {
       

        [Header("Player")]
        [Tooltip("캐릭터의 이동속도 m/s")]
        [SerializeField] private float MoveSpeed;
        public float m_MoveSpeed
        {
            get => MoveSpeed;
            set => MoveSpeed = value;
        }

        [Tooltip("캐릭터가 움직이고있는지")]
        [SerializeField] private bool isMove;

        public bool m_isMove
        {
            get => isMove;
            set => isMove = value;
        }

        [Tooltip("캐릭터의 달리기 속도 m/s")]
        public float m_SprintSpeed = 5.335f;

        [Tooltip("캐릭터가 이동방향을 향하여 회전하는 속도")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("가속 및 감속")]
        public float SpeedChangeRate = 10.0f;

        [Tooltip("마우스 민감도")]
        public float m_Sensitivity = 1f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("점프할수있는 높이")]
        public float JumpHeight = 1.2f;

        [Tooltip("캐릭터는 자체 중력값을 사용 엔진 기본값은 -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("다시 점프할수 있을떄까지 필요하는 시간 즉시 다시 점프할려면 0")]
        public float JumpTimeout = 0.50f;

        [Tooltip("추락 상태로 진입하기까지 소요되는 시간")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("캐릭터가 땅인지 체크")]
        public bool Grounded = true;

        [Tooltip("땅인지 체크하는 구의 OffSet")]
        public float GroundedOffset = -0.14f;

        [Tooltip("땅인지 체크하는 구의 Radius")]
        public float GroundedRadius = 0.28f;

        [Tooltip("Ground 체크 레이어")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("카메라가 따라갈 추적대상")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("카메라 얼만큼 위로 향 할수있는지")]
        public float TopClamp = 70.0f;

        [Tooltip("카메라 얼만큼 아래로 향 할수있는지")]
        public float BottomClamp = -30.0f;

        [Tooltip("카메라를 재정의하는 추가단계 잠겨 있을 떄 카메라 위치를 미세조종하는데 유용함")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("모든 축에서 카메라 위치를 잠금")]
        public bool LockCameraPosition = false;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        public CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private bool m_RotateOnMove = true;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }

        public ThirdPersonController()
        {
            m_MoveSpeed = 2.0f;
            m_isMove = false;
        }


        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
            
        }

        private void Start()
        {
            // 플레이어를 추격하는 카메라의 Y로테이션 오일러값
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM 
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif
            // 애니메이션 파라미터값을 맴버변수로 받음
            AssignAnimationIDs();

            // 시작시 점프,추락 시간초과를 재설정함
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            // 애니메이터를 찾았는지 못찾았는지 Bool변수로 계속 체크함
            _hasAnimator = TryGetComponent(out _animator);

            JumpAndGravity();
            GroundedCheck();
            Move();

            
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void AssignAnimationIDs()
        {
            // 애니메이션 파라미터 맴버변수로 저장
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void GroundedCheck()
        {
            // 오프셋을 사용하여 도구 위치 설정
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // 애니메이터가 True면 애니메이터 Grounded에 Grounded체크 bool변수를 넣어서 애니메이션 업데이트
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            // 입력이 있고 카메라 위치가 고정되지 않은경우
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //마우스 입력에 Time.deltaTime을 곱하지마시오
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                // 마우스 민감도 설정
                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier * m_Sensitivity;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier * m_Sensitivity;
            }

            // 회전을 고정하여 값이 360도로 제한되도록 하기
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // 시네머신은 이 타겟을 따름
            if (!PauseManager.m_Instance.m_isPaused)
            {
                CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                    _cinemachineTargetYaw, 0.0f);
            }
        }

        public void Move()
        {
            // 이동속도, 질주속도 및 질주를 누른경우에 따라 목표 속도를 설정
            float targetSpeed = _input.sprint ? m_SprintSpeed : m_MoveSpeed;

            // 제거, 교체 또는 반복이 쉽도록 설계된 단순한 가속 및 감속

            // 참고 : Vector2의 == 연산자는 근사치를 사용하므로 부동 소수점 오류가 발생하지 않으며 크기보다 저렴함
            // 입력이 없으면 목표 속도를 0으로 설정함
            if (_input.move == Vector2.zero)
            {
                m_isMove = false;
                targetSpeed = 0.0f;
            }
            // 플레이어의 현재 Horizontal 속도에 대한 참조 (백터의 크기)
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // 목표 속도까지 가속 또는 감속
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // 보다 유기적인 속도변화를 제공하는 선형결과가 아닌 곡선 결과를 생성함
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // 속도를 소주점 이하 3자리로 반올림
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // 입력 방향을 Normalize 크기를 0으로만듬 방향만 가지게
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // 이동 입력이 있는경우 플레이어가 움직일떄 플레이어 회전
            if (_input.move != Vector2.zero)
            {
                m_isMove = true;
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                

                if (m_RotateOnMove)
                {
                    // 카메라 위치를 기준으로 입력 방향을 향하도록 회전
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                }
            }


            
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // 플레이어 이동
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // 애니메이터 스피드 파라미터값을 _animationBlend값으로
            // 모션스피드값을 InputMagnitude값으로 증가 및 감소
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                
            }
        }

        private void JumpAndGravity()
        {
            // 땅일경우
            if (Grounded)
            {
                // 떨어지는 타이머를 재설정함
                _fallTimeoutDelta = FallTimeout;

                // Jump와 Fall 애니메이터 파라미터를 false함
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // 땅일경우 속도가 무한이 떨어지는걸 중지함
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // 점프를누르고 점프타임아웃델타가 0과 같거나 작을경우
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // 원하는 높이에 도달 하는데 필요한 속도를 제곱근으로 저장
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // 점프 애니메이션 True
                    if (_hasAnimator)
                    {
                       
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // 점프시간 초과가 0보다 작거나 클떄
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            // 땅이 아닐경우
            else
            {

                // 점프 시간 초과 타이머를 재설정
                _jumpTimeoutDelta = JumpTimeout;

                // Fall 타임아웃일떄
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // Fall 타임아웃이 아닐떄
                    if (_hasAnimator)
                    {
                       
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // 땅이 아닐경우 점프 못하게
                _input.jump = false;
            }

            // 터미널 아래에 있는 경우시간에따라 중력을 적용함 (시간에 따라 떨어지는 속도를 높힐려면 델타타임을 두번곱함)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            // 땅일경우 초록색 땅이아니면 빨간색 기즈모 색상 설정
            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // 충돌체의 위치와 일치하는 반경에 기즈모를 그림
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        public void Set_Sensitivity(float _newSensitivity)
        {
            // 새로운 민감도 설정
            m_Sensitivity = _newSensitivity;
        }

        public void Set_RotateOnMove(bool _newRoatateOnMove)
        {
            m_RotateOnMove = _newRoatateOnMove;
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    }
}