// MoveToClickPoint.cs

using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MoveToClickPoint : MonoBehaviour, IInputClickHandler
{
    NavMeshAgent _agent;

    // パス、座標リスト、ルート表示用Renderer
    NavMeshPath _path = null;
    Vector3[] _positions = new Vector3[9];
    public LineRenderer Lr;


    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        InputManager.Instance.PushFallbackInputHandler(gameObject);
        Lr.enabled = false;
    }


    public void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log("OnClick");
        Lr.enabled = true;

        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;

        //目的地の設定
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            _agent.destination = hitInfo.point;
        }

        // パスの計算
        _path = new NavMeshPath();
        NavMesh.CalculatePath(_agent.transform.position, _agent.destination, NavMesh.AllAreas, _path);
        _positions = _path.corners;

        // ルートの描画
        Lr.widthMultiplier = 0.2f;
        Lr.positionCount = _positions.Length;

        for (int i = 0; i < _positions.Length; i++)
        {
            Debug.Log("point " + i + "=" + _positions[i]);

            Lr.SetPosition(i, _positions[i]);
        }
    }
}