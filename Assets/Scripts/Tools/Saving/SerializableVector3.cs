using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]

//a lot  or all from https://answers.unity.com/questions/956047/serialize-quaternion-or-vector3.html
public struct SerializableVector3 
{

   public float x;
   public float y;
   public float z;

    public SerializableVector3(float _x, float _y, float _z){
        x = _x;
        y = _y;
        z = _z;
    }

    //change coordinates to a Vector3 to be used elsewhere
    public static implicit operator Vector3(SerializableVector3 _value){
        return new Vector3(_value.x, _value.y, _value.z);
    }
    //change from this structs vector3 to actual vector3
    public static implicit operator SerializableVector3(Vector3 _value){
        return new SerializableVector3(_value.x, _value.y, _value.z);
    }

   
}
