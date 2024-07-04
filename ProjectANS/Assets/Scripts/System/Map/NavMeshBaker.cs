using UnityEngine;
using UnityEngine.AI;

namespace System.Map
{
    public class NavMeshBaker : MonoBehaviour
    {
        private NavMeshSurface _navMeshSurface;

        public void BakeNavMesh()
        {
            if (_navMeshSurface == null)
                _navMeshSurface = GetComponent<NavMeshSurface>();
            _navMeshSurface.BuildNavMesh();
        }
    }
}
