 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//bu class karakter objesinin bir component'ı olmalı
public class OGTCharacterRotation : NetworkBehaviour
{
    private OGTCharacterRotation _singleton;

    public OGTCharacterRotation Singleton
    {

        get
        {
            return _singleton;
        }
    }

    [SerializeField]    
    private Transform _character = null;

    private float _angleAmountToStartRotating = 5f;

    private const float _anglePerFrameFixed = 2f;

    private float Angle_Char_yAxis
    {
        get
        {
            float myangle = 90 - _character.rotation.eulerAngles.y;            
            if (myangle < 0)
            {
                myangle = 360 + myangle;
                //myangle = 360 - fark;
            }
            return myangle;
        }
    }

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
    }

    public void CheckRotation()
    {
        //joystick ile karakterin bakis yonu arasinda acisal fark var mi?
        float StickAngle = OGTCharacterMovement.Singleton.DirectionPointAngle;
        float CharAngle = Angle_Char_yAxis;
        float AngleDifference = StickAngle - CharAngle;
        AngleDifference = OgtMathHelper.ConvertToPositive(AngleDifference);
        //eger bu script; Client'imizin karakteri uzerinde calismiyorsa, yani diger oyuncularin karakterlerinden birisi ise;
        //VEYA karakter ile joystick ayni aciya bakiyorsa
        if (!isLocalPlayer || StickAngle == 0f || AngleDifference < _angleAmountToStartRotating )
        {
            //bu fonksiyonun bu satirdan daha altindaki satirlarini okuma
            return;
        }        
        float angle = _anglePerFrameFixed;
        Debug.Log("CHAR:" + CharAngle + " - JOYSTICK:" + OGTCharacterMovement.Singleton.DirectionPointAngle);
        //Debug.DrawLine(_character.transform.position, OGTCharacterMovement.Singleton._directionPoint);

        //karkateri hangi taraftan dondurursek, daha cabuk ulasiriz yeni aciya?
        float LeftAngle = 360 - CharAngle;
        if (180 > CharAngle && CharAngle > 0)
        {
            AngleDifference = 
        }
        else if (180 < CharAngle && CharAngle < 360)
        {
            AngleDifference = 
        }

        if (LeftAngle < AngleDifference)
        {
            angle = -angle;
        }

        _character.Rotate(Vector3.up, angle);
    }

    private void FixedUpdate()
    {
        CheckRotation();
    }
}
