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

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore((typeof(TransformSystemGroup)))]
public class AIAgentSystem : JobComponentSystem
{
    private EntityQuery agentQuery;

    [BurstCompile]
    struct CreateNavNextPointJob : IJobForEachWithEntity<AgentComponent>
    {
        public void Execute(Entity entity, int index, ref AgentComponent c0)
        {
            
        }
    }

    protected override void OnCreate()
    {
        base.OnCreate();
        agentQuery  = GetEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[] { ComponentType.ReadWrite<AgentComponent>(), ComponentType.ReadWrite<LocalToWorld>() },
        });
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new CreateNavNextPointJob
        {

        };
        return job.Schedule(agentQuery, inputDeps);
    }
}
