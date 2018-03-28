using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

//bu class karakter objesinin bir component'ı olmalı
public class OGTCharacterMovement : NetworkBehaviour
{
    [SerializeField]
    private Rigidbody _rigidBody;

    [SerializeField]
    private Animator  _animator;

    [SerializeField]
    private Vector3 _spawnPoint = Vector3.zero;

    [SerializeField]
    GameObject CameraPrefab = null;

    [SerializeField]
    private float _anglePrecision = 2f;

    private float _directionPointAngle;
    //disaridan deskin degeri degistirilememesi icin , sadece okunabilir sekilde disari aciyoruz...
    public float DirectionPointAngle
    {
        get
        {
            return _directionPointAngle;
        }
    }

    private static OGTCharacterMovement _singleton;

    public static OGTCharacterMovement Singleton
    {
        get { return _singleton; }
    }

    [SerializeField]
    private Transform _character = null;

    //Karaktermizin hareket edip etmeyecegini, edecekse hangi yone edecegini buradan anliyoruz. Hareket istenmiyorsa 0'lar atanmali
    private Vector3 _directionPoint = Vector3.zero;
    private Vector3 _ModelVectorRotation = Vector3.zero;

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

        if (_directionPoint == Vector3.zero)
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

        //model oynatma
        _character.Translate(_directionPoint * Time.fixedDeltaTime);

        //model döndürme
        float angleModel = Vector3.Angle(Vector3.right, _ModelVectorRotation);
        float angleDirection = Vector3.Angle(Vector3.right, _directionPoint);
    }

    /// <param name="joystickAngle"joystick'in X ekseni ile yaptiigi aciyi 3D karakteriimizin , Yön Vektorune verdiren method</param>
    public void SetDirectionFromJoystickAngle(float joyStickAngle)
    {
        //bizim elimizde olan bir eşik degerin uzerine cikarsa karakterimizi kosturalim
        if (joyStickAngle > _anglePrecision)
        {
            Vector3 charPos = _character.position;
            Vector3 directionPointFromOrigin = new Vector3(charPos.x, charPos.y, charPos.z + 1);

            _directionPoint = directionPointFromOrigin - charPos;
        }
        else
        {
            _directionPoint.Set(0, 0, 0);
            _directionPointAngle = joyStickAngle;
            return;
        }
        Quaternion rotation = Quaternion.AngleAxis(90 - joyStickAngle, Vector3.up);
        _directionPoint = rotation * _directionPoint;
        //diger scriptlerimiz buradan okuyacak , o sebeple acimizi global bir degiskende tutuyoruz.
        _directionPointAngle = joyStickAngle;
    }

    private void FixedUpdate()
    {
        Movecharacter();
        //Vektor Debug
        if (OGTJoystick._isDown)
        {
            Vector3 charPos = _character.position;
            
            //mavi vektor = kooridnat orjininden baslayip , karakterin durdugu noktayi gosteriyor
            Debug.DrawLine(Vector3.zero, charPos, Color.blue);
            //sari vektor = kooridnat orjininden baslayip , bu karede(frame'de) karakterimizin gitmeye baslayacagi noktayi gosteriyor
            Vector3 zeroToPoint = charPos + _directionPoint;
            Debug.DrawLine(Vector3.zero, zeroToPoint, Color.yellow);
            //kirmzi vektor = mavi_vektor - sari_vektor ; yani karakterin durdugu yerden ; gidecegi noktayi gosteren vektor
            Debug.DrawLine(charPos, zeroToPoint, Color.red);
            //acinin hesaplandigi X eksenini temsil eden zigi
            //nokta1 of cizgi
            Vector3 point1 = new Vector3(charPos.x - 3, charPos.y, charPos.z);
            //nokta2 of cizgi
            Vector3 point2 = new Vector3(charPos.x + 3, charPos.y, charPos.z);
            Debug.DrawLine(point1, point2 , Color.green);
        }
    }

    private void Update()
    {
        
    }

    public override void OnStartLocalPlayer()
    {
        GameObject CopyCamera = GameObject.Instantiate(CameraPrefab, _character, false);
        _character.position = _spawnPoint;
        _ModelVectorRotation = _character.rotation.eulerAngles;
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