using System;
using Portfolio.Utility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Portfolio.Leveling
{
    public class PlayerLevel : MonoBehaviour
    {
        [TitleGroup("Leveling Settings")]
        [Tooltip("Initial level value for the player")]
        [SerializeField] [Min(1)]
        private int _initialLevel = 1;
        [TitleGroup("Leveling Settings")]
        [Tooltip("Initial experience value for the player")]
        [SerializeField] [Min(0)]
        private int _initialExperience;

        [TitleGroup("Formula Settings")]
        [Tooltip("Base experience required for first level up")]
        [SerializeField] [Min(1)]
        private float _baseExperience = 100f;

        [Tooltip("Multiplier for experience required per level")]
        [SerializeField] [Range(1.1f, 2f)]
        private float _experienceMultiplier = 1.2f;

        private ILevelManager _levelManager;
        private IExperienceManager _experienceManager;
        private ILevelingFormulaStrategy _levelingFormula;

        private int _experienceToAdd;

        public Action OnLevelUp = () => { };

        private void Awake()
        {
            _experienceManager = new BasicExperienceManager(_initialExperience);
            _levelingFormula = new StandardLevelingFormula(_baseExperience, _experienceMultiplier);
            _levelManager = new LevelManager(_experienceManager, _levelingFormula, _initialLevel);

            _experienceManager.OnExperienceChanged += OnExperienceChanged;
            _levelManager.OnLevelChanged += OnPlayerLevelChanged;

            ServiceLocator.Instance.Register(this);
        }

        private void OnExperienceChanged(int experience)
        {
        }
        private void OnPlayerLevelChanged(int level)
        {

        }

        private void OnDestroy()
        {
            if (_levelManager != null)
            {
                _levelManager.OnLevelChanged -= OnPlayerLevelChanged;
            }
            if (_experienceManager != null)
            {
                _experienceManager.OnExperienceChanged -= OnExperienceChanged;
            }

            OnLevelUp = () => { };
        }

        public void AddExperience(int experienceToAdd)
        {
            _experienceToAdd += experienceToAdd;

            while (_experienceToAdd > 0)
            {
                int experienceToLevelUp = _levelManager.ExperienceRequiredForNextLevel - _experienceManager.CurrentExperience;
                if (experienceToLevelUp <= _experienceToAdd)
                {
                    _levelManager.GainExperience(experienceToLevelUp);
                    _experienceToAdd -= experienceToLevelUp;

                    bool levelUp = _levelManager.TryLevelUp();
                    if (levelUp)
                    {
                        OnLevelUp.Invoke();
                    }
                    else
                    {
                        Debug.LogError("Level up failed. LEVEL : " + _levelManager.CurrentLevel + ", CURRENT EXPERIENCE : " + _experienceManager.CurrentExperience + ", REQUIRED EXPERIENCE TO LEVEL UP : " + _levelManager.ExperienceRequiredForNextLevel + ", EXPERIENCE TO ADD : " + _experienceToAdd);
                        break;
                    }
                }
                else
                {
                    _levelManager.GainExperience(_experienceToAdd);
                    _experienceToAdd = 0;
                }
            }

            // TODO: Update Experience UI
        }


        #if UNITY_EDITOR
        [TitleGroup("Debug And Testing")]
        [Button("Add Experience", ButtonSizes.Large)]
        [GUIColor(0f, 0.8f, 0f)]
        public void AddExperienceEditor(int experienceToAdd)
        {
            AddExperience(experienceToAdd);
        }
        #endif
    }
}
