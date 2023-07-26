using UnityEngine;
using MyBox;
using MyCode.Player.Inventory;
using MyCode.Data.Settings;
using System;

namespace MyCode.Data.GameSave
{
    [Serializable]
    public class GameSave
    {
            [Space]
            [Separator("Player data", true)]
            [Space]

        [Header("Checkpoint location")]
        [Space]
        [SerializeField, ReadOnly] private float[] _checkpointLocation = new float[3];
        [Space]

        [Header("Health")]
        [Space]
        [SerializeField] private float _health;
        [Space]

        [Header("Stamina")]
        [Space]
        [SerializeField] private float _currentStamina;
        [SerializeField] private bool _reachedLimit;

        [Header("Inventory")]
        [Space]
        [SerializeField] private InventoryObject _inventory;
        [SerializeField] private InventoryItem _primaryEquipment;
        [SerializeField] private InventoryItem _secondaryEquipment;

            [Space]
            [Separator("Game Settings")]
            [Space]

        [Header("Game difficulty")]
        [Space]
        [SerializeField] private DifficultyProperties _difficulty;

        [Space]
        [Separator("Save settings")]
        [Space]

        [Header("Save")]
        [Space]
        [SerializeField] private SaveIndex _saveIndex;


        //                   //
        // Player properties //
        //                   //


        // Location
        public float[] CheckpointLocation { get => _checkpointLocation; set => _checkpointLocation = value; }

        // Health
        public float Health { get => _health; set => _health = value; }

        // Stamina
        public float CurrentStamina { get => _currentStamina; set => _currentStamina = value; }
        public bool ReachedLimit { get => _reachedLimit; set => _reachedLimit = value; }

        // Inventory
        public InventoryObject Inventory { get => _inventory; set => _inventory = value; }
        public InventoryItem PrimaryEquipment { get => _primaryEquipment; set => _primaryEquipment = value; }
        public InventoryItem SecondaryEquipment { get => _secondaryEquipment; set => _secondaryEquipment = value; }


        //               //
        // Game settings //
        //               //


        // Difficulty
        public DifficultyProperties Difficulty { get => _difficulty; set => _difficulty = value; }

        //               //
        // Save settings //
        //               //


        // Save indicator
        public SaveIndex SaveIndex { get => _saveIndex; set => _saveIndex = value; }


        public void SetPlayer(float[] _spawnLocation, float _health, float _stamina, bool _reachedStaminaLimit, InventoryObject _inventory, InventoryItem _pEquipment, InventoryItem _sEquipment)
        {
            this._checkpointLocation = _spawnLocation;
            this._health = _health;
            this._currentStamina = _stamina;
            this._reachedLimit = _reachedStaminaLimit;
            this._inventory = _inventory;
            this._primaryEquipment = _pEquipment;
            this._secondaryEquipment = _sEquipment;
        }

        public void SetDifficulty(DifficultyProperties _properties)
        {
            this._difficulty = _properties;
        }

        public void SetSaveIndex(SaveIndex _index)
        {
            _saveIndex = _index;
        }
    
    }
}
