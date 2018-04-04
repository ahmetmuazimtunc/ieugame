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
            return _character.eulerAngles.y;
            //float myangle = 90 - _character.rotation.eulerAngles.y;            
            //if (myangle < 0)
            //{
            //    myangle = 360 + myangle;
            //}
            //return myangle;
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
        float StickAngleX = OGTCharacterMovement.Singleton.DirectionPointAngle;
        float CharAngleX = Angle_Char_yAxis;
        float AngleDifference = StickAngleX - CharAngleX;        
        //eger bu script; Client'imizin karakteri uzerinde calismiyorsa, yani diger oyuncularin karakterlerinden birisi ise;
        //VEYA karakter ile joystick ayni aciya bakiyorsa
        if (!isLocalPlayer || StickAngleX == 0f || AngleDifference < _angleAmountToStartRotating )
        {
            //bu fonksiyonun bu satirdan daha altindaki satirlarini okuma
            return;
        }        
        float angle = _anglePerFrameFixed;
        Debug.Log("CHAR:" + CharAngleX + " - JOYSTICK:" + OGTCharacterMovement.Singleton.DirectionPointAngle);

        if (0 < StickAngleX && StickAngleX < 180)
        {           
            //eger aralarindaki aci eksi ciktiysa, karakter joystickin solunda demektir
            if (AngleDifference < 0)
            {
                //karakterin saga donmesi icin, bakis yonu acisini azaltmamiz gerekiyor
                angle = -angle;
            } 
        }
        else
        {
            
        }      

        //dairenin kalan acisi (360 - x)
        float CirculerComplementaryAngle = 360 - AngleDifference;
        _character.Rotate(Vector3.up, angle);
        Debug.Log("CHAR DONDUKTEN SONRA ACI: " + _character.transform.rotation.eulerAngles.y);
    }

    private void FixedUpdate()
    {
        //CheckRotation();
    }
}
