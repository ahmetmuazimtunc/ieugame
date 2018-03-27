﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

public class OGTCharacterMovement : NetworkBehaviour
{
    [SerializeField]
    private Rigidbody _rigidBody;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Vector3 _spawnPoint = Vector3.zero;

    [SerializeField]
    GameObject CameraPrefab = null;

    [SerializeField]
    private float _anglePrecision = 2f;

    private static OGTCharacterMovement _singleton;

    public static OGTCharacterMovement Singleton
    {
        get { return _singleton; }
    }

    private const float _directionUnit = 1.0f;

    //Karaktermizin hareket edip etmeyecegini, edecekse hangi yone edecegini buradan anliyoruz. Hareket istenmiyorsa 0'lar atanmali
    private Vector3 _direction = new Vector3(0, 0, 0);

    private int _hashJump;
    private int _hashRun;
    private int _hashIdle;

    private void Start()
    {
        if (_singleton == null)
        {
            _singleton = this;
        }
        else
        {
            Destroy(this);
        }
        _hashIdle = Animator.StringToHash("idle");
        _hashJump = Animator.StringToHash("jump");
        _hashRun = Animator.StringToHash("run");
    }

    private void Movecharacter()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (_direction == Vector3.zero)
        {
            _animator.SetBool(_hashRun, false);
            _animator.SetBool(_hashIdle, true);
            return;
        }

        if (!_animator.GetBool(_hashRun))
        {
            _animator.SetBool(_hashIdle, false);
            _animator.SetBool(_hashRun, true);            
        }

        transform.Translate(_direction * Time.fixedDeltaTime);
    }

    /// <param name="joystickAngle">Joystick'ten gelen aci gosteren Vektor</param>
    public void SetDirectionFromJoystickAngle(float joyStickAngle)
    {
        //bizim elimizde olan bir eşik degerin uzerine cikarsa karakterimizi kosturalim
        if (joyStickAngle > _anglePrecision)
        {
            _direction.Set(0 , 0 , 1);
        }
        else
        {
            _direction.Set(0, 0, 0);
        }
        Quaternion rotation = Quaternion.AngleAxis(joyStickAngle, Vector3.up);
        //_direction = rotation * _direction;
    }

    private void FixedUpdate()
    {
        Movecharacter();
    }

    private void Update()
    {
        if (_direction != Vector3.zero)
        {
            Debug.DrawLine(transform.position, _direction * 10, Color.blue);
        }
    }

    public override void OnStartLocalPlayer()
    {
        GameObject CopyCamera = GameObject.Instantiate(CameraPrefab, transform, false);
        transform.position = _spawnPoint;
    }
}

public static class OgtMathHelper 
{
    /// <summary>
    /// iki vektorumuz arasindaki aciyi hesaplamamiz gereken durumlarda gorsel olarak yardımcı olacak bir fonksiyon
    /// Parametre olarak verdigimiz iki vektoru Unity Editor icerisinde Scene'e cizdiriyor ve aralarindaki aciyi Konsol penceresine yazdiriyor.
    /// </summary>
    public static float CalculateAngleBetweenTwoVectorsAndVisualizeThem(Vector3 vectorOne, Vector3 vectorTwo)
    {
        Debug.DrawRay(Vector3.zero, vectorOne);
        Debug.DrawRay(Vector3.zero, vectorTwo);
        float angle = CalcualteAngleBetweenTwoVectors(vectorOne, vectorTwo);

        Debug.Log(angle);
        return angle;
    }

    public static float CalcualteAngleBetweenTwoVectors(Vector3 vectorOne, Vector3 vectorTwo)
    {
        return Vector3.Angle(vectorOne, vectorTwo);
    }

    public static float ConvertToPositive(float number)
    {
        float toReturn = number;
        toReturn = (toReturn < 0) ? (-toReturn) : (toReturn);
        return toReturn;
    }    
}