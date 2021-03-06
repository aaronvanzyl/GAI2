using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityIDRenderer : PropertyRenderer
{
    public int entityID;
    public Text entityIDText;

    public override void Render(IReadOnlyWorld world, IReadOnlyDictionary<int, int> varDict)
    {
        base.Render(world, varDict);
        IReadOnlyEntity entity = world.GetReadOnlyEntity(entityID);
        entityIDText.text = $"{entity.name} [{entityID}]";
    }
}
