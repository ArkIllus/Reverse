using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;
using Cinemachine;

public class Boss : MonoBehaviour
{
    public PlayerController _player;
    public Transform Target;
    public CinemachineImpulseSource source;


    public float distance;
    private float current_X;

    private BoxCollider2D _boxCollider;
    private Bounds bounds;
    //private float length;

    public float minX;
    public float maxX;
    //public float center_Y;
    private Vector3 _position;
    public Animator _animator;
    public float minX_last;




    [SerializeField] float Cd = 5f;
    [SerializeField] private float endTime1;
    [SerializeField] bool isAttack;
    [SerializeField] private float endTime2;
    [SerializeField] float Stay = 3.1f;

    public GameObject hit;
    public GameObject laser;
    public GameObject aim;
    public GameObject xo;
    public GameObject Fire;
    private AudioSource _audioSource;

    //public GameObject Test;
    // Start is called before the first frame update
    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        bounds = _boxCollider.bounds;
       // length = bounds.max.x- bounds.min.x;
       // _animator = GetComponent<Animator>();
        _animator = GetComponentInChildren<Animator>();
        isAttack = false;
        _audioSource = GetComponent<AudioSource>();
        //transform.position = Target.position;
    }

    private void OnEnable()
    {
        Debug.Log("boss");
        transform.position = Target.position;
        _animator = GetComponentInChildren<Animator>();
        endTime1 = 0;
        endTime2 = 0;
        isAttack = false;
    }
    private void OnDisable()
    {
        Debug.Log(131);
        _animator = null;
        laser.SetActive(false);
        hit.SetActive(false);
        xo.SetActive(false);
        aim.SetActive(false);
        Fire.SetActive(false);
        _audioSource.Stop();
    }
    // Update is called once per frame
    void Update()
    {
        // transform.position = Vector2.MoveTowards(transform.position,_player.transform.position,10f*Time.deltaTime);
       Calculate1();
       Calculate();

    }
    private void Calculate()
    {
        // current_X = _player.transform.position.x - distance;
        // Debug.Log(Vector2.Distance(transform.position, _player.transform.position));
        // current_X = Mathf.Clamp(current_X,minX+length/2, _player.transform.position.x);

        //_position = new Vector3(current_X, _player.transform.position.y, 0);
        //  transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, 10f * Time.deltaTime);
        if (!isAttack)
        {
            if (Vector2.Distance(transform.position, Target.position) >= 0.01f)
            {
                if(Target.position.x > minX)
                {

                
                    transform.position = Vector2.MoveTowards(transform.position, Target.position, 40f * Time.deltaTime);
                }
                else
                {
                
                    _position = new Vector3(minX , _player.transform.position.y, 0);
                    transform.position = Vector2.MoveTowards(transform.position,_position, 40f * Time.deltaTime);
                }
               
                
            }
            else
            {
            
                //current_X = _player.transform.position.x - distance;
                current_X = Target.position.x;
                if (Target.position.x < minX)
                {
                  
                    _position = new Vector3(minX, _player.transform.position.y, 0);
                    transform.position = Vector2.MoveTowards(transform.position, _position, 20f * Time.deltaTime);
                }
                // Debug.Log(Vector2.Distance(transform.position, _player.transform.position));
                //current_X = Mathf.Clamp(current_X, minX, _player.transform.position.x);
                else
                {
                   
                    _position = new Vector3(current_X, _player.transform.position.y, 0);
                    transform.position = _position;
                }
                
            }
        }
       
        
            
        


        //
        minX_last = minX;
    }
    private void Calculate1()
    {
        if (transform.position.x < minX)
        {
            return;
        }
        if(!isAttack)
        endTime1 += Time.deltaTime;
        if(endTime1 > Cd&&!isAttack)
        {
           isAttack = true;
            _animator.SetBool("Attack", true);
            // hit.SetActive(true);
            // Test.SetActive(true);
            _audioSource.Play();
            Invoke("Impulse", 1);


        }
        if (isAttack)
        {
            endTime2 += Time.deltaTime;
        }
      
        if(endTime2 >= 3f)
        {
            _animator.SetBool("Attack", false);
            laser.SetActive(false);
            hit.SetActive(false);
            Fire.SetActive(false);
        }
        if (endTime2 > Stay)
        {
            endTime1 = 0;
            endTime2 = 0;
            isAttack = false;
          //  Test.SetActive(false);
            
             
            
           
            // _animator.ResetTrigger("Attack");
          
        }


    }

    public void Impulse()
    {
        source.GenerateImpulse();
    }
}
