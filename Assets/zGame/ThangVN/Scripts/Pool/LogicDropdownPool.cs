using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicDropdownPool : ObjectPool<LogicDropdown>
{
    public LogicDropdown GetPooledDropdown()
    {
        return GetPooledObject();
    }
}
