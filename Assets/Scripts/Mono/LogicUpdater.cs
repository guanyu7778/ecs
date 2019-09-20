using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public enum OPType
{
    Idle = 0,
    CreateAgent = 1
}

[SerializeField]
public struct LogicComfirmedFrame
{
    public int opId;
}

public class LogicUpdater : MonoBehaviour
{
    private LogicSystem logicMainSystem;
    void Awake()
    {
        Time.fixedDeltaTime = 0.05f;
    }

    void FixedUpdate()
    {
        if (logicMainSystem == null)
        {
            logicMainSystem = World.Active.GetOrCreateSystem<LogicSystem>();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            logicMainSystem.logicFrames.Enqueue(new LogicComfirmedFrame { opId = 1 });
        }
        else
        {
            logicMainSystem.logicFrames.Enqueue(new LogicComfirmedFrame { opId = 0 });
        }
        logicMainSystem.Update();
    }
}
