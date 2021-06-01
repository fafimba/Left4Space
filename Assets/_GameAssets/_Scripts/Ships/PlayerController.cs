using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpaceShip))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float mouseSensitivity;
    [SerializeField]
    RectTransform pointer;
    [SerializeField]
    Image shootPointerImage;
    [SerializeField]
    Image boostPointerImage;
    [SerializeField]
    RectTransform targetPointer;
    [SerializeField]
    Image targetPointerImage;

    [SerializeField]
    float maxDistaceAim = 300f;

    [SerializeField]
    GameObject _firstPersonCamera;
    [SerializeField]
    GameObject _thirdPersonCamera;

    [SerializeField]
    GameManager.Team _team;
    [SerializeField]
    GameObject colliderBody;

    AudioSource shootAudio;
    Vector2 screenCenter;
    SpaceShip ship;

    Ray pointerRay;
    RaycastHit pointerRayHit;
    bool isPointerHitting = false;
    float _tmpRate;


    void Start()
    {
        ship = GetComponent<SpaceShip>();
        GameManager.instance.CurrentCamera = _firstPersonCamera.activeSelf ? _firstPersonCamera : _thirdPersonCamera;
        colliderBody.layer = (int)_team;
        shootAudio = GetComponent<AudioSource>();
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

    }

    void Update()
    {
        SelectCamera();
        UpdateInputs();
        SetCursor(GameManager.instance.gameState == GameManager.GameState.InGame);
    }

    void FixedUpdate()
    {
        SetCursorPosition();
        SetTarget();
    }

    void UpdateInputs()
    {
        ship.Engine.acelerationInput.Value = Input.GetAxis("Aceleration");
        ship.Engine.horizontalInput.Value = Input.GetAxis("Horizontal");
        ship.Engine.veritcalInput.Value = Input.GetAxis("Vertical");
        ship.Engine.rollInput.Value = Input.GetAxis("Roll");
        ship.Engine.boostInput.Value = Input.GetAxis("Boost");
        if (Input.GetButton("Fire1")) Shoot();
        if (Input.GetButton("Fire2")) Shoot(true);
    }

    public void SetCursor(bool On)
    {
        if (On)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void SetCursorPosition()
    {
        boostPointerImage.gameObject.SetActive(ship.Engine.boostInput.pressing);
        shootPointerImage.gameObject.SetActive(!ship.Engine.boostInput.pressing);

        float pointerMaxDistance = Screen.height / 2;

        Vector2 pointerPosition = pointer.position;

        pointerPosition.x += Input.GetAxis("Yaw") * mouseSensitivity;
        pointerPosition.y += Input.GetAxis("Pitch") * mouseSensitivity;

        Vector2 newPointerPosition = Vector2.ClampMagnitude((pointerPosition - screenCenter), pointerMaxDistance) + screenCenter;
        pointer.position = newPointerPosition;

        ship.Engine.yawInput.Value = NormalizeToRange(0, 1, 0, pointerMaxDistance, newPointerPosition.x - screenCenter.x);
        ship.Engine.pitchInput.Value = NormalizeToRange(0, 1, 0, pointerMaxDistance, newPointerPosition.y - screenCenter.y);

    }

    float NormalizeToRange(float NewMin, float NewMax, float OldMin, float OldMax, float OldValue)
    {
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        return (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;
    }

    void SetTarget()
    {
        pointerRay = Camera.main.ScreenPointToRay(pointer.transform.position);

        if (Physics.SphereCast(pointerRay, 10f, out pointerRayHit, maxDistaceAim, GameManager.instance.SetLayers(_team)))
        {
            isPointerHitting = true;
            targetPointerImage.gameObject.SetActive(true);
            targetPointer.position = Camera.main.WorldToScreenPoint(pointerRayHit.transform.position);
        }
        else
        {
            isPointerHitting = false;
            targetPointerImage.gameObject.SetActive(false);
        }
    }


    void Shoot(bool missile = false)
    {

        pointerRay = Camera.main.ScreenPointToRay(pointer.transform.position);

        if (!ship.Engine.boostInput.pressing)
        {
            if (Time.time > _tmpRate)
            {
                _tmpRate = Time.time + ship.weapon.rate;

                if (isPointerHitting)
                {
                    if (!missile)
                    {
                        ship.weapon.Shoot(pointerRayHit);
                        shootAudio.PlayOneShot(shootAudio.clip);
                    }
                    else
                    {
                        ship.weapon.ShootMissile(pointerRayHit.transform);
                    }

                }
                else
                {
                    if (!missile)
                    {
                        ship.weapon.Shoot(pointerRay.GetPoint(maxDistaceAim));
                        shootAudio.PlayOneShot(shootAudio.clip);
                    }
                    else
                    {
                        ship.weapon.ShootMissile();
                    }
                }
            }


            //     if (Physics.SphereCast(pointerRay, 10f, out pointerRayHit, maxDistaceAim, GameManager.instance.SetLayers(_team)))
            //   {
            //       targetPointerImage.gameObject.SetActive(true);
            //       targetPointer.position = Camera.main.WorldToScreenPoint(pointerRayHit.transform.position);
            //
            //       if (Time.time > _tmpRate)
            //       {
            //           _tmpRate = Time.time + ship.weapon.rate;
            //
            //           if (!missile)
            //           {
            //               ship.weapon.Shoot(pointerRayHit);
            //               shootAudio.PlayOneShot(shootAudio.clip);
            //           }
            //           else
            //           {
            //               ship.weapon.ShootMissile(pointerRayHit.transform);
            //           }
            //       }
            //   }
            ///     else
            //   {
            //       targetPointerImage.gameObject.SetActive(false);
            //       if (Time.time > _tmpRate)
            //       {
            //           _tmpRate = Time.time + ship.weapon.rate;
            //           if (!missile)
            //           {
            //               ship.weapon.Shoot(pointerRay.GetPoint(maxDistaceAim));
            //               shootAudio.PlayOneShot(shootAudio.clip);
            //           }
            //           else
            //           {
            //               ship.weapon.ShootMissile();
            //           }
            //       }
            //   }
        }
    }




    void SelectCamera()
    {
        if (Input.GetButtonDown("ToggleCamera"))
        {
            if (GameManager.instance.CurrentCamera == _firstPersonCamera)
            {
                ship.Engine.SwitchTiltControl(true);
                GameManager.instance.CurrentCamera = _thirdPersonCamera;
            }
            else
            {
                GameManager.instance.CurrentCamera = _firstPersonCamera;
                ship.Engine.SwitchTiltControl(false);
            }
        }
    }





    private void OnDestroy()
    {
        if (ship.armor.Life.Actual == 0)
        {
            GameManager.instance.EndGame(true);
        }

    }


    // IEnumerator Dodge()
    // {
    //     if (Input.GetKey(KeyCode.Mouse2))
    //     {
    //
    //     }
    //
    //     yield return new  WaitForFixedUpdate();
    // }

    IEnumerator CheckMaxDistance()
    {
        while (true)
        {
            if (new Vector2(transform.position.x, transform.position.z).magnitude > GameManager.instance.maxRadius ||
                transform.position.y > GameManager.instance.maxHeight ||
                transform.position.y < GameManager.instance.minHeight)
            {
                //Alert and hurt player

            }

            yield return new WaitForSeconds(1f);
        }
    }

}
