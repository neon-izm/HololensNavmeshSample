using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCube: MonoBehaviour {

    Vector3 _startPosition;
    public float Amplitude;  //←移動させる幅値を各Cubeごとにインスペクタからセットする
    public float Speed;      //←移動スピードを各Cubeごとにインスペクタからセットする

    // Use this for initialization
    void Start () {
        _startPosition = transform.localPosition;

    }

    // Update is called once per frame
    void Update () {
        float z = Amplitude * Mathf.Sin(Time.time * Speed);
        transform.localPosition = _startPosition + new Vector3(0,0,z);
    }
}