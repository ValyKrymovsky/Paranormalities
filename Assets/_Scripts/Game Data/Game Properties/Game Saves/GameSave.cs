using UnityEngine;
using MyBox;
using MyCode.GameData.Inventory;
using MyCode.GameData.GameSettings;
using System;

namespace MyCode.GameData.GameSave
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
        [SerializeField] private InventoryItem[] _inventory;
        [SerializeField] private InventoryItem _primaryEquipment;
        [SerializeField] private InventoryItem _secondaryEquipment;

            [Space]
            [Separator("Game Settings")]
            [Space]

        [Header("Game difficulty")]
        [Space]
        [SerializeField] private DifficultyProperties _difficultyProperties;

        [Space]
        [Separator("Save settings")]
        [Space]

        [Header("Save")]
        [Space]
        [SerializeField] private SaveIndex _saveIndex;
        [SerializeField] private string _saveName;
        [SerializeField, ReadOnly] private DateTime _saveTime;
        private string _savePath;


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
        public InventoryItem[] Inventory { get => _inventory; set => _inventory = value; }
        public InventoryItem PrimaryEquipment { get => _primaryEquipment; set => _primaryEquipment = value; }
        public InventoryItem SecondaryEquipment { get => _secondaryEquipment; set => _secondaryEquipment = value; }


        //               //
        // Game settings //
        //               //


        // Difficulty
        public DifficultyProperties Difficulty { get => _difficultyProperties; set => _difficultyProperties = value; }

        //               //
        // Save settings //
        //               //


        public SaveIndex SaveIndex { get => _saveIndex; set => _saveIndex = value; }
        public string SaveName { get => _saveName; set => _saveName = value; }
        public DateTime SaveTime { get => _saveTime; set => _saveTime = value; }
        public string SavePath { get => _savePath; set => _savePath = value; }

        public void SetPlayer(float[] _spawnLocation, float _health, float _stamina, bool _reachedStaminaLimit, InventoryItem[] _inventory, InventoryItem _pEquipment, InventoryItem _sEquipment)
        {
            this._checkpointLocation = _spawnLocation;
            this._health = _health;
            this._currentStamina = _stamina;
            this._reachedLimit = _reachedStaminaLimit;
            this._inventory = _inventory;
            this._primaryEquipment = _pEquipment;
            this._secondaryEquipment = _sEquipment;
        }
    
    }
}

