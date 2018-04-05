 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//1-Bu class karakter objesinin bir component'ı olmalı
//2-Adi Joystick olan aci degiskenine istediginiz hardware'den aci vererek de kullanabilirsiniz...
//...Joystick haricindeki aygitlardan aci alarak da calisir bu class.
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
        float StickAngleY = OGTCharacterMovement.Singleton.DirectionPointAngle;
        float CharAngleY = Angle_Char_yAxis;
        //joystick aci - karakter aci (signed)
        float StickMinusCharSigned = StickAngleY - CharAngleY;
        //joystick ile karakter arasindaki aci (unsigned - pozitif)
        float StickMinusChar = OgtMathHelper.ConvertToPositive(StickMinusCharSigned);
        //eger bu script; Client'imizin karakteri uzerinde calismiyorsa, yani diger oyuncularin karakterlerinden birisi ise;
        //VEYA karakter ile joystick ayni aciya bakiyorsa
        if (!isLocalPlayer || StickAngleY == 0f || StickMinusChar < _angleAmountToStartRotating)
        {
            //bu fonksiyonun bu satirdan daha altindaki satirlarini okuma
            return;
        }        
        //bir framede donecegi aci
        float angle = _anglePerFrameFixed;        
        //aralarindaki aciyi 360'a tamamlayan aci
        float RemainingAngle = 360 - StickMinusChar;

        //hangi aci daha kisa donmek icin? Bu kosul
        if(StickMinusChar <= RemainingAngle)
        {
            //eger aralarindaki aci eksi ciktiysa, karakter joystickin saginda demektir
            if (StickMinusCharSigned < 0)
            {
                //karakterin sola donmesi icin, bakis yonu acisini azaltmamiz gerekiyor
                angle = -angle;
            }
        }
        else //360a tamamlayan diger aci daha kisa ise;
        {
            //ve aralarindaki aci pozitif ise , yani karakter joystickin solunda ise;
            if (StickMinusCharSigned > 0)
            {
                //karakterin saga donmesi icin, bakis yonu acisini arttirmamiz gerekiyor
                angle = -angle;
            }
        }


        _character.Rotate(Vector3.up, angle);
    }

    private void FixedUpdate()
    {
        CheckRotation();
    }
}
