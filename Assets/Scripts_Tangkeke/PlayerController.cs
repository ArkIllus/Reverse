using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace TarodevController {
    /// <summary>
    /// Hey!
    /// Tarodev here. I built this controller as there was a severe lack of quality & free 2D controllers out there.
    /// Right now it only contains movement and jumping, but it should be pretty easy to expand... I may even do it myself
    /// if there's enough interest. You can play and compete for best times here: https://tarodev.itch.io/
    /// If you hve any questions or would like to brag about your score, come to discord: https://discord.gg/GqeHHnhHpz
    /// </summary>
    public class PlayerController : MonoBehaviour, IPlayerController {
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

        public bool Grounded => _colDown;

        public bool Dashed { get; private set; }

        private Vector3 _lastPosition;
        public  float _currentHorizontalSpeed;
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

        // 结束冲刺状态的计时器
        public float endTimer = 1f;
        public float endTimer_2;

        // 非正常结束(提前结束冲刺)
        public bool isAdvancedEnd;

        public bool CanReverse;

        //Cinemachine
        private Cinemachine.CinemachineImpulseSource impulseSource;

        void Awake()
        {
            Invoke(nameof(Activate), 0.5f);

            _playerAnimator = GetComponentInChildren<PlayerAnimator>();
            dashWave = GetComponentInChildren<DashWave>();
            impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
        }

        void Activate() =>  _active = true;

        //private void OnEnable()
        //{
        //    GameManager.Instance.player = this;
        //}
        //private void OnDisable()
        //{
        //    GameManager.Instance.player = null;
        //}

        private void Update() {
            if(!_active) return;
            // Calculate velocity
            Velocity = (transform.position - _lastPosition) / Time.deltaTime;
            _lastPosition = transform.position;

            GatherInput();
           // Debug.Log(UnityEngine.Input.GetAxisRaw("Horizontal"));
            RunCollisionChecks();
            CalulateDash();

            CalculateWalk(); // Horizontal movement
            CaculateReverse();
            CalculateJumpApex(); // Affects fall speed, so calculate before gravity
            CalculateGravity(); // Vertical movement
            CalculateJump(); // Possibly overrides vertical

            MoveCharacter(); // Actually perform the axis movement
        }

        #region Gather Input

        private void GatherInput() {
            Input = new FrameInput {
                JumpDown = UnityEngine.Input.GetButtonDown("Jump"),
                JumpUp = UnityEngine.Input.GetButtonUp("Jump"),
                X = UnityEngine.Input.GetAxisRaw("Horizontal"),
                Y = UnityEngine.Input.GetAxisRaw("Vertical"),
                Dash = UnityEngine.Input.GetButtonDown("Dash"),
                Reverse = UnityEngine.Input.GetButtonDown("Reverse")
            };
           if (Input.JumpDown) {
               _lastJumpPressed = Time.time;
           }
        }

        private Vector2Int GetHVinput()
        {
            int xInput = 0;
            int yInput = 0;
            if (Mathf.Abs(UnityEngine.Input.GetAxisRaw("Horizontal")) >thresHold)
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

        [Header("COLLISION")] [SerializeField] private Bounds _characterBounds;
        [SerializeField] private LayerMask _groundLayer;
        //[SerializeField] private LayerMask _otherLayer;
        // [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private int _detectorCount = 3;
        [SerializeField] private float _detectionRayLength = 0.1f;
        [SerializeField] [Range(0.1f, 0.3f)] private float _rayBuffer = 0.1f; // Prevents side detectors hitting the ground

        private RayRange _raysUp, _raysRight, _raysDown, _raysLeft;
        [SerializeField]public bool _colUp, _colRight, _colDown, _colLeft;
        //[SerializeField] private bool _colUp_oneway, _colRight_oneway, _colDown_oneway, _colLeft_oneway;

        private float _timeLeftGrounded;

        // We use these raycast checks for pre-collision information
        private void RunCollisionChecks() {
            // Generate ray ranges. 
            CalculateRayRanged();
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



    //  _colDown_oneway = RunDetection_other(_raysDown);
    //  _colUp_oneway = RunDetection_other(_raysUp);
    //  _colLeft_oneway = RunDetection_other(_raysLeft);  
    //  _colRight_oneway = RunDetection_other(_raysRight);
    //  

         bool RunDetection(RayRange range) {
                return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _groundLayer));
            }
       // bool RunDetection_other(RayRange range)
       // {
       //     return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength,_otherLayer));
       // }
            //  bool RunDetection_oneway(RayRange range)
            //  {
            //
            //        return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _ground_onewayLayer) ? Physics2D.Raycast(point, range.Dir, _detectionRayLength, _ground_onewayLayer).collider.gameObject.CompareTag("oneway"):false);
            //
            //
            //
            //
            //
            //    }
            //
            //  bool DamageDetection(RayRange range)
            //  {
            //      return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _enemyLayer));
            //  }


        }

        private void CalculateRayRanged() {
            // This is crying out for some kind of refactor. 
            var b = new Bounds(transform.position, _characterBounds.size);

            _raysDown = new RayRange(b.min.x + _rayBuffer, b.min.y, b.max.x - _rayBuffer, b.min.y, Vector2.down);
            _raysUp = new RayRange(b.min.x + _rayBuffer, b.max.y, b.max.x - _rayBuffer, b.max.y, Vector2.up);
            _raysLeft = new RayRange(b.min.x, b.min.y + _rayBuffer, b.min.x, b.max.y - _rayBuffer, Vector2.left);
            _raysRight = new RayRange(b.max.x, b.min.y + _rayBuffer, b.max.x, b.max.y - _rayBuffer, Vector2.right);
        }


        private IEnumerable<Vector2> EvaluateRayPositions(RayRange range) {
            for (var i = 0; i < _detectorCount; i++) {
                var t = (float)i / (_detectorCount - 1);
                yield return Vector2.Lerp(range.Start, range.End, t);
            }
        }

        private void OnDrawGizmos() {
            // Bounds
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + _characterBounds.center, _characterBounds.size);

            // Rays
            if (!Application.isPlaying) {
                CalculateRayRanged();
                Gizmos.color = Color.blue;
                foreach (var range in new List<RayRange> { _raysUp, _raysRight, _raysDown, _raysLeft }) {
                    foreach (var point in EvaluateRayPositions(range)) {
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
        private float currDirection_X =0;
        private float currDirection_Y =0;
        private float currDashSpeed = 0;
        private float _currentHorizontalSpeed_Dash = 0;
        private float _currentVerticalSpeed_Dash = 0;
        private void CalulateDash()
        {
            if (isDashing)
            {
                endTimer += Time.deltaTime;
                //currDashSpeed *= Mathf.Lerp(1, 0, endTimer_2 / duration_2);
                ////currDashSpeed = 0;
                //_currentHorizontalSpeed = currDashSpeed * currDirection_X;
                //_currentVerticalSpeed = currDashSpeed * currDirection_Y;

            }
            if (endTimer > (duration_1 - duration_2)) {
                endTimer_2 += Time.deltaTime;
                isDashing_Over = true;
                // _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, _deAcceleration * Time.deltaTime);
                _currentHorizontalSpeed -= currDashSpeed * currDirection_X * Time.deltaTime ;
                //_currentVerticalSpeed += (currDashSpeed * currDirection_Y * -1 * Mathf.Lerp(1, 0, endTimer_2 / duration_2));
                _currentVerticalSpeed -= currDashSpeed * currDirection_Y * Time.deltaTime;
            }
                
            if ((endTimer >= duration_1 && isDashing)) { 
                isAdvancedEnd = false;
                _currentHorizontalSpeed = 0;
                _currentVerticalSpeed = 0;
                isDashing = false;
                Dashed = false;
                endTimer = 0;   
            }
            // Debug.Log(currDashSpeed);
            if (!Input.Dash||dashCount<=0||isDashing)
            {
                dashWave.DashUpdate(false);
                return;
            }

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
            _currentHorizontalSpeed = currDashSpeed * direction.x;
            _currentVerticalSpeed = currDashSpeed * direction.y;

            //相机抖动
            //[DOTween]使用cinemachine相机下，其Transform组件不可被代码修改
            //Camera.main.transform.DOComplete();
            //var tmp = Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
            //tmp.onComplete += () => { print("相机抖动完成"); };
            //[Cinemachine]
            impulseSource.GenerateImpulse();
            //impulseSource.GenerateImpulse(Camera.main.transform.forward);

            //播放冲刺屏幕波纹特效
            //FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));
            dashWave.DashUpdate(true);
        }
        #endregion


        #region Walk

        [Header("WALKING")] [SerializeField] private float _acceleration = 90;
        [SerializeField] private float _moveClamp = 13;
        [SerializeField] private float _deAcceleration = 60f;
        [SerializeField] private float _apexBonus = 2;

        private void CalculateWalk() {
            if (Input.X != 0) {
                // Set horizontal move speed
                if (isDashing_Over)
                {
                    _currentHorizontalSpeed += Input.X * _acceleration * Time.deltaTime  ;

                    // clamped by max frame movement

                    _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -_moveClamp *Curr_Re_HoSpeed, _moveClamp * Curr_Re_HoSpeed);
                }
                  

                // Apply bonus at the apex of a jump
               // var apexBonus = Mathf.Sign(Input.X) * _apexBonus * _apexPoint;
              //  _currentHorizontalSpeed += apexBonus * Time.deltaTime;
            }
            else {
                // No input. Let's slow the character down
                if(!isDashing)
                _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, _deAcceleration * Time.deltaTime) ;
            }

            if (_currentHorizontalSpeed > 0 && _colRight || _currentHorizontalSpeed < 0 && _colLeft) {
                // Don't walk through walls
                _currentHorizontalSpeed = 0;
            }
        }
        #endregion


        #region Gravity

        [Header("GRAVITY")] [SerializeField] private float _fallClamp = -40f;
        [SerializeField] private float _fallClampReverse = 40f;
       // [SerializeField] private float _minReverseSpeed = 80f;
        //[SerializeField] private float _maxReverseSpeed = 120f;
        [SerializeField] private float _minFallSpeed = 80f;
        [SerializeField] private float _maxFallSpeed = 120f;
        private float _fallSpeed;

        private void CalculateGravity() {
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
                        if (_currentVerticalSpeed  <0&&!FallThisFrame_Last)
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
        
        [Header("JUMPING")] [SerializeField] private float _jumpHeight = 10;
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


        private void CalculateJumpApex() {
            if (!_colDown) {
                // Gets stronger the closer to the top of the jump
                _apexPoint = Mathf.InverseLerp(_jumpApexThreshold, 0, Mathf.Abs(Velocity.y));
                _fallSpeed = Mathf.Lerp(_minFallSpeed, _maxFallSpeed, _apexPoint);
            }
            else {
                _apexPoint = 0;
            }
        }
        
        private void CalculateJump() {

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
                _currentVerticalSpeed = _jumpHeight*-1;
                _endedJumpEarly = false;
                _coyoteUsable = false;
                _timeLeftGrounded = float.MinValue;
                // JumpingThisFrame = true;
            }
            else
            {
                // JumpingThisFrame = false;
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
             //Jump if: grounded or within coyote threshold || sufficient jump buffer
            if (Input.Reverse&&CanReverse&&!isDashing) {
                //_currentHorizontalSpeed = 0;
                Curr_Re_HoSpeed = Re_HoSpeed;
                //_currentVerticalSpeed = _jumpHeight;
                GameManager.Instance.isReverse = GameManager.Instance.isReverse?false:true;
                //_endedJumpEarly = false;
                // _timeLeftGrounded = float.MinValue;
                // _coyoteUsable = false;
                CanReverse = false;
                ReverseThisFrame = true;
                //JumpingThisFrame = true;
            }                  
            else {
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
        }
        #endregion


        #region Move

        [Header("MOVE")] [SerializeField, Tooltip("Raising this value increases collision accuracy at the cost of performance.")]
        private int _freeColliderIterations = 10;

        // We cast our bounds before moving to avoid future collisions
        private void MoveCharacter() {
            var pos = transform.position;
            RawMovement = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed); // Used externally
            //RawMovement_Dash = new Vector3(_currentHorizontalSpeed_Dash, _currentVerticalSpeed_Dash);
            var move = RawMovement * Time.deltaTime;
            var furthestPoint = pos + move;

            // check furthest movement. If nothing hit, move and don't do extra checks
            var hit = Physics2D.OverlapBox(furthestPoint, _characterBounds.size, 0, _groundLayer);
            if (!hit) {
                transform.position += move;
                return;
            }

            //otherwise increment away from current pos; see what closest position we can move to
         
      var positionToMoveTo = transform.position;
      for (int i = 1; i < _freeColliderIterations; i++) {
          // increment to check all but furthestPoint - we did that already
          var t = (float)i / _freeColliderIterations;
          var posToTry = Vector2.Lerp(pos, furthestPoint, t);
     
          if (Physics2D.OverlapBox(posToTry, _characterBounds.size, 0, _groundLayer)) {
              transform.position = positionToMoveTo;
     
              // We've landed on a corner or hit our head on a ledge. Nudge the player gently
              if (i == 1) {
                 if (_currentVerticalSpeed < 0&&!_colUp) _currentVerticalSpeed = 0;
                  var dir = transform.position - hit.transform.position;
                  transform.position += dir.normalized * move.magnitude;
              }
     
              return;
          }
     
          positionToMoveTo = posToTry;
      }
        }

        #endregion


        #region Die

        [Header("Death")]
        // 是否处于死亡状态
        public bool isDead;
        // 死亡动画是否完成
        public bool isDeathAnimFinish;

        public void Die()
        {
            if (isDead) return;
            Debug.Log("Die");
            //TODO 死亡无法移动
            isDead = true;

            // 隐藏Player(Sprite)
            Hide();

            // 产生Death动画预制体（播放完后自动销毁）
            //var dust = S_Dust_Factory.Instance.CreateDust(transform.position);
            //dust.AddObserver(this);

            //TODO 完善死亡效果
            temp_DeathAndRebirth();

            // 相机抖动
            //S_MainCamera.Instance.Shake(C_CameraShake.ShakeType.Die);

            //TODO 死亡后重载场景 or ...
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            //恢复正常重力
            GameManager.Instance.isReverse = false;
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
            Debug.Log("OnDeathAnimFinish");
            // 重新设置Player的位置
            transform.position = GameManager.Instance.playerRebirthPlace.position;
            //m_Rigidbody2DWrapper.Resume();
            // 产生Rebirth动画预制体（播放完成后自动销毁）
            //var dust = S_Dust_Factory.Instance.CreateDust(transform.position);
            //dust.AddObserver(this);
            isDeathAnimFinish = true;
        }

        public void OnRebirthAnimFinish()
        {
            Debug.Log("OnRebirthAnimFinish");
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