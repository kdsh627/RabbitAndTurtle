using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Weapon", story: "Try Attack With [CurrentWeapon]", category: "Action", id: "2993594abf4c0e0611492dee58653a97")]
public partial class WeaponAction : Action
{
    [SerializeReference] public BlackboardVariable<WeaponBase> CurrentWeapon;

    
    protected override Status OnUpdate()
    {
        CurrentWeapon.Value.TryAttack();

        return Status.Success;
    }

  
}

