// MoveToClickPoint.cs

using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MoveToClickPoint : MonoBehaviour, IInputClickHandler
{
    NavMeshAgent agent;

    // パス、座標リスト、ルート表示用Renderer
    NavMeshPath path = null;
    Vector3[] positions = new Vector3[9];
    public LineRenderer lr;

    public GameObject cl; //追加
    private IInputClickHandler _inputClickHandlerImplementation;

    void Start()
    {
        agent = cl.GetComponent<NavMeshAgent>(); //変更
        InputManager.Instance.PushFallbackInputHandler(gameObject);
        cl.SetActive(false); //追加
        lr.enabled = false;
    }


    public void OnInputClicked(InputClickedEventData eventData)
    {
        //追加
        Vector3 hitPos, hitNormal;
        RaycastHit hitInfo;
        Vector3 uiRayCastOrigin = Camera.main.transform.position;
        Vector3 uiRayCastDirection = Camera.main.transform.forward;
        if (Physics.Raycast(uiRayCastOrigin, uiRayCastDirection, out hitInfo))
        {
            if (!cl.activeSelf)
            {
                cl.SetActive(true);
                hitPos = hitInfo.point;
                hitNormal = hitInfo.normal;
                agent.transform.position = hitPos;
            }
        }


        lr.enabled = true;

        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        //目的地の設定
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            agent.destination = hitInfo.point;
        }

        // パスの計算
        path = new NavMeshPath();
        NavMesh.CalculatePath(agent.transform.position, agent.destination, NavMesh.AllAreas, path);
        positions = path.corners;

        // ルートの描画
        lr.widthMultiplier = 0.2f;
        lr.positionCount = positions.Length;

        for (int i = 0; i < positions.Length; i++)
        {
            Debug.Log("point " + i + "=" + positions[i]);

            lr.SetPosition(i, positions[i]);
        }
    }
}