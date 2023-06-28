using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using Unity.Collections;

public class JobManager : MonoBehaviour
{
    List<JobHandle> jobs = new List<JobHandle>();
}
