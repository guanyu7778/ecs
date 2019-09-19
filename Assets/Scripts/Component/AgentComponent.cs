using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[SerializeField]
public struct AgentComponent : IComponentData
{
    public float3 position;
    public int moveSpeed;
}
