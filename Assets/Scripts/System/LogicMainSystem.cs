using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;

[DisableAutoCreation]
public class LogicSystem : ComponentSystem
{
    public NativeQueue<LogicComfirmedFrame> logicFrames;

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

    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        logicFrames.Dispose(); 
    }
}
