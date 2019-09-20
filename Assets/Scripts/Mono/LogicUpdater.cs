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
    public OPType opId;
}

public class LogicUpdater : MonoBehaviour
{
    private LogicSystem logicMainSystem;
    public GameObject agentPrefab;
    void Awake()
    {
        Time.fixedDeltaTime = 0.05f;
    }

    void FixedUpdate()
    {
        if (logicMainSystem == null)
        {
            logicMainSystem = World.Active.GetOrCreateSystem<LogicSystem>();
            var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(agentPrefab, World.Active);
            logicMainSystem.prefab = prefab;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            logicMainSystem.logicFrames.Enqueue(new LogicComfirmedFrame { opId =  OPType.CreateAgent });
        }
        else
        {
            logicMainSystem.logicFrames.Enqueue(new LogicComfirmedFrame { opId = OPType.Idle });
        }
        logicMainSystem.Update();
    }
}
