using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

namespace TarodevController
{
  
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        // Public for external hooks

        public Vector3 Velocity { get; private set; }
        public FrameInput Input { get; private set; }
        public bool JumpingThisFrame { get; private set; }

        public bool FallThisFrame { get; private set; }

        private bool FallThisFrame_Last = false;

        public bool ReverseThisFrame { get; private set; }
        public bool LandingThisFrame { get; private set; }
        public Vector3 RawMovement { get; private set; }

        public Vector3 RawMovement_Dash { get; private set; }

        private PlayerAnimator _playerAnimator;

        public Vector3 LeftUp { get; private set; }

        public Vector3 LeftDown { get; private set; }

        public Vector3 RightUp { get; private set; }

        public Vector3 RightDown { get; private set; }


      // public Joystick joystick;
      // private Jump_Btn JumpBtn;
      // private Dash_Btn DashBtn;
      // private Reverse_Btn ReverseBtn;
      //

        public bool Grounded => _colDown;

        public bool Dashed { get; private set; }

        private Vector3 _lastPosition;
        public float _currentHorizontalSpeed;
        public float _currentVerticalSpeed;

        // private float _currentVerticalSpeed_Fall;

        // This is horrible, but for some reason colliders are not fully established when update starts...
        private bool _active;

        public float thresHold = 0.1f; //输入阈值

        //dash
        // 处于冲刺状态
        public bool isDashing;
        private bool isDashing_Over = true;

        private DashWave dashWave;

        //死亡
        private DeathCircle deathCircle;

        //轨迹渲染器
        private TrailRenderer trailRenderer;

        //对话
        public bool isTalking;

        // 结束冲刺状态的计时器
        public float endTimer = 1f;
        public float endTimer_2;

        // 非正常结束(提前结束冲刺)
        public bool isAdvancedEnd;

        public bool CanReverse;

        //获得翻转能力
        public bool gainReverse;

        //Cinemachine
        public Cinemachine.CinemachineImpulseSource impulseSourceXY;
        public Cinemachine.CinemachineImpulseSource impulseSourceX;
        public Cinemachine.CinemachineImpulseSource impulseSourceY;
        public Animator anim;

        public Animation_Event animation_Event;

        //shader
        public SpriteRenderer Sp;
        private Material material;

        //sound
        private AudioSource audioSource;
        public bool isSound = true;

        void Awake()
        {
            Invoke(nameof(PlayRebirthDissolve), 0.5f);
            Invoke(nameof(Activate), 0.75f);

            _playerAnimator = GetComponentInChildren<PlayerAnimator>();
            dashWave = GetComponentInChildren<DashWave>();
            deathCircle = GetComponentInChildren<DeathCircle>();
            //impulseSource = GetComponentInChildren<Cinemachine.CinemachineImpulseSource>();
            Parent = transform.parent; 
            trailRenderer = GetComponentInChildren<TrailRenderer>();
            trailRenderer.enabled = false; //避免开局位移产生拖尾
            material = Sp.material;
            audioSource = GetComponent<AudioSource>();
          //  JumpBtn = GameObject.Find("Jump").GetComponent<Jump_Btn>();
          //  DashBtn = GameObject.Find("Dash").GetComponent<Dash_Btn>();
          //  ReverseBtn = GameObject.Find("Reverse").GetComponent<Reverse_Btn>();

            //【初次游玩】第一关与精灵对话前 不能翻转
            if (SceneManager.GetActiveScene().name == GameData_SO.Levels[0])
            {
                //GameManager_global.GetInstance().gameData_SO.gainReverse = false;
                //gainReverse = false;
                gainReverse = GameManager_global.GetInstance().gameData_SO.gainReverse;
            }
            else
            {
                GameManager_global.GetInstance().gameData_SO.gainReverse = true;
                gainReverse = true;
            }
        }

        void Activate() => _active = true;

        private void Start()
        {
            //读取玩家关卡记录的重生点，若有非零记录则玩家在该点（只改位置，不设重生点）
            for (int i = 0; i < GameData_SO.Levels.Length; i++)
            {
                if (SceneManager.GetActiveScene().name == GameData_SO.Levels[i] &&
                    (GameManager_global.GetInstance().gameData_SO.levelRecords[i].rebirth_pos != Vector3.zero))
                {
                    Debug.Log("111");
                    transform.position = GameManager_global.GetInstance().gameData_SO.levelRecords[i].rebirth_pos;
                    GameManager.Instance.isReverse = GameManager_global.GetInstance().gameData_SO.levelRecords[i].rebirth_Reverse;
                    if (GameManager.Instance.isReverse)
                    {
                        animation_Event.Reverse_1(); //变蓝
                    }
                }
            }
            trailRenderer.enabled = true;

            //开始时，屏幕全黑，然后圈从内向外变大，显示游戏画面
            deathCircle.SetCircleMin();
            Invoke(nameof(PlayCircleInToOut), 0.5f); //不Invoke0.5秒的话容易卡掉没效果
        }

        void PlayCircleInToOut()
        {
            StartCoroutine(deathCircle.PlayCircleInToOut());
        }

        private float lastY = 0;
        private float lastTime = 0;
        private bool aaa = false;

        private void Update()
        {
            //if (UnityEngine.Input.GetKeyDown(KeyCode.Return))
            //{
            //    //从内向外播放圆圈
            //    StartCoroutine(deathCircle.PlayCircleInToOut());
            //}

            if (!_active || isPaused) return;
            if (isDead) return; //死亡时禁用Update：人物操作、移动

            // Calculate velocity
            Velocity = (transform.position - _lastPosition) / Time.deltaTime;
            _lastPosition = transform.position;

            if (!isTalking) GatherInput(); //对话时禁用：人物操作
            // Debug.Log(UnityEngine.Input.GetAxisRaw("Horizontal"));
            RunCollisionChecks();
            CalulateDash();

            CalculateWalk(); // Horizontal movement
            CaculateReverse();
            CalculateJumpApex(); // Affects fall speed, so calculate before gravity
            CalculateGravity(); // Vertical movement
            CalculateJump(); // Possibly overrides vertical

            MoveCharacter(); // Actually perform the axis movement
            Caculate_Enchanting();

         //  float y = this.transform.position.y;
         //  float time = UnityEngine.Time.realtimeSinceStartup;
         //  if (Mathf.Abs(y - lastY) < 0.1f )
         //  {
         //      if(time - lastTime > 0.5f)
         //      {
         //          Debug.LogError("time:" + Time.frameCount);
         //          aaa = true;
         //      }
         //  }
         //  else
         //  {
         //      lastY = y;
         //      lastTime = time;
         //      aaa = false;
         //  }
        }

