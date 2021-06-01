using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipEngine : MonoBehaviour
{

    [SerializeField]
    Transform model;

    [SerializeField]
    float _forwardForce = 10;
    [SerializeField]
    float _maxForwardSpeed = 10;
    [SerializeField]
    float _maxBackSpeed = 2;

    [SerializeField]
    float _boostForce = 50;
    [SerializeField]
    float _boostMaxSpeed = 20;

    [SerializeField]
    float _horizontalForce = 20;
    [SerializeField]
    float _maxHorizontalSpeed = 4;

    [SerializeField]
    float _verticalForce = 20;
    [SerializeField]
    float _maxVerticalSpeed = 4;

    [SerializeField]
    float _rollForce = 20;
    [SerializeField]
    float _maxRollSpeed = 4;

    [SerializeField]
    float _yawForce = 20;
    [SerializeField]
    float _maxYawSpeed = 4;

    [SerializeField]
    float _pitchtForce = 20;
    [SerializeField]
    float _maxPitchSpeed = 4;

    [SerializeField]
    float tilt = 1f;
    [SerializeField]
    float tiltSpeed = 2f;
    [SerializeField]
    float maxTilt = 20;
    [SerializeField]
    float yawMinToTilt = 20f;

    public bool tiltControl = true;

    public struct EngineInput
    {
        float _inputValue;
        bool _justPressed, _isPressing;
        public float Value
        {
            get { return _inputValue; }
            set
            {
                pressing = value == 0 ? false : true;
                _inputValue = Mathf.Clamp(value, -1, 1);
            }
        }
        public bool pressing
        {
            get { return _isPressing; }
            set
            {
                justPressed = value && !pressing ? true : false;
                _isPressing = value;
            }
        }
        public bool justPressed { get; set; }
    }

    public EngineInput acelerationInput;
    public EngineInput veritcalInput;
    public EngineInput horizontalInput;
    public EngineInput rollInput;
    public EngineInput yawInput;
    public EngineInput pitchInput;
    public EngineInput boostInput;

    IEnumerator AcelerationEngine;
    IEnumerator HorizontalMoveEngine;
    IEnumerator VerticalMoveEngine;
    IEnumerator YawEngine;
    IEnumerator PitchEngine;
    IEnumerator RollEngine;
    IEnumerator TiltControllEngine;
    IEnumerator BoostEngine;

    Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        AcelerationEngine = Aceleration();
        HorizontalMoveEngine = HorizontalMove();
        VerticalMoveEngine = VerticalMove();
        YawEngine = Yaw();
        PitchEngine = Pitch();
        RollEngine = Roll();
        TiltControllEngine = TiltControll();
        BoostEngine = Boost();

        SwitchEngine(true);

    }


    public void SwitchEngine(bool switchOn)
    {
        if (switchOn)
        {
            StartCoroutine(AcelerationEngine);
            StartCoroutine(HorizontalMoveEngine);
            StartCoroutine(VerticalMoveEngine);
            StartCoroutine(YawEngine);
            StartCoroutine(PitchEngine);
            StartCoroutine(RollEngine);
            StartCoroutine(BoostEngine);


            if (tiltControl)
            {
                StartCoroutine(TiltControllEngine);
            }
        }
        else
        {
            StopAllCoroutines();
        }
    }

    public void SwitchTiltControl(bool switchOn)
    {
        if (switchOn)
        {
            StartCoroutine(TiltControllEngine);
            tiltControl = true;
        }
        else
        {
            StopCoroutine(TiltControllEngine);
            Vector3 actualRotation = model.localRotation.eulerAngles;
            model.localRotation = Quaternion.Euler(actualRotation.x, actualRotation.y, 0);
            tiltControl = false;
        }

    }

    IEnumerator Aceleration()
    {
        WaitForFixedUpdate wait = new WaitForFixedUpdate();
        while (true)
        {
            if (acelerationInput.pressing && !boostInput.pressing)
            {
                _rb.AddRelativeForce(Vector3.forward * _forwardForce * acelerationInput.Value, ForceMode.Force);
                Vector3 localVelocity = transform.InverseTransformDirection(_rb.velocity);
                localVelocity.z = Mathf.Clamp(localVelocity.z, -_maxBackSpeed, _maxForwardSpeed);
                _rb.velocity = transform.TransformDirection(localVelocity);
                yield return wait;
            }
            else
            {
                yield return wait;
            }
        }
    }

    IEnumerator HorizontalMove()
    {
        WaitForFixedUpdate wait = new WaitForFixedUpdate();
        while (true)
        {
            if (horizontalInput.pressing)
            {
                _rb.AddRelativeForce(Vector3.right * _horizontalForce * horizontalInput.Value, ForceMode.Force);
                Vector3 localVelocity = transform.InverseTransformDirection(_rb.velocity);
                localVelocity.x = Mathf.Clamp(localVelocity.x, -_maxHorizontalSpeed, _maxHorizontalSpeed);
                _rb.velocity = transform.TransformDirection(localVelocity);
                yield return wait;
            }
            else
            {
                yield return wait;
            }
        }
    }


    IEnumerator VerticalMove()
    {
        WaitForFixedUpdate wait = new WaitForFixedUpdate();
        while (true)
        {
            if (veritcalInput.pressing)
            {
                _rb.AddRelativeForce(Vector3.up * _verticalForce * veritcalInput.Value, ForceMode.Force);
                Vector3 localVelocity = transform.InverseTransformDirection(_rb.velocity);
                localVelocity.y = Mathf.Clamp(localVelocity.y, -_maxVerticalSpeed, _maxVerticalSpeed);
                _rb.velocity = transform.TransformDirection(localVelocity);
                yield return wait;
            }
            else
            {
                yield return wait;
            }
        }
    }

    IEnumerator Roll()
    {
        WaitForFixedUpdate wait = new WaitForFixedUpdate();
        while (true)
        {
            if (rollInput.pressing)
            {
                _rb.AddRelativeTorque(Vector3.forward * _rollForce * rollInput.Value, ForceMode.Force);
                Vector3 localAngularVelocity = transform.InverseTransformVector(_rb.angularVelocity);
                localAngularVelocity.z = Mathf.Clamp(localAngularVelocity.z, -_maxRollSpeed, _maxRollSpeed);
                _rb.angularVelocity = transform.TransformVector(localAngularVelocity);
                yield return wait;
            }
            else
            {
                yield return wait;
            }
        }
    }

    IEnumerator Yaw()
    {
        WaitForFixedUpdate wait = new WaitForFixedUpdate();
        while (true)
        {
            if (yawInput.pressing)
            {

                _rb.AddRelativeTorque(Vector3.up * yawInput.Value * Mathf.Abs(yawInput.Value) * _yawForce, ForceMode.Force);

                Vector3 localAngularVelocity = transform.InverseTransformVector(_rb.angularVelocity);
                localAngularVelocity.y = Mathf.Clamp(localAngularVelocity.y, -_maxYawSpeed, _maxYawSpeed);
                _rb.angularVelocity = transform.TransformVector(localAngularVelocity);

                yield return wait;
            }
            else
            {
                yield return wait;
            }
        }
    }


    IEnumerator Pitch()
    {
        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        while (true)
        {
            if (pitchInput.pressing)
            {
                _rb.AddRelativeTorque(Vector3.left * pitchInput.Value * Mathf.Abs(pitchInput.Value) * _pitchtForce, ForceMode.Force);
                Vector3 localAngularVelocity = transform.InverseTransformVector(_rb.angularVelocity);
                localAngularVelocity.x = Mathf.Clamp(localAngularVelocity.x, -_maxPitchSpeed, _maxPitchSpeed);
                _rb.angularVelocity = transform.TransformVector(localAngularVelocity);

                yield return wait;
            }
            else
            {
                yield return wait;
            }
        }
    }

    IEnumerator Boost()
    {

        WaitForFixedUpdate wait = new WaitForFixedUpdate();
        while (true)
        {
            if (boostInput.pressing)
            {

                _rb.AddRelativeForce(Vector3.forward * _boostForce * boostInput.Value, ForceMode.Force);
                Vector3 localVelocity = transform.InverseTransformDirection(_rb.velocity);
                localVelocity.z = Mathf.Clamp(localVelocity.z, -_boostMaxSpeed, _boostMaxSpeed);
                _rb.velocity = transform.TransformDirection(localVelocity);
                yield return wait;
            }
            else
            {
                yield return wait;
            }
        }
    }

    IEnumerator TiltControll()
    {
        while (true)
        {
            //   Vector3 localAngularVelocity = transform.InverseTransformVector(_rb.angularVelocity);

            Quaternion rotation;
            if (Mathf.Abs(yawInput.Value) > yawMinToTilt || horizontalInput.Value == 0)
            {
                rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Clamp(Mathf.Abs(yawInput.Value) * yawInput.Value, -maxTilt, maxTilt) * -tilt);
            }
            else
            {
                rotation = Quaternion.Euler(0.0f, 0.0f, horizontalInput.Value * -tilt);
            }

            model.localRotation = Quaternion.Lerp(model.localRotation, rotation, Time.fixedDeltaTime * tiltSpeed);

            yield return new WaitForFixedUpdate();
        }
    }


}
