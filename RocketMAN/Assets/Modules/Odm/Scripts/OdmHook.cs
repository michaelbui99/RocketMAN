using System;
using Modules.Shared.Cooldown;
using UnityEngine;

namespace Modules.Odm.Scripts
{
    public class OdmHook : MonoBehaviour
    {
        private CooldownHandler _cooldownHandler;

        private void Awake()
        {
            _cooldownHandler = GetComponent<CooldownHandler>() ?? gameObject.AddComponent<CooldownHandler>();
        }
        
    }
}