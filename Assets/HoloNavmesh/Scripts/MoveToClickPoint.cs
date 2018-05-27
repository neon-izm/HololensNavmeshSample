// MoveToClickPoint.cs

using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace HololensNavmeshSample
{
    /// <summary>
    /// AirTapクリックに合わせてキャラクターを移動させるスクリプト。
    /// </summary>
    public class MoveToClickPoint : MonoBehaviour, IInputClickHandler
    {
        /// <summary>
        /// 実際にNavmeshで動くキャラクターのルート(NavmeshAgentがアタッチされていることを想定している
        /// </summary>
        public GameObject TargetCharacter;

        /// <summary>
        /// LineRenderer（移動経路の可視化に使うLineRenderer
        /// </summary>
        public LineRenderer NavmeshPathLineRenderer;

        NavMeshAgent _targetCharactorNavmeshAgent;

        // パス、座標リスト、ルート表示用Renderer
        NavMeshPath _path = null;
        Vector3[] _navMeshCornerPositions = new Vector3[9];

        private IInputClickHandler _inputClickHandlerImplementation;

        void Start()
        {
            _targetCharactorNavmeshAgent = TargetCharacter.GetComponent<NavMeshAgent>(); //変更
            if (_targetCharactorNavmeshAgent == null)
            {
                Debug.LogError("Target Charactor Object Does't Attached NavmeshAgent");
                Debug.LogError("Destroy MoveToClickPoint script");
                Destroy(this);
            }

            //AirTapを受け取れるように設定
            InputManager.Instance.PushFallbackInputHandler(gameObject);
            TargetCharacter.SetActive(false); //キャラクターを一旦無効化（NavMesh生成前に出現すると、一瞬で落下してしまう為）
            if (NavmeshPathLineRenderer)
            {
                //LineRendererも非表示にする
                NavmeshPathLineRenderer.enabled = false;
            }
        }


        /// <summary>
        /// AirTap時の挙動
        /// </summary>
        /// <param name="eventData"></param>
        public void OnInputClicked(InputClickedEventData eventData)
        {
            RaycastHit hitInfo;
            var uiRayCastOrigin = Camera.main.transform.position;
            var uiRayCastDirection = Camera.main.transform.forward;
            if (Physics.Raycast(uiRayCastOrigin, uiRayCastDirection, out hitInfo))
            {
                //初めてのAirTapを検知したら、キャラクターを表示させて、AirTap位置にキャラクターを移動する
                if (!TargetCharacter.activeSelf)
                {
                    TargetCharacter.SetActive(true);
                    var hitPos = hitInfo.point;
                    _targetCharactorNavmeshAgent.transform.position = hitPos;
                }
            }


            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            //目的地の設定
            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
            {
                _targetCharactorNavmeshAgent.destination = hitInfo.point;
            }

            //ナビメッシュパスの計算
            _path = new NavMeshPath();
            NavMesh.CalculatePath(_targetCharactorNavmeshAgent.transform.position,
                _targetCharactorNavmeshAgent.destination, NavMesh.AllAreas, _path);
            _navMeshCornerPositions = _path.corners;

            if (!NavmeshPathLineRenderer) return;

            NavmeshPathLineRenderer.enabled = true;

            // ルートの描画
            NavmeshPathLineRenderer.widthMultiplier = 0.1f;
            NavmeshPathLineRenderer.positionCount = _navMeshCornerPositions.Length;

            for (int i = 0; i < _navMeshCornerPositions.Length; i++)
            {
                Debug.Log("point " + i + "=" + _navMeshCornerPositions[i]);

                NavmeshPathLineRenderer.SetPosition(i, _navMeshCornerPositions[i]);
            }
        }
    }
}