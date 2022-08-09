using UnityEngine;
using Random = UnityEngine.Random;

namespace TarodevController {
    /// <summary>
    /// This is a pretty filthy script. I was just arbitrarily adding to it as I went.
    /// You won't find any programming prowess here.
    /// This is a supplementary script to help with effects and animation. Basically a juice factory.
    /// </summary>
    public class PlayerAnimator : MonoBehaviour {
        [SerializeField] private Animator _anim;
        [SerializeField] private AudioSource _source;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private ParticleSystem _jumpParticles, _launchParticles;
        [SerializeField] private ParticleSystem _moveParticles, _landParticles;
        [SerializeField] private AudioClip[] _footsteps;
        [SerializeField] private float _maxTilt = .1f;
        [SerializeField] private float _tiltSpeed = 1;
        [SerializeField, Range(1f, 3f)] private float _maxIdleSpeed = 2;
        [SerializeField] private float _maxParticleFallSpeed = -40;

        private PlayerController _player;
        private bool _playerGrounded;
        private ParticleSystem.MinMaxGradient _currentGradient;
        private Vector2 _movement;

        private Vector3 x1 = new Vector3(1,1,1);
        private Vector3 x2 = new Vector3(-1, 1, 1);
        private Vector3 y1 = new Vector3(1, 1, 1);
        private Vector3 y2 = new Vector3(1, -1, 1);

        //public GameObject go;

        void Awake() => _player = GetComponentInParent<PlayerController>();

        void Update() {
            if (_player == null) return;
         
            if (_player.Dashed)
            {
                _anim.SetBool(DashKey,true);
                return;
            }
            else
            {
                _anim.SetBool(DashKey, false);
            }
            if (_player.Input.X != 0) transform.localScale = _player.Input.X > 0 ? x1 : x2; 
            if (_player.ReverseThisFrame) _anim.SetTrigger(ReverseKey);
            _player.gameObject.transform.localScale = !GameManager.Instance.isReverse ? y1 : y2;
            // Flip the sprite
          
            //
            //   if ((_player._currentVerticalSpeed < 0 && !GameManager.Instance..isReverse)|| (_player._currentVerticalSpeed > 0 && GameManager.Instance..isReverse))
            //   {
            //       _anim.SetBool(FallKey, true);
            //   }
            //   else
            //   {
            //       _anim.SetBool(FallKey, false);
            //   }
            _anim.SetFloat(IdleSpeedKey, Mathf.Lerp(0, 1, Mathf.Abs(_player.Input.X)));

            // Lean while running
            //var targetRotVector = new Vector3(0, 0, Mathf.Lerp(-_maxTilt, _maxTilt, Mathf.InverseLerp(-1, 1, _player.Input.X)));
            //_anim.transform.rotation = Quaternion.RotateTowards(_anim.transform.rotation, Quaternion.Euler(targetRotVector), _tiltSpeed * Time.deltaTime);

            // Speed up idle while running
            //_anim.SetFloat(IdleSpeedKey, Mathf.Lerp(1, _maxIdleSpeed, Mathf.Abs(_player.Input.X)));
            
            if (_player.LandingThisFrame) {
                _anim.SetTrigger(GroundedKey);
            _anim.ResetTrigger(FallKey);
           
                //_anim.ResetTrigger(JumpKey);
                // _anim.ResetTrigger(FallKey);

                //_source.PlayOneShot(_footsteps[Random.Range(0, _footsteps.Length)]);
            }
   
            if (_player.JumpingThisFrame)
            {
                _anim.SetTrigger(JumpKey);
                _anim.ResetTrigger(GroundedKey);
              
            }
            if (_player.FallThisFrame)
            {
                _anim.SetTrigger(FallKey);
                _anim.ResetTrigger(GroundedKey);
                
            }
          
            // Jump effects

            //
            //   // Play landing effects and begin ground movement effects
            //   if (!_playerGrounded && _player.Grounded) {
            //       _playerGrounded = true;
            //       _moveParticles.Play();
            //       _landParticles.transform.localScale = Vector3.one * Mathf.InverseLerp(0, _maxParticleFallSpeed, _movement.y);
            //       SetColor(_landParticles);
            //       _landParticles.Play();
            //   }
            //   else if (_playerGrounded && !_player.Grounded) {
            //       _playerGrounded = false;
            //       _moveParticles.Stop();
            //   }
            //
            // Detect ground color
            // var groundHit = Physics2D.Raycast(transform.position, Vector3.down, 2, _groundMask);
            // if (groundHit && groundHit.transform.TryGetComponent(out SpriteRenderer r)) {
            //     _currentGradient = new ParticleSystem.MinMaxGradient(r.color * 0.9f, r.color * 1.2f);
            //     SetColor(_moveParticles);
            // }
            //
            // _movement = _player.RawMovement; // Previous frame movement is more valuable
        }

        #region Animation Keys

        private static readonly int GroundedKey = Animator.StringToHash("Grounded");
        private static readonly int IdleSpeedKey = Animator.StringToHash("IdleSpeed");
        private static readonly int JumpKey = Animator.StringToHash("Jump");
        private static readonly int ReverseKey = Animator.StringToHash("Reverse");
        private static readonly int DashKey = Animator.StringToHash("Dash");
        private static readonly int FallKey = Animator.StringToHash("Fall");

        #endregion
    }
}