using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log_Rotation_Handler : MonoBehaviour {


    // ★彡[ Making a custom class to handle rotation speed and time to move it clock and counter-clock wise and having Serializable attribute will allow us to change the settings of this class in the Unity Editor ]彡★
    [Serializable]
    private class RotationComponent {

        public float _speed;
        public float _duration;
    }

    [SerializeField] List<RotationComponent> _rotationForm;
    [SerializeField] float _maxJointMotorTorque = 10000;
    private WheelJoint2D _wheelJoint;
    private JointMotor2D _jointMotor;

    private void Awake() {
        
        // ★彡[ Getting refrence to the wheel joint 2d component ]彡★
        _wheelJoint = GetComponent<WheelJoint2D> ();
        // ★彡[ Declaring new Joint Motor 2D method ]彡★
        _jointMotor = new JointMotor2D();
        // ★彡[ Starting the wheel rotation form coroutine ]彡★
        StartCoroutine( PlayRotationForm() );
    }

    private IEnumerator PlayRotationForm() {

        int _rotationIndex = 0;
        bool _canRotate = true;

        // ★彡[ Starting while loop to rotate the object continuously  ]彡★
        while ( _canRotate ) {

            // ★彡[ Waiting for the fixed update method to execute (only to make stable physics movement) ]彡★ 
            yield return new WaitForFixedUpdate();

            // ★彡[ assigning every rotation form list speed to the joint motor speed ]彡★
            _jointMotor.motorSpeed = _rotationForm[_rotationIndex]._speed;
            // ★彡[ assigning maximum motor torque to that joint motor ]彡★
            _jointMotor.maxMotorTorque = _maxJointMotorTorque;
            // ★彡[ Making the wheel joint motor equal to the changed joint motor ]彡★
            _wheelJoint.motor = _jointMotor;

            // ★彡[ Making the coroutine wait for the realtime seconds accrording to the duration of that perticular rotation form component (waiting for seconds acc to realtime only in order to not affect our loop if Time.timeScale changes) ]彡★
            yield return new WaitForSecondsRealtime( _rotationForm[_rotationIndex]._duration );

            // ★彡[ Incrementing the number in order to get and change the porperties of other components too that are available in rotation form list ]彡★
            _rotationIndex++;

            // ★彡[ Making the _rotation index back to 0 if there are no more components available in rotation form list (just to make this loop work but making it start from the 0th index again) ]彡★
            _rotationIndex = _rotationIndex < _rotationForm.Count ? _rotationIndex : 0;
        }
    }
}

