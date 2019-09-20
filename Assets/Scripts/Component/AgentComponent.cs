using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[SerializeField]
public struct AgentComponent : IComponentData
{
    public float3 position;
    public float3 nexPosition;
    public float moveSpeed;
}

[SerializeField]
public struct SceneTarget
{
    public float3 position1;
    public float3 position2;
    public float3 position3;
    public float3 position4;
}
