using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMorningCache
{
    bool IsActive { get; set; }
}
public interface IRun : IMorningCache
{
    void Run();
}
public interface IFixedRun : IMorningCache
{
    void FixedRun();
}
