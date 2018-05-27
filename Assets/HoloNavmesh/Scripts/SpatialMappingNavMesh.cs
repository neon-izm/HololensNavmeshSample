using HoloToolkit.Unity.SpatialMapping;
using UnityEngine;
using HoloToolkit.Unity;

namespace HololensNavmeshSample
{
    /// <summary>
    /// SpatialMappingの更新タイミングでNavmeshを再生成するスクリプト
    /// </summary>
    public class SpatialMappingNavMesh : MonoBehaviour
    {
        [Tooltip("SpatialMappingプレハブを指定してください")]
        public GameObject SpatialMapping;

        private void Awake()
        {
            var spatialMappingSources = SpatialMapping.GetComponents<SpatialMappingSource>();
            foreach (var source in spatialMappingSources)
            {
                source.SurfaceAdded += SpatialMappingSource_SurfaceAdded;
                source.SurfaceUpdated += SpatialMappingSource_SurfaceUpdated;
            }
        }


        private void SpatialMappingSource_SurfaceAdded(object sender,
            DataEventArgs<SpatialMappingSource.SurfaceObject> e)
        {
            e.Data.Object.AddComponent<NavMeshSourceTag>();
        }

        private void SpatialMappingSource_SurfaceUpdated(object sender,
            DataEventArgs<SpatialMappingSource.SurfaceUpdate> e)
        {
            var navMeshSourceTag = e.Data.New.Object.GetComponent<NavMeshSourceTag>();
            if (navMeshSourceTag == null)
            {
                e.Data.New.Object.AddComponent<NavMeshSourceTag>();
            }
        }
    }
}