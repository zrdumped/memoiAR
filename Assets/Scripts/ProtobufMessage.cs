using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;

public class ProtobufMessage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ProtoContract]
    public class Information
    {
        [ProtoMember(1)]
        //0 - start program; 1 - start recording(b); 2 - start tracing(r); 3 - exit(e); -1 idle
        public string testString { get; set; }
    }
}
