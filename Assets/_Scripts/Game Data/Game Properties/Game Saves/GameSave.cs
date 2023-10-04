using UnityEngine;
using MyBox;
using System;

namespace MyCode.GameData
{
    public enum SaveIndex
    {
        entrance,
        stairs,
        dungeons
    }

    [Serializable]
    public class GameSave
    {
            [Space]
            [Separator("Player data", true)]
            [Space]

        [Header("Checkpoint location")]
        [Space]
        [SerializeField, ReadOnly] private SerializableVector3 _checkpointLocation;
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
        [SerializeField] private Inventory _inventory;

            [Space]
            [Separator("Game Settings")]
            [Space]

        [Header("Game difficulty")]
        [Space]
        [SerializeField] private Difficulty _gameDifficulty;

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
        public SerializableVector3 CheckpointLocation { get => _checkpointLocation; set => _checkpointLocation = value; }

        // Health
        public float Health { get => _health; set => _health = value; }

        // Stamina
        public float CurrentStamina { get => _currentStamina; set => _currentStamina = value; }
        public bool ReachedLimit { get => _reachedLimit; set => _reachedLimit = value; }

        // Inventory
        public Inventory Inventory { get => _inventory; set => _inventory = value; }

        //               //
        // Game settings //
        //               //


        // Difficulty
        public Difficulty GameDifficulty { get => _gameDifficulty; set => _gameDifficulty = value; }

        //               //
        // Save settings //
        //               //


        public SaveIndex SaveIndex { get => _saveIndex; set => _saveIndex = value; }
        public string SaveName { get => _saveName; set => _saveName = value; }
        public DateTime SaveTime { get => _saveTime; set => _saveTime = value; }
        public string SavePath { get => _savePath; set => _savePath = value; }

        public void SetPlayerProperties(PlayerMovementData _movementData, PlayerInventoryData _inventoryData)
        {
            _checkpointLocation = SerializableVector3.Zero;

            _inventory = _inventoryData.Inventory;
        }
    
    }
}

