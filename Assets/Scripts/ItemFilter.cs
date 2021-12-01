using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFilter
{
    public string name;

    public bool Satisfied(IReadOnlyItem item) {
        if (name != "" && item.name != name) {
            return false;
        }
        return true;
    }
}
