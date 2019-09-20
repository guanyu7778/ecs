using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Entities;
using UnityEngine.Experimental.AI;
using Unity.Jobs;
using Unity.Collections;
using Unity.Transforms;
using System;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore((typeof(TransformSystemGroup)))]
public class Logic_AIAgentSystem : JobComponentSystem
{
    private EntityQuery agentQuery;
    private NavMeshWorld navWorld;

    //[BurstCompile]
    struct CreateNavNextPointJob : IJobForEachWithEntity<AgentComponent, LocalToWorld>
    {
        public SceneTarget setting;
        public void Execute(Entity entity, int index, ref AgentComponent c0, ref LocalToWorld c2)
        {
            //c2 = new LocalToWorld { Value = new float4x4(quaternion.identity, c2.Position + new float3(0, c0.moveSpeed * 0.01f, 0))};
            var navQuery = new NavMeshQuery(navWorld, Allocator.TempJob, 20);
            var startPos = navQuery.MapLocation(c0.position, new float3(10), 0);
            var endPos = navQuery.MapLocation(setting.position1, new float3(10), 0);
            var status = navQuery.BeginFindPath(startPos, endPos);
            int pathFindingInter = 0;
            if (status == PathQueryStatus.InProgress)
            {
                bool end = false;
                bool err = false;
                int nodeCount = 0;
                while (!end)
                {
                    var upStatus = navQuery.UpdateFindPath(10, out pathFindingInter);
                    if (upStatus != PathQueryStatus.InProgress)
                    {
                        end = true;
                        err = upStatus == PathQueryStatus.Success;
                    }
                }
                if (!err)
                {
                    var endStatus = navQuery.EndFindPath(out nodeCount);
                    if (endStatus == PathQueryStatus.Failure)
                    {
                        err = true;
                    }
                }
                if (!err)
                {
                    NativeSlice<PolygonId> path = new NativeSlice<PolygonId>();
                    var count = navQuery.GetPathResult(path);
                    Debug.Log(count);
                }
            }
        }
    }

    protected override void OnCreate()
    {
        base.OnCreate();
        var navWorld = NavMeshWorld.GetDefaultWorld();
        agentQuery  = GetEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[] { ComponentType.ReadWrite<AgentComponent>(), ComponentType.ReadWrite<LocalToWorld>() },
        });
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var agentCount = agentQuery.CalculateEntityCount();
        var job = new CreateNavNextPointJob
        {
            setting = new SceneTarget
            {
                position1 = new float3(-48, 0, 48),
                position2 = new float3(-48, 0, -48),
                position3 = new float3(48, 0, -48),
                position4 = new float3(48, 0, 48)
            }
        };
        return job.Schedule(agentQuery, inputDeps);
    }
}
