using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

 public  class Env_Manager : MonoBehaviour
{
  
    private bool _active;
    public float _currentHorizontalSpeed;
    public float _currentVerticalSpeed;
    //public BoxCollider2D box;
    
    virtual public void Awake()
    {
        Invoke(nameof(Activate), 0.5f);
        //box = GetComponent<BoxCollider2D>();
    }

    void Activate() => _active = true;
  
    virtual public void Update()
    {
        if (!_active) return;
        // Calculate velocity
        // Velocity = (transform.position - _lastPosition) / Time.deltaTime;
        // _lastPosition = transform.position;

        //  GatherInput();
        // Debug.Log(UnityEngine.Input.GetAxisRaw("Horizontal"));
        RunCollisionChecks();
        // CalulateDash();

        //  CalculateWalk(); // Horizontal movement
        //  CaculateReverse();
        //  CalculateJumpApex(); // Affects fall speed, so calculate before gravity
        //  CalculateGravity(); // Vertical movement
        //  CalculateJump(); // Possibly overrides vertical
        //
        //  MoveCharacter(); // Actually perform the axis movement
    }

    [Header("COLLISION")][SerializeField] private Bounds _characterBounds;
    // [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _otherLayer;
    // [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private int _detectorCount = 3;
    [SerializeField] private float _detectionRayLength = 0.1f;
    [SerializeField][Range(0.1f, 0.3f)] private float _rayBuffer = 0.1f; // Prevents side detectors hitting the ground

    private RayRange _raysUp, _raysRight, _raysDown, _raysLeft;
    //[SerializeField] private bool _colUp, _colRight, _colDown, _colLeft;
    [SerializeField] public bool _colUp_other, _colRight_other, _colDown_other, _colLeft_other;

    private float _timeLeftGrounded;

    // We use these raycast checks for pre-collision information
    private void RunCollisionChecks()
    {
        // Generate ray ranges. 
        CalculateRayRanged();
        // if(DamageDetection(_raysUp)|| DamageDetection(_raysDown)|| DamageDetection(_raysLeft)|| DamageDetection(_raysRight))
        //  {
        //      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //  }
        //  LandingThisFrame = false;
        //  var groundedCheck = RunDetection(_raysDown);
        //  var groundedCheck_Re = RunDetection(_raysUp);
        //

        // if(_gameManager.isReverse && groundedCheck_Re) CanReverse = true;

        //
        //  _colDown = groundedCheck;
        //  _colUp = groundedCheck_Re;
        //  // The rest
        //  // _colUp = RunDetection(_raysUp);
        //  _colLeft = RunDetection(_raysLeft);
        //  _colRight = RunDetection(_raysRight);
        //

        _colDown_other = RunDetection_other(_raysDown);
        _colUp_other = RunDetection_other(_raysUp);
        _colLeft_other = RunDetection_other(_raysLeft);
        _colRight_other = RunDetection_other(_raysRight);

        //  bool RunDetection(RayRange range)
        //  {
        //      return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _groundLayer));
        //  }
        bool RunDetection_other(RayRange range)
        {
            return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _otherLayer));
        }
        //  bool RunDetection_oneway(RayRange range)
        //  {
        //
        //        return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _ground_onewayLayer) ? Physics2D.Raycast(point, range.Dir, _detectionRayLength, _ground_onewayLayer).collider.gameObject.CompareTag("oneway"):false);
        //    }
        //
        //  bool DamageDetection(RayRange range)
        //  {
        //      return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _enemyLayer));
        //  }
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

    public struct RayRange
    {
        public RayRange(float x1, float y1, float x2, float y2, Vector2 dir)
        {
            Start = new Vector2(x1, y1);
            End = new Vector2(x2, y2);
            Dir = dir;
        }

        public readonly Vector2 Start, End, Dir;
    }
}
