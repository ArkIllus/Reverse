using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;
using Cinemachine;

public class Boss_2 : MonoBehaviour
{
    public PlayerController _player;
    public Transform Target;
    public CinemachineImpulseSource source;

    public float distance;
    private float current_X;

    private BoxCollider2D _boxCollider;
    private Bounds bounds;
    private float length;

    public float minX;
    public float maxX;
    public float center_Y;
    private Vector3 _position;
    public Animator _animator;
    public float minX_last;




    [SerializeField] float Cd = 5f;
    [SerializeField] private float endTime1;
    [SerializeField] bool isAttack;
    [SerializeField] private float endTime2;
    [SerializeField] float Stay = 3.1f;
    private int lightNumber =3;
    private int current_number =0;
    //public GameObject hit;
    private Vector3 lightingPosition;
    public GameObject lightning;
    public GameObject Aim;
    public GameObject Hit;
    public bool isStart;


    private bool isLighting;
    private bool isAim;

    public float AimTime = 0.75f;
    private float End_AimTime;
    public float LightingTime = 1f;
    private float End_LightTime;
    public AudioSource Aim_Audio;
    public AudioSource Hit_Audio;

    //public GameObject Test;
    // Start is called before the first frame update
    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        bounds = _boxCollider.bounds;
        length = bounds.max.x - bounds.min.x;
        // _animator = GetComponent<Animator>();
        _animator = GetComponentInChildren<Animator>();
        transform.position = Target.position;
    }

    private void OnEnable()
    {
        transform.position = Target.position;
        _animator = GetComponentInChildren<Animator>();
        endTime1 = 0;
        endTime2 = 0;
        End_AimTime = 0;
        End_LightTime = 0;
        current_number = 0; 
        isLighting = false;
        isAim = false;
        isAttack = false;
        isStart = false;
    }
    private void OnDisable()
    {
        _animator = null;
        lightning.SetActive(false);
        Aim.SetActive(false);
        Hit.SetActive(false);
        Aim_Audio.Stop();
        Hit_Audio.Stop();
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
                if (Target.position.x > maxX)
                {

                    Debug.Log(9);
                  //  transform.position = Vector2.MoveTowards(transform.position, Target.position, 20f * Time.deltaTime);
                    _position = new Vector3(maxX, _player.transform.position.y, 0);
                    transform.position = Vector2.MoveTowards(transform.position, _position, 20f * Time.deltaTime);
                }
                else
                {
                    Debug.Log(8);

                    transform.position = Vector2.MoveTowards(transform.position, Target.position, 20f * Time.deltaTime);
                }


            }
            else
            {

                //current_X = _player.transform.position.x - distance;
                current_X = Target.position.x;
                if (Target.position.x > maxX)
                {
                    Debug.Log(7);
                    _position = new Vector3(maxX, _player.transform.position.y, 0);
                    transform.position = Vector2.MoveTowards(transform.position, _position, 20f * Time.deltaTime);
                }
                // Debug.Log(Vector2.Distance(transform.position, _player.transform.position));
                //current_X = Mathf.Clamp(current_X, minX, _player.transform.position.x);
                else
                {
                    Debug.Log(6);

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
        if (!isAttack)
            endTime1 += Time.deltaTime;
        if (endTime1 > Cd && !isAttack)
        {
            isAttack = true;
             _animator.SetBool("Attack", true);
           
            // Test.SetActive(true);


            //Debug.Log("cd1");
        }
        if (isAttack)
        {
            endTime2 += Time.deltaTime;
            if (isStart)
            {
                Light();
            }
           

       



        }















        if (endTime2 >= 6.5f)
        {
            _animator.SetBool("Attack", false);
            Hit.SetActive(false);
            // laser.SetActive(false);
            // hit.SetActive(false);
        }
        if (endTime2 > Stay)
        {
            current_number = 0;
            endTime1 = 0;
            endTime2 = 0;
            isAttack = false;
            isStart = false;
            lightning.SetActive(false);
            //Aim.SetActive(false);
            //  Test.SetActive(false);




            // _animator.ResetTrigger("Attack");
        
        }


    }


  
    private void Light()
    {
        if (current_number < lightNumber)
        {

            if (!isAim)
            {
                Debug.Log(current_number);
                isAim = true;
                lightning.SetActive(false);
                lightingPosition = new Vector3(_player.transform.position.x, Aim.transform.position.y, Aim.transform.position.z);
                Aim.transform.position = lightingPosition;
                //  Debug.Log(lightingPosition.x);

                Aim.SetActive(true);
                if (current_number == 0)
                    Aim_Audio.Play();
            }
            else
            {
                End_AimTime += Time.deltaTime;
                {

                    if (End_AimTime > AimTime)
                    {


                        if (!isLighting)
                        {
                            Aim_Audio.Stop();
                            Hit_Audio.Play();
                            isLighting = true;
                            Aim.SetActive(false);

                            lightning.transform.position = new Vector3(lightingPosition.x, lightning.transform.position.y, lightning.transform.position.z);
                            source.GenerateImpulse();

                            lightning.SetActive(true);
                        }
                        else
                        {

                            End_LightTime += Time.deltaTime;
                            if (End_LightTime > LightingTime)
                            {
                                Debug.Log(465464);
                                //Hit_Audio.Stop();
                                End_AimTime = 0;
                                isAim = false;
                                End_LightTime = 0;
                                isLighting = false;
                                lightning.SetActive(false);
                                current_number++;



                                //Aim.SetActive(false);

                            }
                        }



                    }
                }
            }

        }
    }




}
