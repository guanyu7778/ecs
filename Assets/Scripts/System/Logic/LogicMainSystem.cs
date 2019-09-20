using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Transforms;

[DisableAutoCreation]
public class LogicSystem : ComponentSystem
{
    public NativeQueue<LogicComfirmedFrame> logicFrames;
    public Entity prefab;

    protected override void OnCreate()
    {
        base.OnCreate();
        logicFrames = new NativeQueue<LogicComfirmedFrame>(Allocator.Persistent);
    }

    protected override void OnUpdate()
    {
        if (logicFrames.Count <= 0)
        {
            return;
        }
        //run logic frame
        var logicFrame = logicFrames.Dequeue();
        switch (logicFrame.opId)
        {
            case OPType.CreateAgent:
                var instance = PostUpdateCommands.Instantiate(prefab);
                PostUpdateCommands.AddComponent(instance, new AgentComponent {
                    nexPosition = new float3(0),
                    position = new float3(0),
                    moveSpeed = 1
                });
                PostUpdateCommands.SetComponent(instance, new LocalToWorld() { Value = new float4x4(quaternion.identity, new float3(0, 0, 0)) });
                break;
            case OPType.Idle:
                break;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        logicFrames.Dispose(); 
    }
}