        #region Gather Input

        private void GatherInput()
        {
            Input = new FrameInput
            {
              JumpDown = UnityEngine.Input.GetButtonDown("Jump"),
              JumpUp = UnityEngine.Input.GetButtonUp("Jump"),
             X = UnityEngine.Input.GetAxisRaw("Horizontal"),
             Y = UnityEngine.Input.GetAxisRaw("Vertical"),
              Dash = UnityEngine.Input.GetButtonDown("Dash"),
              Reverse = UnityEngine.Input.GetButtonDown("Reverse")
             //JumpDown = JumpBtn.down,
             //JumpUp = JumpBtn.up,
             //X = joystick.Horizontal,
             //Y = joystick.Vertical,
             //Dash = DashBtn.Dash,
             //Reverse = ReverseBtn.Reverse
            };
            if (Input.JumpDown)
            {
                _lastJumpPressed = Time.time;
            }
        }

        private Vector2Int GetHVinput()
        {
            int xInput = 0;
            int yInput = 0;
            if (Mathf.Abs(UnityEngine.Input.GetAxisRaw("Horizontal")) > thresHold)
            {
                xInput = UnityEngine.Input.GetAxisRaw("Horizontal") > 0 ? 1 : -1;
            }
            if (Mathf.Abs(UnityEngine.Input.GetAxisRaw("Vertical")) > thresHold)
            {
                yInput = UnityEngine.Input.GetAxisRaw("Vertical") > 0 ? 1 : -1;
            }
            return new Vector2Int(xInput, yInput);

        }








        #endregion


        #region Collisions
        public GameObject MovingGround;
        private Transform Parent;
        public GameObject Ice;
        public GameObject Cake;

        [Header("COLLISION")][SerializeField] private Bounds _characterBounds;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private LayerMask _cakeLayer;
        //[SerializeField] private LayerMask _rebirthLayer;
        // [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private int _detectorCount = 3;
        [SerializeField] private float _detectionRayLength = 0.1f;
        [SerializeField][Range(0.1f, 0.3f)] private float _rayBuffer = 0.1f; // Prevents side detectors hitting the ground

        private RayRange _raysUp, _raysRight, _raysDown, _raysLeft;
        [SerializeField] public bool _colUp, _colRight, _colDown, _colLeft;
        //[SerializeField] private bool _colUp_oneway, _colRight_oneway, _colDown_oneway, _colLeft_oneway;
        [SerializeField] public bool _colUp_enemy, _colRight_enemy, _colDown_enemy, _colLeft_enemy;
        [SerializeField] public bool _colUp_ReverseGround, _colDown_ReverseGround, _colLeft_ReverseGround, _colRight_ReverseGround;
        //  [SerializeField] public bool _colUp_Rebirth,_colRight_Rebirth,_colDown_Rebirth,_colLeft_Rebirth;
        [SerializeField] public bool _colUp_Moving, _colRight_Moving, _colDown_Moving, _colLeft_Moving;

        [SerializeField] public bool _colUP_Ice, _colRight_Ice, _colDown_Ice, _colLeft_Ice;

        [SerializeField] private bool _colUP_Cake, _colRight_Cake, _colDown_Cake, _colLeft_Cake;
        private float _timeLeftGrounded;

