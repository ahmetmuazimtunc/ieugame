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

    private float _angleOffset = 10f;

    private const float _anglePerFrameFixed = 2f;

    private float Angle_Char_yAxis
    {
        get
        {
            return _character.rotation.eulerAngles.y;
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
        //eger bu script; Client'imizin karakterinden calismiyorsa, yani diger oyuncularin karakterlerinden birisi ise;
        if (!isLocalPlayer || OGTCharacterMovement.Singleton.DirectionPointAngle == 0f)
        {
            //bu fonksiyonun buranin altindaki satirlarini okuma
            return;
        }
        //karakterin x ekseni ile yaptigi aci , gitmesi gereken patikanin-x Ekesni ile yaptigi acidan buyuk ise;   
        float angle = _anglePerFrameFixed;
        Debug.Log("Char_Angle: " + Angle_Char_yAxis);
        Debug.DrawLine(_character.transform.position, OGTCharacterMovement.Singleton._directionPoint);
        if (Angle_Char_yAxis > OGTCharacterMovement.Singleton.DirectionPointAngle + _angleOffset)
        {
            //aci kabull ettigimiz aralik degerinin icerisine girene kadar
            //karakteri saga dondur
            angle = -angle;
        }
        else if (Angle_Char_yAxis < OGTCharacterMovement.Singleton.DirectionPointAngle -_angleOffset)
        {
            
        }
        _character.Rotate(Vector3.up, angle);
    }

    private void FixedUpdate()
    {
        CheckRotation();
    }
}
