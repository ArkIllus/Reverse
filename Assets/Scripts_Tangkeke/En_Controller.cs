using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class En_Controller : MonoBehaviour
{
    //public GameManager _gameManager;
    [SerializeField] private float _currentVerticalSpeed;
    private bool _active;
    private Vector3 RawMovement;


    void Awake()
    {
        Invoke(nameof(Activate), 0.5f);

        
    }
    void Activate() => _active = true;


    private void Update()
    {
        if (!_active) return;
        RunCollisionChecks();
        CalculateGravity();
        MoveCharacter();
    }





    [Header("COLLISION")][SerializeField] private Bounds _characterBounds;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private int _detectorCount = 3;
    [SerializeField] private float _detectionRayLength = 0.1f;
    [SerializeField][Range(0.1f, 0.3f)] private float _rayBuffer = 0.1f; // Prevents side detectors hitting the ground

    private RayRange _raysUp, _raysRight, _raysDown, _raysLeft;
    [SerializeField] private bool _colUp, _colRight, _colDown, _colLeft;


    private void RunCollisionChecks()
    {
        // Generate ray ranges. 
        CalculateRayRanged();


        var groundedCheck = RunDetection(_raysDown);
        var groundedCheck_Re = RunDetection(_raysUp);









        _colDown = groundedCheck;

        // The rest
        _colUp = RunDetection(_raysUp);
        _colLeft = RunDetection(_raysLeft);
        _colRight = RunDetection(_raysRight);

        bool RunDetection(RayRange range)
        {
            return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _groundLayer));

        }



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

        // if (!Application.isPlaying) return;
        //
        // // Draw the future position. Handy for visualizing gravity
        // Gizmos.color = Color.red;
        // var move = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed) * Time.deltaTime;
        // Gizmos.DrawWireCube(transform.position + move, _characterBounds.size);
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



    #region Gravity

    [Header("GRAVITY")][SerializeField] private float _fallClamp = -40f;
    [SerializeField] private float _fallClampReverse = 40f;
   // [SerializeField] private float _minReverseSpeed = 80f;
   // [SerializeField] private float _maxReverseSpeed = 120f;
    private float _fallSpeed;

    private void CalculateGravity()
    {
        if (!GameManager.Instance.isReverse)
        {
            if (_colDown)
            {
                // Move out of the ground
                if (_currentVerticalSpeed < 0) _currentVerticalSpeed = 0;

            }
            else
            {
                // Add downward force while ascending if we ended the jump early
                // var fallSpeed = _endedJumpEarly && _currentVerticalSpeed > 0 ? _fallSpeed * _jumpEndEarlyGravityModifier : _fallSpeed;

                _fallSpeed = 30f;


                // Fall
                _currentVerticalSpeed -= _fallSpeed * Time.deltaTime;
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

                _fallSpeed = 30f;


                // Fall
                _currentVerticalSpeed += _fallSpeed * Time.deltaTime;
                // Debug.Log(_currentVerticalSpeed);

                // Clamp
                if (_currentVerticalSpeed > _fallClampReverse) _currentVerticalSpeed = _fallClampReverse;
            }
        }
    }

    #endregion




    #region Move

    [Header("MOVE")]
    [SerializeField, Tooltip("Raising this value increases collision accuracy at the cost of performance.")]
    private int _freeColliderIterations = 10; //Œ¥ π”√

    // We cast our bounds before moving to avoid future collisions
    private void MoveCharacter()
    {
        var pos = transform.position;
        RawMovement = new Vector3(0f, _currentVerticalSpeed); // Used externally
        var move = RawMovement * Time.deltaTime;
        var furthestPoint = pos + move;

        // check furthest movement. If nothing hit, move and don't do extra checks
        var hit = Physics2D.OverlapBox(furthestPoint, _characterBounds.size, 0, _groundLayer);
        if (!hit)
        {
            transform.position += move;
            return;
        }

        // otherwise increment away from current pos; see what closest position we can move to
      //var positionToMoveTo = transform.position;
      //for (int i = 1; i < _freeColliderIterations; i++)
      //{
      //    // increment to check all but furthestPoint - we did that already
      //    var t = (float)i / _freeColliderIterations;
      //    var posToTry = Vector2.Lerp(pos, furthestPoint, t);
      //
      //    if (Physics2D.OverlapBox(posToTry, _characterBounds.size, 0, _groundLayer))
      //    {
      //        transform.position = positionToMoveTo;
      //
      //        // We've landed on a corner or hit our head on a ledge. Nudge the player gently
      //        if (i == 1)
      //        {
      //            if (_currentVerticalSpeed < 0) _currentVerticalSpeed = 0;
      //            var dir = transform.position - hit.transform.position;
      //            transform.position += dir.normalized * move.magnitude;
      //        }
      //
      //        return;
      //    }
      //
      //    positionToMoveTo = posToTry;
      //}
    }

    #endregion



}