        // We use these raycast checks for pre-collision information
        private void RunCollisionChecks()
        {
            // Generate ray ranges. 
            CalculateRayRanged();
            Calculate_In();
            // if(DamageDetection(_raysUp)|| DamageDetection(_raysDown)|| DamageDetection(_raysLeft)|| DamageDetection(_raysRight))
            //  {
            //      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            //  }
            LandingThisFrame = false;
            FallThisFrame = false;
            var groundedCheck = RunDetection(_raysDown);
            var groundedCheck_Re = RunDetection(_raysUp);

            if ((!GameManager.Instance.isReverse && groundedCheck) || (GameManager.Instance.isReverse && groundedCheck_Re))
            {
                CanReverse = true;
                dashCount = dashMaxCount;
                Curr_Re_HoSpeed = 1;
            }

            // if(GameManager.Instance.isReverse && groundedCheck_Re) CanReverse = true;

            if (!GameManager.Instance.isReverse)
            {
                if (_colDown && !groundedCheck) _timeLeftGrounded = Time.time; // Only trigger when first leaving
                else if (!_colDown && groundedCheck)
                {
                    _coyoteUsable = true; // Only trigger when first touching
                    LandingThisFrame = true;
                    FallThisFrame_Last = false;
                }
            }
            else
            {
                if (_colUp && !groundedCheck_Re) _timeLeftGrounded = Time.time; // Only trigger when first leaving
                else if (!_colUp && groundedCheck_Re)
                {
                    _coyoteUsable = true; // Only trigger when first touching
                    LandingThisFrame = true;
                    FallThisFrame_Last = false;
                }
            }


            _colDown = groundedCheck;
            _colUp = groundedCheck_Re;
            // The rest
            // _colUp = RunDetection(_raysUp);
            _colLeft = RunDetection(_raysLeft);
            _colRight = RunDetection(_raysRight);



            _colDown = groundedCheck;
            _colUp = groundedCheck_Re;
            // The rest
            _colUp = RunDetection(_raysUp);
            _colLeft = RunDetection(_raysLeft);
            _colRight = RunDetection(_raysRight);



            _colDown_enemy = RunDetection_enemy(_raysDown);
            _colUp_enemy = RunDetection_enemy(_raysUp);
            _colLeft_enemy = RunDetection_enemy(_raysLeft);
            _colRight_enemy = RunDetection_enemy(_raysRight);


            Caculate_enemy(_colUp_enemy, _colDown_enemy, _colLeft_enemy, _colRight_enemy);


            _colDown_Moving = RunDetection_Moving(_raysDown);
            _colLeft_Moving = RunDetection_Moving(_raysLeft);
            _colRight_Moving = RunDetection_Moving(_raysRight);
            _colUp_Moving = RunDetection_Moving(_raysUp);

            Caculate_MovingGround(_colDown_Moving, _colUp_Moving, _colLeft_Moving, _colRight_Moving);


            // _colUp_ReverseGround = RunDetection_ReverseGround(_raysUp);
            // _colDown_ReverseGround = RunDetection_ReverseGround(_raysDown);
            // _colLeft_ReverseGround = RunDetection_ReverseGround(_raysLeft);
            // _colRight_ReverseGround = RunDetection_ReverseGround(_raysRight);
            // Caculate_ReverseGround(_colUp_ReverseGround, _colDown_ReverseGround, _colLeft_ReverseGround, _colRight_ReverseGround);


            _colDown_Ice = RunDetection_Ice(_raysDown);
            _colLeft_Ice = RunDetection_Ice(_raysLeft);
            _colRight_Ice = RunDetection_Ice(_raysRight);
            _colUP_Ice = RunDetection_Ice(_raysUp);
            Calculate_Ice(_colUP_Ice, _colDown_Ice, _colLeft_Ice, _colRight_Ice);





            // _colDown_Rebirth = RunDetection_Rebirth(_raysDown);
            // _colLeft_Rebirth = RunDetection_Rebirth(_raysLeft);
            // _colUp_Rebirth = RunDetection_Rebirth(_raysUp);
            // _colRight_Rebirth = RunDetection_Rebirth(_raysRight);
            //


            _colDown_Cake = RunDetection_Cake(_raysDown);
            _colUP_Cake = RunDetection_Cake(_raysUp);
            _colLeft_Cake = RunDetection_Cake(_raysLeft);
            _colRight_Cake = RunDetection_Cake(_raysRight);
            Calculate_Cake(_colUP_Cake, _colDown_Cake, _colLeft_Cake, _colRight_Cake);















            //  _colDown_oneway = RunDetection_other(_raysDown);
            //  _colUp_oneway = RunDetection_other(_raysUp);
            //  _colLeft_oneway = RunDetection_other(_raysLeft);  
            //  _colRight_oneway = RunDetection_other(_raysRight);
            //  

            bool RunDetection(RayRange range)
            {
                return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _groundLayer));
            }
            bool RunDetection_enemy(RayRange range)
            {
                return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _groundLayer) ? Physics2D.Raycast(point, range.Dir, _detectionRayLength, _groundLayer).collider.gameObject.CompareTag("enemy") : false);
            }


            bool RunDetection_Cake(RayRange range)
            {
                return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _cakeLayer) ? Physics2D.Raycast(point, range.Dir, _detectionRayLength, _cakeLayer) && (Cake = Physics2D.Raycast(point, range.Dir, _detectionRayLength, _cakeLayer).collider.gameObject) : false);
            }

            bool RunDetection_Moving(RayRange range)
            {
                return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _groundLayer) ? Physics2D.Raycast(point, range.Dir, _detectionRayLength, _groundLayer).collider.gameObject.CompareTag("MovingGround") && (MovingGround = Physics2D.Raycast(point, range.Dir, _detectionRayLength, _groundLayer).collider.gameObject) : false);
            }


            bool RunDetection_Ice(RayRange range)
            {
                return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _groundLayer) ? Physics2D.Raycast(point, range.Dir, _detectionRayLength, _groundLayer).collider.gameObject.CompareTag("Ice") && (Ice = Physics2D.Raycast(point, range.Dir, _detectionRayLength, _groundLayer).collider.gameObject) : false);
            }

            //bool RunDetection_ReverseGround(RayRange range)
            //{
            //    return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _groundLayer) ? Physics2D.Raycast(point, range.Dir, _detectionRayLength, _groundLayer).collider.gameObject.CompareTag("ReverseGround") : false);
            //}

            //   bool RunDetection_Rebirth(RayRange range)
            //   {
            //       return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _rebirthLayer));
            //   }











        }

        private void CalculateRayRanged()
        {
            // This is crying out for some kind of refactor. 
            var b = new Bounds(transform.position, _characterBounds.size);

            _raysDown = new RayRange(b.min.x + _rayBuffer, b.min.y, b.max.x - _rayBuffer, b.min.y, Vector2.down);
            _raysUp = new RayRange(b.min.x + _rayBuffer, b.max.y, b.max.x - _rayBuffer, b.max.y, Vector2.up);
            _raysLeft = new RayRange(b.min.x, b.min.y + _rayBuffer, b.min.x, b.max.y - _rayBuffer, Vector2.left);
            _raysRight = new RayRange(b.max.x, b.min.y + _rayBuffer, b.max.x, b.max.y - _rayBuffer, Vector2.right);
        }


        private void Calculate_In()
        {
            var c = new Bounds(transform.position, _characterBounds.size);
            LeftUp = new Vector3(c.min.x, c.max.y, 0);
            LeftDown = new Vector3(c.min.x, c.min.y, 0);
            RightUp = new Vector3(c.max.x, c.max.y, 0);
            RightDown = new Vector3(c.max.x, c.min.y, 0);

        }





        private IEnumerable<Vector2> EvaluateRayPositions(RayRange range)
        {
            for (var i = 0; i < _detectorCount; i++)
            {
                var t = (float)i / (_detectorCount - 1);
                yield return Vector2.Lerp(range.Start, range.End, t);
            }
        }

        private void OnDrawGizmos()
        {
            // Bounds
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + _characterBounds.center, _characterBounds.size);

            // Rays
            if (!Application.isPlaying)
            {
                CalculateRayRanged();
                Gizmos.color = Color.blue;
                foreach (var range in new List<RayRange> { _raysUp, _raysRight, _raysDown, _raysLeft })
                {
                    foreach (var point in EvaluateRayPositions(range))
                    {
                        Gizmos.DrawRay(point, range.Dir * _detectionRayLength);
                    }
                }
            }

            if (!Application.isPlaying) return;

            // Draw the future position. Handy for visualizing gravity
            Gizmos.color = Color.red;
            var move = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed) * Time.deltaTime;
            Gizmos.DrawWireCube(transform.position + move, _characterBounds.size);
        }
        #endregion


        #region dash
        [Header("DASH")][SerializeField] private float dashSpeed = 40f;//冲刺速率

        //[SerializeField]public float duration = 0.2f; // 冲刺持续时间
        [SerializeField] public float duration_1 = 0.2f; // 冲刺一段持续时间
        [SerializeField] public float duration_2 = 0.05f; // 冲刺二段持续时间

        [SerializeField] private float dashCount = 1; // 当前冲刺次数
        [SerializeField] private float dashMaxCount = 1;
        private float currDirection_X = 0;
        private float currDirection_Y = 0;
        private float currDashSpeed = 0;
        //private float _currentHorizontalSpeed_Dash = 0;
        //private float _currentVerticalSpeed_Dash = 0;
        public float Dashdir;
        private void CalulateDash()
        {
            if (isDashing)
            {
                endTimer += Time.deltaTime;
                LandingThisFrame = true;
                //currDashSpeed *= Mathf.Lerp(1, 0, endTimer_2 / duration_2);
                ////currDashSpeed = 0;
                //_currentHorizontalSpeed = currDashSpeed * currDirection_X;
                //_currentVerticalSpeed = currDashSpeed * currDirection_Y;

            }
            if (endTimer > (duration_1 - duration_2))
            {
                endTimer_2 += Time.deltaTime;
                isDashing_Over = true;
                // _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, _deAcceleration * Time.deltaTime);
                _currentHorizontalSpeed -= currDashSpeed * currDirection_X * Time.deltaTime;
                //_currentVerticalSpeed += (currDashSpeed * currDirection_Y * -1 * Mathf.Lerp(1, 0, endTimer_2 / duration_2));
                _currentVerticalSpeed -= currDashSpeed * currDirection_Y * Time.deltaTime;
            }

            if ((endTimer >= duration_1 && isDashing))
            {
                isAdvancedEnd = false;
                _currentHorizontalSpeed = 0;
                _currentVerticalSpeed = 0;
                isDashing = false;
                Dashed = false;
                FallThisFrame_Last = false;
                endTimer = 0;
            }
            // Debug.Log(currDashSpeed);
            if (!Input.Dash || dashCount <= 0 || isDashing)
            {
                dashWave.DashUpdate(false);
                return;
            }
            // DashThisFrame = true;
            Dashed = true;
            isDashing = true;
            isDashing_Over = false;
            dashCount -= 1;
            endTimer = 0;
            endTimer_2 = 0;
            isAdvancedEnd = false;
            currDirection_X = 0;
            currDirection_Y = 0;
            currDashSpeed = dashSpeed;
            Vector2 direction = GetHVinput();
            //Debug.Log(direction.y);
            if (direction.Equals(Vector2Int.zero))
                direction = Vector2Int.right * (int)_playerAnimator.transform.localScale.x;

            //dashSpeed = dashSpeed * Mathf.Lerp(0, 1, endTimer_2/duration_2);

            // dashSpeed -= dashSpeed * 1;
            currDirection_X = direction.x;
            currDirection_Y = direction.y;

            switch (currDirection_X, currDirection_Y)
            {
                case (-1, 0):
                    Dashdir = 0.5f;
                    break;
                case (-1, 1):
                    Dashdir = 0.25f;
                    break;
                case (-1, -1):
                    Dashdir = 0.75f;
                    break;
                case (0, 1):
                    Dashdir = 0;
                    break;
                case (1, 1):
                    Dashdir = 0.25f;
                    break;
                case (1, 0):
                    Dashdir = 0.5f;
                    break;
                case (1, -1):
                    Dashdir = 0.75f;
                    break;
                case (0, -1):
                    Dashdir = 1f;
                    break;

            }

            //相机抖动
            //[DOTween]使用cinemachine相机下，其Transform组件不可被代码修改
            //Camera.main.transform.DOComplete();
            //var tmp = Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
            //tmp.onComplete += () => { print("相机抖动完成"); };
            //[Cinemachine]
            //impulseSource.GenerateImpulse(Camera.main.transform.forward);
            if (currDirection_X != 0 && currDirection_Y != 0)
            {
                impulseSourceXY.GenerateImpulse();
            } 
            else if (currDirection_X != 0 && currDirection_Y == 0)
            {
                impulseSourceX.GenerateImpulse();
            }
            else if (currDirection_X == 0 && currDirection_Y != 0)
            {
                impulseSourceY.GenerateImpulse();
            }

            _currentHorizontalSpeed = currDashSpeed * direction.x;
            _currentVerticalSpeed = currDashSpeed * direction.y;

            //播放冲刺屏幕波纹特效
            //FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));
            dashWave.DashUpdate(true);

            //播放冲刺音效
            AllSceneMusicManager.Instance.PlaySound("dash");
        }
        #endregion


        #region Walk

        [Header("WALKING")][SerializeField] private float _acceleration = 90;
        [SerializeField] private float _moveClamp = 13;
        [SerializeField] private float _deAcceleration = 60f;
        //[SerializeField] private float _apexBonus = 2;

        private void CalculateWalk()
        {
            if (Input.X != 0)
            {
                // Set horizontal move speed
                if (isDashing_Over)
                {
                    _currentHorizontalSpeed += Input.X * _acceleration * Time.deltaTime;

                    // clamped by max frame movement

                    _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -_moveClamp * Curr_Re_HoSpeed, _moveClamp * Curr_Re_HoSpeed);
                }

                if (isSound)
                {
                    if (!GameManager.Instance.isReverse)
                    {
                        if (!audioSource.isPlaying && _colDown)
                        {
                            audioSource.Play();
                        }

                    }
                    else
                    {
                        if (!audioSource.isPlaying && _colUp)
                        {
                            audioSource.Play();
                        }
                    }

                }




                // Apply bonus at the apex of a jump
                // var apexBonus = Mathf.Sign(Input.X) * _apexBonus * _apexPoint;
                //  _currentHorizontalSpeed += apexBonus * Time.deltaTime;
            }
            else
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
                // No input. Let's slow the character down
                if (!isDashing)
                    _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, _deAcceleration * Time.deltaTime);
            }

            if (_currentHorizontalSpeed > 0 && _colRight || _currentHorizontalSpeed < 0 && _colLeft)
            {
                // Don't walk through walls
                _currentHorizontalSpeed = 0;
            }
        }
        #endregion


        #region Gravity

        [Header("GRAVITY")][SerializeField] private float _fallClamp = -40f;
        [SerializeField] private float _fallClampReverse = 40f;
        // [SerializeField] private float _minReverseSpeed = 80f;
        //[SerializeField] private float _maxReverseSpeed = 120f;
        [SerializeField] private float _minFallSpeed = 80f;
        [SerializeField] private float _maxFallSpeed = 120f;
        private float _fallSpeed;

        private void CalculateGravity()
        {
            if (!GameManager.Instance.isReverse)
            {
                if (_colDown)
                {

                    if (_currentVerticalSpeed < 0) _currentVerticalSpeed = 0;

                }
                else
                {
                    // Add downward force while ascending if we ended the jump early

                    if (isDashing) _fallSpeed = 0f;
                    else
                    {

                        var fallSpeed = _endedJumpEarly && _currentVerticalSpeed > 0 ? _fallSpeed * _jumpEndEarlyGravityModifier : _fallSpeed;

                        _currentVerticalSpeed -= fallSpeed * Time.deltaTime;
                        // Debug.Log(_currentVerticalSpeed * _currentVerticalSpeed_Fall);
                        if (_currentVerticalSpeed < 0 && !FallThisFrame_Last)
                        {
                            //Debug.Log("thisFrame");
                            FallThisFrame = true;
                            FallThisFrame_Last = true;

                        }
                        //_currentVerticalSpeed_Fall = _currentVerticalSpeed;
                    }

                    // Fall

                    // Debug.Log(_currentVerticalSpeed);

                    // Clamp
                    if (_currentVerticalSpeed < _fallClamp) _currentVerticalSpeed = _fallClamp;
                }
            }
            else
            {
                if (_colUp)
                {
                    // Move out of the ground
                    if (_currentVerticalSpeed > 0) _currentVerticalSpeed = 0;

                }
                else
                {
                    // Add downward force while ascending if we ended the jump early
                    // var fallSpeed = _endedJumpEarly && _currentVerticalSpeed > 0 ? _fallSpeed * _jumpEndEarlyGravityModifier : _fallSpeed;
                    if (isDashing)
                    {
                        //_fallSpeed = 0f;

                    }
                    else
                    {
                        // var fallSpeed = _endedJumpEarly && _currentVerticalSpeed < 0 ? _fallSpeed * _jumpEndEarlyGravityModifier : _fallSpeed;
                        // var test_sp = 30f;
                        // _currentVerticalSpeed += test_sp * Time.deltaTime;
                        var fallSpeed = _endedJumpEarly && _currentVerticalSpeed < 0 ? _fallSpeed * _jumpEndEarlyGravityModifier : _fallSpeed;
                        // Debug.Log(_fallSpeed);
                        _currentVerticalSpeed += fallSpeed * Time.deltaTime;



                        if (_currentVerticalSpeed > 0 && !FallThisFrame_Last && !isDashing)
                        {
                            Debug.Log("thisFrame_21");
                            FallThisFrame = true;
                            FallThisFrame_Last = true;

                        }

                    }

                    // Fall

                    // Debug.Log(_currentVerticalSpeed);

                    // Clamp
                    if (_currentVerticalSpeed > _fallClampReverse) _currentVerticalSpeed = _fallClampReverse;
                }
            }
        }
        #endregion


        #region Jump

        [Header("JUMPING")][SerializeField] private float _jumpHeight = 10;
        [SerializeField] private float _jumpApexThreshold = 10f;
        [SerializeField] private float _coyoteTimeThreshold = 0.1f;
        [SerializeField] private float _jumpBuffer = 0.1f;
        [SerializeField] private float _jumpEndEarlyGravityModifier = 3;
        //[SerializeField] private float _jumpEndEarlyGravityModifier_Re = -3;
        [SerializeField] private bool _coyoteUsable;
        [SerializeField] private bool _endedJumpEarly = true;
        [SerializeField] private float _apexPoint; // Becomes 1 at the apex of a jump
        [SerializeField] private float _lastJumpPressed;
        private bool CanUseCoyote => _coyoteUsable && !_colDown && _timeLeftGrounded + _coyoteTimeThreshold > Time.time;
        private bool CanUseCoyte_Re => _coyoteUsable && !_colUp && _timeLeftGrounded + _coyoteTimeThreshold > Time.time;
        private bool HasBufferedJump => _colDown && _lastJumpPressed + _jumpBuffer > Time.time;

        private bool HaBufferedJump_Re => _colUp && _lastJumpPressed + _jumpBuffer > Time.time;


        private void CalculateJumpApex()
        {

            if (!GameManager.Instance.isReverse)
            {
                if (!_colDown)
                {
                    // Gets stronger the closer to the top of the jump
                    _apexPoint = Mathf.InverseLerp(_jumpApexThreshold, 0, Mathf.Abs(Velocity.y));
                    _fallSpeed = Mathf.Lerp(_minFallSpeed, _maxFallSpeed, _apexPoint);
                }
                else
                {
                    _apexPoint = 0;
                }
            }
            else
            {
                if (!_colUp)
                {
                    // Gets stronger the closer to the top of the jump
                    _apexPoint = Mathf.InverseLerp(_jumpApexThreshold, 0, Mathf.Abs(Velocity.y));
                    _fallSpeed = Mathf.Lerp(_minFallSpeed, _maxFallSpeed, _apexPoint);
                }
                else
                {
                    _apexPoint = 0;
                }
            }


        }

        private void CalculateJump()
        {

            // Jump if: grounded or within coyote threshold || sufficient jump buffer
            if (!GameManager.Instance.isReverse)
            {

                if (Input.JumpDown && CanUseCoyote || HasBufferedJump)
                {
                    _currentVerticalSpeed = _jumpHeight;
                    _endedJumpEarly = false;
                    _coyoteUsable = false;
                    _timeLeftGrounded = float.MinValue;
                    JumpingThisFrame = true;
                }
                else
                {
                    JumpingThisFrame = false;
                }

                // End the jump early if button released
                if (!_colDown && Input.JumpUp && !_endedJumpEarly && Velocity.y > 0)
                {
                    // _currentVerticalSpeed = 0;
                    _endedJumpEarly = true;
                }

                if (_colUp)
                {
                    if (_currentVerticalSpeed > 0) _currentVerticalSpeed = 0;
                }
            }
            else
            {

                if (Input.JumpDown && CanUseCoyte_Re || HaBufferedJump_Re)
                {
                    // Debug.Log(9);
                    _currentVerticalSpeed = _jumpHeight * -1;
                    _endedJumpEarly = false;
                    _coyoteUsable = false;
                    _timeLeftGrounded = float.MinValue;
                    JumpingThisFrame = true;
                }
                else
                {
                    JumpingThisFrame = false;
                }

                // End the jump early if button released
                if (!_colUp && Input.JumpUp && !_endedJumpEarly && Velocity.y < 0)
                {
                    // _currentVerticalSpeed = 0;
                    _endedJumpEarly = true;
                }

                if (_colDown)
                {
                    if (_currentVerticalSpeed < 0) _currentVerticalSpeed = 0;
                }
            }

            //播放跳跃音效
            if (JumpingThisFrame)
                AllSceneMusicManager.Instance.PlaySound("jump");
        }
        #endregion


        #region Reverse

        // [Header("Reverse")] [SerializeField] private float _jumpHeight = 30;
        // [SerializeField] private float _ReverseApexThreshold = 10f;
        // [SerializeField] private float _coyoteTimeThreshold = 0.1f;
        // [SerializeField] private float _jumpBuffer = 0.1f;
        // [SerializeField] private float _jumpEndEarlyGravityModifier = 3;
        //
        //
        // private float _apexPoint;
        //  private bool _coyoteUsable;
        // // private bool _endedJumpEarly = true;
        // // private float _apexPoint; // Becomes 1 at the apex of a jump
        //  private float _lastJumpPressed;
        //  private bool CanUseCoyote => _coyoteUsable && !_colDown && _timeLeftGrounded + _coyoteTimeThreshold > Time.time;
        //  private bool HasBufferedJump => _colDown && _lastJumpPressed + _jumpBuffer > Time.time;

        //  private void CalculateJumpApex()
        //  {
        //      if (!_colDown)
        //      {
        //          // Gets stronger the closer to the top of the jump
        //          _apexPoint = Mathf.InverseLerp(_ReverseApexThreshold, 0, Mathf.Abs(Velocity.y));
        //          _fallSpeed = Mathf.Lerp(_minReverseSpeed, _maxReverseSpeed, _apexPoint);
        //      }
        //      else
        //      {
        //          _apexPoint = 0;
        //      }
        //  }

        [Header("REVERSE")]
        [SerializeField] private float Re_HoSpeed = 1;
        private float Curr_Re_HoSpeed = 1f;

        private void CaculateReverse()
        {
            //if (!GainReverse) return;

            //Jump if: grounded or within coyote threshold || sufficient jump buffer
            if (gainReverse && Input.Reverse && CanReverse && !isDashing)
            {
                if (FallThisFrame_Last)
                {
                    FallThisFrame_Last = false;
                }
                //_currentHorizontalSpeed = 0;
                Curr_Re_HoSpeed = Re_HoSpeed;
                //_currentVerticalSpeed = _jumpHeight;
                GameManager.Instance.isReverse = GameManager.Instance.isReverse ? false : true;
                //_endedJumpEarly = false;
                // _timeLeftGrounded = float.MinValue;
                // _coyoteUsable = false;
                CanReverse = false;
                ReverseThisFrame = true;
                //JumpingThisFrame = true;

                ////相机抖动
                //impulseSourceY.GenerateImpulse();
            }
            else
            {
                ReverseThisFrame = false;
            }

            // End the jump early if button released
            //  if (!_colDown && Input.JumpUp && !_endedJumpEarly && Velocity.y > 0) {
            //      // _currentVerticalSpeed = 0;
            //      _endedJumpEarly = true;
            //  }
            //
            // if (_colUp) {
            //     if (_currentVerticalSpeed > 0) _currentVerticalSpeed = 0;
            // }

            //播放翻转音效
            if (ReverseThisFrame)
                AllSceneMusicManager.Instance.PlaySound("reverse");

        }
        #endregion


        #region Move

        [Header("MOVE")]
        [SerializeField, Tooltip("Raising this value increases collision accuracy at the cost of performance.")]
        private int _freeColliderIterations = 10;

        // We cast our bounds before moving to avoid future collisions
        private void MoveCharacter()
        {
            var pos = transform.position;
            RawMovement = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed); // Used externally
            //RawMovement_Dash = new Vector3(_currentHorizontalSpeed_Dash, _currentVerticalSpeed_Dash);
            var move = RawMovement * Time.deltaTime;
            var furthestPoint = pos + move + move;

            // check furthest movement. If nothing hit, move and don't do extra checks
            var hit = Physics2D.OverlapBox(furthestPoint, _characterBounds.size, 0, _groundLayer);
            if (!hit)
            {
                transform.position += move;
                return;
            }

            //otherwise increment away from current pos; see what closest position we can move to

            var positionToMoveTo = transform.position;
            for (int i = 1; i <= _freeColliderIterations; i++)
            {
                // increment to check all but furthestPoint - we did that already
                var t = (float)i / _freeColliderIterations;
                var posToTry = Vector2.Lerp(pos, furthestPoint, t);

                if (Physics2D.OverlapBox(posToTry, _characterBounds.size, 0, _groundLayer))
                {
                    transform.position = positionToMoveTo;

                    // We've landed on a corner or hit our head on a ledge. Nudge the player gently
                    if (i == 1)
                    {
                        //if (_currentVerticalSpeed < 0 && !_colUp) _currentVerticalSpeed = 0;
                        //var dir = transform.position - hit.transform.position;
                        //if (_playerAnimator.transform.localScale.x == 1)
                        //{
                        //    transform.position += dir.normalized * move.magnitude;
                        //}
                        //else
                        //{
                        //    transform.position -= dir.normalized * move.magnitude;
                        //}

                        if (!GameManager.Instance.isReverse)
                        {
                            if (_currentVerticalSpeed < 0 && !_colUp) _currentVerticalSpeed = 0;
                            var dir = transform.position - hit.transform.position;
                            transform.position += dir.normalized * move.magnitude;
                        }
                        else
                        {
                            if (_currentVerticalSpeed > 0 && !_colDown) _currentVerticalSpeed = 0;
                            var dir = transform.position - hit.transform.position;
                            transform.position += dir.normalized * move.magnitude;
                        }

                        if(aaa)
                        {
                            Debug.LogError("zhuang");
                        }
                    }

                    if (aaa)
                    {
                        Debug.LogError("qqq");
                    }
                    return;
                }

                positionToMoveTo = posToTry;
            }

           // transform.position = positionToMoveTo;
        }

        #endregion

        #region Enchanting
        [Header("Enchant")]
        public int enchant_Value;
        public bool is_Enchant;
        [SerializeField] private float enchant_Time;
        [SerializeField] private float enchant_Time_Stay = 5f;
        [SerializeField] public bool enchant_hot;
        [SerializeField] public bool enchant_cold;
        private void Caculate_Enchanting()
        {
            if (!is_Enchant)
            {
                if (enchant_Value != 0)
                {
                    enchant_Time += Time.deltaTime;
                    if (enchant_Time >= enchant_Time_Stay)
                    {
                        enchant_Value = 0;
                        enchant_Time = 0;
                        //快速消除过热/过冷屏幕效果
                        FreezePanel.Instance.QuickFade();
                        OverheatPanel.Instance.QuickFade();
                    }
                }

            }
            else
            {
                enchant_Time = 0;
            }
            if (enchant_Value>= 60 || enchant_Value <= -60)
            {
                GameManager_global.GetInstance().gameData_SO.ach_2_Overload.UpdateMe(1);
                Die();
            }

            enchant_hot = enchant_Value >= 10 ? true : false;
            enchant_cold = enchant_Value <= -10 ? true : false;
            if (enchant_hot)
            {
                material.EnableKeyword("OUTBASE_ON");
                material.DisableKeyword("ALPHAOUTLINE_ON");
            }
            else if (enchant_cold)
            {
                material.EnableKeyword("ALPHAOUTLINE_ON");
                material.DisableKeyword("OUTBASE_ON");
            }
            else
            {
                material.DisableKeyword("ALPHAOUTLINE_ON");
                material.DisableKeyword("OUTBASE_ON");
            }

            //过冷、过热的屏幕效果
            ////附魔值40~60对应透明度0~185/255
            //附魔值30~60对应透明度0~185/255
            if (enchant_Value > 30)
            {
                Debug.Log("OverheatPanel");
                //OverheatPanel.Instance.SetAlpha((enchant_Value-30) * 0.037f);
                OverheatPanel.Instance.SetAlpha((enchant_Value - 30) * 0.025f);
                //OverheatPanel.Instance.SetAlpha((enchant_Value - 30) * 0.033f);
            }
            else if(enchant_Value < -30)
            {
                Debug.Log("FreezePanel");
                //FreezePanel.Instance.SetAlpha((-0 - enchant_Value) * 0.049f);
                //FreezePanel.Instance.SetAlpha((-30 - enchant_Value) * 0.025f);
                //FreezePanel.Instance.SetAlpha((-30 - enchant_Value) * 0.015f);
                FreezePanel.Instance.SetAlpha((-30 - enchant_Value) * 0.033f);
            }
            else
            {
                FreezePanel.Instance.SetAlpha(0);
                OverheatPanel.Instance.SetAlpha(0);
            }
        }



        #endregion

        #region Boss
        public GameObject Boss;
        public float Boss_min;
        public float Boss_max;

        public void Boss_Positon()
        {
            if (Boss == null)
            {
                return;
            }
            if (Boss.GetComponent<Boss>() != null)
            {
                Boss.GetComponent<Boss>().minX = Boss_min;
                Boss.GetComponent<Boss>().maxX = Boss_max;
                //   return;
            }
            if (Boss.GetComponent<Boss_2>() != null)
            {
                Boss.GetComponent<Boss_2>().minX = Boss_min;
                Boss.GetComponent<Boss_2>().maxX = Boss_max;
            }
        }
        public void Boss_End()

        {
            if (Boss == null)
            {
                return;
            }

            Boss.SetActive(false);
        }
        #endregion

        #region CalculateEnv

        private void Caculate_enemy(bool up, bool down, bool left, bool right)
        {
            if (up || down || left || right)
            {
                Die();
            }
            //  SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


        private void Caculate_ReverseGround(bool up, bool down, bool left, bool right)
        {
            if ((up && _colDown) || (down && _colUp))
            {
                Die();
            }
            //  SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        #endregion

        private void Caculate_MovingGround(bool up, bool down, bool left, bool right)
        {
            if (up || down || left || right)
            {
                transform.parent = MovingGround.transform;
            }
            else
            {
                transform.parent = Parent;
            }
        }


        private void Calculate_Ice(bool up, bool down, bool left, bool right)
        {
            if (enchant_hot && isDashing)
            {
                if (up)
                {
                    if (currDirection_Y > 0)
                    {
                        Debug.Log(1);
                        Invoke("Destroy_Ice", 0.1f);
                    }
                }
                else if (down)
                {
                    if (currDirection_Y < 0)
                    {
                        Debug.Log(2);
                        Invoke("Destroy_Ice", 0.1f);
                    }
                }
                else if (left)
                {
                    if (currDirection_X < 0)
                    {
                        Debug.Log(3);
                        Invoke("Destroy_Ice", 0.1f);
                    }
                }
                else if (right)
                {
                    if (currDirection_X > 0)
                    {
                        Debug.Log(4);
                        Invoke("Destroy_Ice", 0.1f);
                    }
                }
            }




        }
        private void Destroy_Ice()
        {
            GameManager_global.GetInstance().gameData_SO.ach_6_FirstEnchant.UpdateMe(1);
            Destroy(Ice);
        }

        private void Calculate_Cake(bool up, bool down, bool left, bool right)
        {
            if (up || down || left || right)
            {
                Cake.GetComponent<Env_Cake>().isOn = true;
            }
        }


        #region Die

        [Header("Death")]
        // 是否处于死亡状态
        public bool isDead;
        // 死亡动画是否完成
        public bool isDeathAnimFinish;

        public Transform rebirth;

        private float Reverse_color;

        public void Die()
        {
            impulseSourceXY.GenerateImpulse();

            if (isDead) return;
            //死亡时禁用Update：操作、移动
            isDead = true;
            MusicManager.GetInstance().PlaySound("dead", true);
            //隐藏暂停
            UIManager.GetInstance().GetPanel<PausePanel>("PausePanel").HideMe();
            //相机抖动
            impulseSourceXY.GenerateImpulse();
            StartCoroutine(Co_Die());
        }
        
        public IEnumerator Co_Die()
        {
            //关闭拖尾渲染器
            trailRenderer.enabled = false;

            //播放死亡动画和特效（花1.0秒）
            StartCoroutine(PlayDeathDissolve());

            //从外向内播放圆圈（花1.0秒）
            yield return deathCircle.PlayCircleOutToIn();

            material.DisableKeyword("FADE_ON");
            material.SetFloat("_FadeAmount", 0);

            //TODO
            //做法1.死亡后重载场景
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            ////做法2.重新设置Player的位置
            //OnDeathAnimFinish();
            ////（假装要花0.1秒）
            //yield return new WaitForSeconds(0.1f);

            //// 显示Player 重设死亡状态
            //OnRebirthAnimFinish();

            ////TODO [可选]播放重生动画和特效

            ////恢复正常重力
            //GameManager.Instance.isReverse = Rebirth_Reverse;
            //Reverse_color = Rebirth_Reverse ? 1f : 0f;
            //anim.SetLayerWeight(1, Reverse_color);

            ////从内向外播放圆圈
            //yield return deathCircle.PlayCircleInToOut();

            ////开启拖尾渲染器
            //trailRenderer.enabled = true;
        }

        public void EndOfGame()
        {
            StartCoroutine(Co_EndofGame());
        }
        public IEnumerator Co_EndofGame()
        {
            //TODO 剧情演出

            GameManager_global.GetInstance().gameData_SO.UpdateLevelRecords();

            GameManager_global.GetInstance().gameData_SO.ach_3_Pass.UpdateMe(1);

            //yield return new WaitForSeconds(1.5f);

            //从外向内播放圆圈（花2.0秒）
            yield return deathCircle.PlayCircleOutToIn(2.0f);

            //场景
            SceneManager.LoadScene(GameData_SO.AfterLoginScene);
        }

        //死亡的消融dissolve特效
        public IEnumerator PlayDeathDissolve()
        {
            material.EnableKeyword("FADE_ON");
            material.SetFloat("_FadeAmount", 0);
            DOTween.To(() => material.GetFloat("_FadeAmount"), x => material.SetFloat("_FadeAmount", x), 1f, 1f).onComplete += () =>
            {
                // 隐藏Player(Sprite)
                Hide();
            };
            yield return new WaitForSeconds(1f); //TODO:优化
        }

        //重生的消融dissolve特效
        public void PlayRebirthDissolve()
        //public IEnumerator PlayRebirthDissolve()
        {
            material.EnableKeyword("FADE_ON");
            material.SetFloat("_FadeAmount", 1);
            DOTween.To(() => material.GetFloat("_FadeAmount"), x => material.SetFloat("_FadeAmount", x), 0f, 1f).onComplete += () =>
            {
                material.DisableKeyword("FADE_ON");
            };
            //yield return new WaitForSeconds(1f); //TODO:优化
        }

        public void temp_DeathAndRebirth()
        {
            OnDeathAnimFinish();

            OnRebirthAnimFinish();
        }

        public void OnDustDestroy()
        {
            if (!isDeathAnimFinish)
            {
                OnDeathAnimFinish();
            }
            else
            {
                OnRebirthAnimFinish();
            }
        }

        public void OnDeathAnimFinish()
        {
            transform.position = rebirth.position;
            // 产生Rebirth动画预制体（播放完成后自动销毁）
            isDeathAnimFinish = true;
        }

        public void OnRebirthAnimFinish()
        {
            // 显示Player
            Show();
            // 重设死亡状态
            isDead = false;
            isDeathAnimFinish = false;
        }
        #endregion


        #region Show and Hide player sprite

        [Header("Show and Hide sprite")]
        // 玩家是否暂停
        public bool isPaused;

        // 玩家是否在暂停的基础上进行了隐藏
        // 如果玩家是隐藏的，那么必然也是暂停的
        public bool isHidden { get; private set; }

        // 隐藏Player(Sprite)
        public void Hide()
        {
            if (isHidden) return;
            var srList = GetComponentsInChildren<SpriteRenderer>();
            foreach (var sr in srList)
            {
                sr.enabled = false;
            }

            isPaused = true;
            isHidden = true;
        }

        // 显示Player(Sprite)
        public void Show()
        {
            if (!isHidden) return;
            var srList = GetComponentsInChildren<SpriteRenderer>();
            foreach (var sr in srList)
            {
                sr.enabled = true;
            }

            isPaused = false;
            isHidden = false;
        }
        #endregion
    }
}