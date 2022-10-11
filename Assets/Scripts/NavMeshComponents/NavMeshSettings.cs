using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshSettings : MonoBehaviour
{

    private NavMeshSurface _surface;

    public static NavMeshSettings Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        _surface = GetComponent<NavMeshSurface>();
        _surface.BuildNavMesh();
    }

   public void UpdateNavMesh()
    {
        _surface.UpdateNavMesh(_surface.navMeshData);
        _surface.AddData();
    }
    
#if UNITY_EDITOR

    [CustomEditor(typeof(NavMeshSettings))]
    public class ItemEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            NavMeshSettings item = target as NavMeshSettings;
            
            item.Start();
            item.UpdateNavMesh();
        }
    }
#endif
}
