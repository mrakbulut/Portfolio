using System.Collections.Generic;
using System.Linq;
using Portfolio.Utility;
using UnityEngine;

namespace Portfolio.Unit
{
    public class StatusEffectManager
    {
        private readonly Dictionary<SerializableGuid, StatusEffectInstance> _activeEffects = new Dictionary<SerializableGuid, StatusEffectInstance>();
        private readonly Dictionary<SerializableGuid, List<StatusEffectInstance>> _inactiveEffects = new Dictionary<SerializableGuid, List<StatusEffectInstance>>();

        private readonly List<(SerializableGuid id, int level)> _effectsToRemove = new List<(SerializableGuid, int)>();
        private readonly List<StatusEffect> _effectsToAdd = new List<StatusEffect>();

        private bool _removeAllEffectsNeeded;
        private IUnit _unit;

        public StatusEffectManager(IUnit unit)
        {
            _unit = unit;
        }

        public void Refresh()
        {
            _removeAllEffectsNeeded = true;
        }

        public void AddEffect(StatusEffect newEffect)
        {
            _effectsToAdd.Add(newEffect);
        }

        private void ApplyNewEffect(StatusEffect newEffect)
        {
            // Debug.Log("ACTIVE EFFECT COUNT : " + _activeEffects.Count + ", ID : " + newEffect.Data.Id.ToHexString() + ", CONTAINS : " + _activeEffects.ContainsKey(newEffect.Data.Id));
            if (_activeEffects.TryGetValue(newEffect.Data.Id, out var activeInstance))
            {
                HandleExistingEffect(newEffect, activeInstance);
            }
            else
            {
                var newInstance = new StatusEffectInstance(newEffect, _unit);
                newEffect.ApplyEffect(newInstance.Target);
                _activeEffects[newEffect.Data.Id] = newInstance;
            }
        }

        private void HandleExistingEffect(StatusEffect newEffect, StatusEffectInstance activeInstance)
        {
            if (newEffect.Level > activeInstance.Effect.Level)
            {
                DeactivateEffect(activeInstance);
                var newInstance = new StatusEffectInstance(newEffect, _unit);
                newEffect.ApplyEffect(newInstance.Target);
                _activeEffects[newEffect.Data.Id] = newInstance;
            }
            else if (newEffect.Level < activeInstance.Effect.Level)
            {
                AddToInactiveEffects(new StatusEffectInstance(newEffect, _unit));
            }
            else if (newEffect.Data.IsStackable)
            {
                // Handle stacking logic here if needed
            }
            else
            {
                activeInstance.Effect.Refresh();
            }
        }

        private void DeactivateEffect(StatusEffectInstance instance)
        {
            instance.Effect.RemoveEffect(instance.Target);
            _activeEffects.Remove(instance.Effect.Data.Id);
            //Debug.Log("REMOVED ACTIVE EFFECT : " + instance.Effect, instance.Target);
        }

        private void AddToInactiveEffects(StatusEffectInstance instance)
        {
            if (!_inactiveEffects.ContainsKey(instance.Effect.Data.Id))
            {
                _inactiveEffects[instance.Effect.Data.Id] = new List<StatusEffectInstance>();
            }
            _inactiveEffects[instance.Effect.Data.Id].Add(instance);
        }

        public void Tick(float deltaTime)
        {
            if (_removeAllEffectsNeeded) RemoveAllEffects();

            UpdateActiveEffects();
            ProcessEffectsToRemove();
            ProcessEffectsToAdd();
        }

        private void UpdateActiveEffects()
        {
            foreach (var kvp in _activeEffects)
            {
                var instance = kvp.Value;
                instance.Effect.UpdateEffect(instance.Target, Time.deltaTime);
                instance.UpdateDuration(Time.deltaTime);

                if (instance.IsExpired())
                {
                    _effectsToRemove.Add((kvp.Key, instance.Effect.Level));
                }
            }
        }

        private void ProcessEffectsToRemove()
        {
            foreach ((var id, int level) in _effectsToRemove)
            {
                ActuallyRemoveEffect(id, level);
            }
            _effectsToRemove.Clear();
        }

        private void ProcessEffectsToAdd()
        {
            foreach (var effect in _effectsToAdd)
            {
                ApplyNewEffect(effect);
            }
            _effectsToAdd.Clear();
        }

        public void RemoveAllEffects()
        {
            var instancesToRemove = new List<StatusEffectInstance>();

            foreach (var kvp in _inactiveEffects)
            {
                instancesToRemove.AddRange(kvp.Value);
            }

            for (int i = instancesToRemove.Count - 1; i >= 0; i--)
            {
                ActuallyRemoveEffect(instancesToRemove[i].Effect.Data.Id, instancesToRemove[i].Effect.Level);
            }

            _inactiveEffects.Clear();

            foreach (var kvp in _activeEffects)
            {
                _effectsToRemove.Add((kvp.Key, kvp.Value.Effect.Level));
            }

            ProcessEffectsToRemove();

            _effectsToAdd.Clear();
            _removeAllEffectsNeeded = false;
        }

        public void RemoveEffect(SerializableGuid effectId, int level)
        {
            //Debug.Log("REMOVING : " + effectId.ToHexString());
            _effectsToRemove.Add((effectId, level));
        }

        private void ActuallyRemoveEffect(SerializableGuid effectId, int level)
        {
            if (_activeEffects.TryGetValue(effectId, out var activeInstance) && activeInstance.Effect.Level == level)
            {
                DeactivateEffect(activeInstance);
                ActivateNextHighestEffect(effectId);
            }
            else if (_inactiveEffects.TryGetValue(effectId, out var inactiveInstances))
            {
                var instanceToRemove = inactiveInstances.FirstOrDefault(i => i.Effect.Level == level);
                if (instanceToRemove != null)
                {
                    inactiveInstances.Remove(instanceToRemove);
                    if (inactiveInstances.Count == 0)
                    {
                        _inactiveEffects.Remove(effectId);
                    }
                }
            }
        }

        private void ActivateNextHighestEffect(SerializableGuid effectId)
        {
            if (!_inactiveEffects.TryGetValue(effectId, out var inactiveInstances) || inactiveInstances.Count == 0) return;

            var highestLevelInstance = inactiveInstances.OrderByDescending(ei => ei.Effect.Level).First();
            inactiveInstances.Remove(highestLevelInstance);

            if (inactiveInstances.Count == 0)
            {
                _inactiveEffects.Remove(effectId);
            }

            ApplyNewEffect(highestLevelInstance.Effect);
        }
    }
}
