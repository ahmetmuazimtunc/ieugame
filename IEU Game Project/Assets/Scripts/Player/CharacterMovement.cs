using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

public class CharacterMovement : NetworkBehaviour
{
    #region OldCode
    //[SerializeField]
    //private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
    //private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    //private Vector3 m_CamForward;             // The current forward direction of the camera
    //private Vector3 m_Move;
    //private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.


    //[SerializeField]
    //GameObject CameraPrefab = null;

    //private void Start()
    //{
    //    // get the transform of the main camera
    //    if (Camera.main != null)
    //    {
    //        m_Cam = Camera.main.transform;
    //    }
    //    else
    //    {
    //        Debug.LogWarning(
    //            "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
    //        // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
    //    }

    //    // get the third person character ( this should never be null due to require component )
    //    m_Character = GetComponent<ThirdPersonCharacter>();
    //}

    //public override void OnStartLocalPlayer()
    //{
    //    GameObject CopyCamera = GameObject.Instantiate(CameraPrefab, transform, false);
    //}

    //private void FixedUpdate()
    //{
    //    if (!isLocalPlayer)
    //        {
    //            return;
    //        }

    //        // read inputs
    //        float h = CrossPlatformInputManager/*Input*/.GetAxis("Horizontal");
    //        float v = CrossPlatformInputManager/*Input*/.GetAxis("Vertical");

    //        // calculate move direction to pass to character
    //        if (m_Cam != null)
    //        {
    //            // calculate camera relative direction to move:
    //            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
    //            m_Move = v*m_CamForward + h*m_Cam.right;
    //        }
    //        else
    //        {
    //            // we use world-relative directions in the case of no main camera
    //            m_Move = v*Vector3.forward + h*Vector3.right;
    //        }

    //        // pass all parameters to the character control script
    //        m_Character.Move(m_Move, false, m_Jump);
    //        m_Jump = false;
    //}

    //private void Update()
    //{
    //    if (!m_Jump)
    //    {
    //        m_Jump = CrossPlatformInputManager/*Input*/.GetButtonDown("Jump");
    //    }
    //}

    //public override void OnStartClient()
    //{

    //} 
    #endregion

    [SerializeField]
    private Rigidbody _rigidBody;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    GameObject CameraPrefab = null;

    private static CharacterMovement _singleton;

    public static CharacterMovement Singleton
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

        //_directionOfMovement = transform.position + 
    }

    private void Movecharacter()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (_direction == Vector3.zero)
        {
            return;
        }

        if (!_animator.GetBool(_hashRun))
        {
            _animator.SetBool(_hashRun, true);
        }

        transform.Translate(_direction * Time.fixedDeltaTime);
    }


    /// <param name="joystickAngle">Joystick'ten gelen aci gosteren Vektor</param>
    public void SetDirectionFromJoystickAngle(float joyStickAngle)
    {
        _direction = Quaternion.AngleAxis(joyStickAngle, Vector3.forward) * _direction;
        Debug.Log(_direction);
    }

    private void FixedUpdate()
    {
        Movecharacter();
    }

    public override void OnStartLocalPlayer()
    {
        GameObject CopyCamera = GameObject.Instantiate(CameraPrefab, transform, false);
    }
}

public static class OgtMathHelper 
{
    /// <summary>
    /// iki vektorumuz arasindaki aciyi hesaplamamiz gereken durumlarda gorsel olarak yardımcı olacak bir fonksiyon
    /// Parametre olarak verdigimiz iki vektoru Unity Editor icerisinde Scene'e cizdiriyor ve aralarindaki aciyi Konsol penceresine yazdiriyor.
    /// </summary>
    public static void DebugAngleAndVisualizeVectors(Vector3 vectorOne, Vector3 vectorTwo)
    {
        Debug.DrawRay(Vector3.zero, vectorOne);
        Debug.DrawRay(Vector3.zero, vectorTwo);
        float angle = CalcualteAngleBetweenTwoVectors(vectorOne, vectorTwo);

        Debug.Log(angle);
    }

    public static float CalcualteAngleBetweenTwoVectors(Vector3 vectorOne, Vector3 vectorTwo)
    {
        return Vector3.Angle(vectorOne, vectorTwo);
    }
}