using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCameraMove : CinemachineExtension
{
    public CinemachineConfiner Cc;

    public RoomManager rm;
    public string Roomname;

    public PolygonCollider2D ply;
    public PolygonCollider2D VillagePly;
    //public PolygonCollider2D BossPly;

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, 
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Cc.m_ConfineMode = CinemachineConfiner.Mode.Confine2D;

        rm = FindObjectOfType<RoomManager>();      
    }
    // Update is called once per frame
    void Update()
    {
        Roomname = rm.Roomname;
        if (Roomname != "")
        {
            //Invoke("StartConfiner", 1f);
            StartConfiner();
        }
        else
        {
            Cc.m_BoundingShape2D = VillagePly;
        }
    }

    public void StartConfiner()
    {
        ply = GameObject.Find(Roomname).GetComponentInChildren<PolygonCollider2D>();
        Cc.m_BoundingShape2D = ply;
    }

}
