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

    private CharacterMovement _singleton;

    public CharacterMovement Singleton
    {
        get { return _singleton; }
    }

    private const float _speedPerFrame = 0.2f;
    public Vector3 _directionOfMovement = new Vector3(0, 0, 1);
    public bool _isMoving = false;

    private int hashJump;
    private int hashRun;
    private int hashIdle;

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
        hashIdle = Animator.StringToHash("idle");
        hashJump = Animator.StringToHash("jump");
        hashRun = Animator.StringToHash("run");
    }

    /// <summary>
    /// The method should be called in FixedUpdate()
    /// </summary>        
    private void MoveCharacter(/*Vector3 direction*/)
    {     
        Vector3 destination = _directionOfMovement * _speedPerFrame;
        _rigidBody.transform.Translate(destination);

        //Visualizing our 3D Vectors for debugging purposes 
        Debug.DrawRay(Vector3.zero, transform.position, Color.yellow);
        Debug.DrawRay(Vector3.zero, destination, Color.cyan);
        Debug.DrawRay(transform.position, _directionOfMovement, Color.blue);

        Debug.DrawRay(transform.position, new Vector3(0f, 0f, 10f), Color.red);
    }
    

    /// <param name="newDirection">direction vector should be normalized</param>
    public void AssignDirection(Vector3 newDirection)
    {
        Vector3 directionNormal = newDirection.normalized;
        _directionOfMovement = directionNormal;
    }

    private void FixedUpdate()
    {

    }

    public override void OnStartLocalPlayer()
    {
        GameObject CopyCamera = GameObject.Instantiate(CameraPrefab, transform, false);
    }

}