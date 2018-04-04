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
        float AngleDifferenceSigned = StickAngleY - CharAngleY;        
        //eger bu script; Client'imizin karakteri uzerinde calismiyorsa, yani diger oyuncularin karakterlerinden birisi ise;
        //VEYA karakter ile joystick ayni aciya bakiyorsa
        if (!isLocalPlayer || StickAngleY == 0f /*|| AngleDifference < _angleAmountToStartRotating */)
        {
            //bu fonksiyonun bu satirdan daha altindaki satirlarini okuma
            return;
        }        
        //bir framede donecegi aci
        float angle = _anglePerFrameFixed;
        //joystick ile karakter arasindaki acinin pozitifi
        float AngleDifference = OgtMathHelper.ConvertToPositive(AngleDifferenceSigned);
        //aralarindaki aciyi 360'a tamamlayan aci
        float RemainingAngle = 360 - AngleDifference;
        //hangi aci daha kisa donmek icin?
        if (AngleDifference <= RemainingAngle)
        {

        }
        else
        {

        }

        //eger aralarindaki aci eksi ciktiysa, karakter joystickin solunda demektir
        if (AngleDifferenceSigned < 0)
        {
            //karakterin saga donmesi icin, bakis yonu acisini azaltmamiz gerekiyor
            angle = -angle;
        }
        _character.Rotate(Vector3.up, angle);
    }

    private void FixedUpdate()
    {
        CheckRotation();
    }
}
