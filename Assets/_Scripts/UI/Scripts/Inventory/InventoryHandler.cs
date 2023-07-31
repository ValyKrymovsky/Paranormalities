using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using UnityEngine.InputSystem;
using MyCode.Managers;
using MyCode.GameData.Inventory;

namespace MyCode.UI.Inventory
{
    public class InventoryHandler : MonoBehaviour
    {

        //             //
        // UI Elements //
        //             //

        [Separator("Inventory UI", true)]
        [SerializeField] private UIDocument inventoryUI;
        public VisualElement root;
        public VisualElement descriptionElement;
        public VisualElement descriptionImage;
        public Label descriptionText;

        [Space]
        [Header("Inventory Slots")]
        public VisualElement inventoryGrid;
        public List<InventorySlot> inventorySlots = new List<InventorySlot>();

        [Space]
        [Header("Equipment Slots")]
        private VisualElement equipment;
        private VisualElement equipmentImage;
        public InventorySlot primarySlot;
        public InventorySlot secondarySlot;

        [Space]
        [Header("Ghost Icon")]
        private static VisualElement ghostIcon;
        private static bool isDragging;
        private static InventorySlot originalSlot;

        [SerializeField] private InputActionReference _input_DropItem;
        [SerializeField] private InputActionReference _input_ToggleInventory;

        //                  //
        // Input Controller //
        //                  //

        private PlayerManager _pm;
        private void Awake()
        {

            //             //
            // UI Elements //
            //             //

            inventoryUI = GetComponent<UIDocument>();
            root = inventoryUI.rootVisualElement;

            descriptionElement = root.Q<VisualElement>("ItemDescription");
            descriptionImage = descriptionElement.Q<VisualElement>("Image");
            descriptionText = descriptionElement.Q<Label>("Description");

            inventoryGrid = root.Q<VisualElement>("InventoryGrid");

            equipment = root.Q<VisualElement>("Equipment");
            equipmentImage = equipment.Q<VisualElement>("EquipmentImage");
            primarySlot = equipment.Q<InventorySlot>("PrimarySlot");
            secondarySlot = equipment.Q<InventorySlot>("SecondarySlot");

            ghostIcon = root.Q<VisualElement>("GhostIcon");

            foreach (InventorySlot inventorySlot in inventoryGrid.Children())
            {
                inventorySlots.Add(inventorySlot);
            }

            inventorySlots.Add(primarySlot);
            inventorySlots.Add(secondarySlot);

            foreach (InventorySlot inventorySlot in inventorySlots)
            {
                inventorySlot.RegisterCallback<MouseOverEvent>((type) =>
                {
                    if (inventorySlot.item != InventoryItem.empty)
                    {
                        SetDescription(inventorySlot.item.Item.description, inventorySlot.slotImage.style.backgroundImage.value.sprite);
                    }
                });

                inventorySlot.RegisterCallback<MouseOutEvent>((type) =>
                {
                    if (inventorySlot.item != InventoryItem.empty)
                        ResetDescription();
                });
            }

            root.style.display = DisplayStyle.None;

            ghostIcon.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            ghostIcon.RegisterCallback<PointerUpEvent>(OnPointerUp);

            //                  //
            // Input Controller //
            //                  //

            _pm = PlayerManager.Instance;


        }

        private void OnEnable()
        {
            _input_ToggleInventory.action.Enable();
            _input_DropItem.action.Enable();

            _input_ToggleInventory.action.performed += ToggleInventoryUI;
            _pm.InventoryData.OnAddItem += AddItemToUI;
        }

        private void OnDisable()
        {
            _input_ToggleInventory.action.Disable();
            _input_DropItem.action.Disable();

            _input_ToggleInventory.action.performed -= ToggleInventoryUI;
            _pm.InventoryData.OnAddItem -= AddItemToUI;
        }

        private void SetDescription(string _description, Sprite _itemImage)
        {
            descriptionImage.style.backgroundImage = new StyleBackground(_itemImage);
            descriptionText.text = _description;
        }

        private void ResetDescription()
        {
            descriptionImage.style.backgroundImage = null;
            descriptionText.text = null;
        }

        public static void StartDrag(Vector2 _position, InventorySlot _originalInventorySlot)
        {
            isDragging = true;
            originalSlot = _originalInventorySlot;

            ghostIcon.style.top = _position.y - ghostIcon.layout.height / 2;
            ghostIcon.style.left = _position.x - ghostIcon.layout.width / 2;

            ghostIcon.style.backgroundImage = new StyleBackground(_originalInventorySlot.GetItemImage());

            ghostIcon.style.visibility = Visibility.Visible;
        }

        private void OnPointerMove(PointerMoveEvent _event)
        {
            if (!isDragging) return;

            ghostIcon.style.top = _event.position.y - ghostIcon.layout.height / 2;
            ghostIcon.style.left = _event.position.x - ghostIcon.layout.width / 2;
        }

        private void OnPointerUp(PointerUpEvent _event)
        {
            if (!isDragging) return;

            IEnumerable<InventorySlot> overlapingSlots = inventorySlots.Where(x => x.worldBound.Overlaps(ghostIcon.worldBound));

            if (overlapingSlots.Count() == 0)
            {
                ReturnToOriginalSlot(originalSlot);
                StopDragging();
                return;
            }


            InventorySlot closestSlot = overlapingSlots.OrderBy(x => Vector2.Distance(x.worldBound.position, ghostIcon.worldBound.position)).First();
            InventoryItem originalSlotItem = new InventoryItem(originalSlot.item.ItemId, originalSlot.item.Item, originalSlot.item.Model, ghostIcon.style.backgroundImage.value.sprite);

            // Returns item to original slot if the item is not equipment and is trying to go to equipment slots
            if (originalSlot.item.Item.itemType != ItemObject.ItemType.Equipment &&
            closestSlot.name != "InventorySlot")
            {
                ReturnToOriginalSlot(originalSlot);
                StopDragging();
                return;
            }

            // Swaps equipment items in primary and secondary equipment slots
            if (originalSlot.name != "InventorySlot" &&
                (closestSlot.name != "InventorySlot" && closestSlot.name != originalSlot.name))
            {
                InventoryItem closestSlotItem = new InventoryItem(closestSlot.item.ItemId, closestSlot.item.Item, closestSlot.item.Model, closestSlot.GetItemImage());

                SwapEquipment(originalSlot, closestSlot, originalSlotItem, closestSlotItem);
                StopDragging();
                return;
            }

            // Returns item to original slot when the new slot is not empty
            if (!closestSlot.item.Equals(InventoryItem.empty))
            {
                ReturnToOriginalSlot(originalSlot);
                StopDragging();
                return;
            }

            // Sets the new slot with the original slot item and empties the original slot
            closestSlot.SetItemParameters(originalSlotItem);
            originalSlot.ResetParameters();

            StopDragging();

        }

        private static void StopDragging()
        {
            isDragging = false;
            originalSlot = null;
            ghostIcon.style.visibility = Visibility.Hidden;
        }

        private void ReturnToOriginalSlot(InventorySlot _originalSlot)
        {
            _originalSlot.SetSlotImage(ghostIcon.style.backgroundImage.value.sprite);
        }

        private void SwapEquipment(InventorySlot _originalSlot, InventorySlot _newSlot, InventoryItem _originalItem, InventoryItem _newItem)
        {
            _newSlot.SetItemParameters(_originalItem);
            _originalSlot.SetItemParameters(_newItem);

            InventoryItem tempItem = PlayerManager.Instance.InventoryData.PrimaryEquipment;

            PlayerManager.Instance.InventoryData.PrimaryEquipment = PlayerManager.Instance.InventoryData.SecondaryEquipment;
            PlayerManager.Instance.InventoryData.SecondaryEquipment = tempItem;
        }

        private void AddItemToUI(InventoryItem _item)
        {
            InventorySlot tmp = _item.Item.itemType == ItemObject.ItemType.Equipment ? GetEmptyEquipmentSlot() : GetFirstEmptySlot();
            tmp.SetItemParameters(_item);
        }

        private InventorySlot GetFirstEmptySlot()
        {
            foreach (InventorySlot inventorySlot in inventorySlots)
            {
                if (inventorySlot.item.Equals(InventoryItem.empty))
                {
                    return inventorySlot;
                }
                continue;
            }
            return null;
        }

        private InventorySlot GetEmptyEquipmentSlot()
        {
            if (primarySlot.item.Equals(InventoryItem.empty))
            {
                return primarySlot;
            }
            else if (secondarySlot.item.Equals(InventoryItem.empty))
            {
                return secondarySlot;
            }

            return null;
        }

        private void ToggleInventoryUI(InputAction.CallbackContext _ctx)
        {
            if (root.style.display == DisplayStyle.Flex)
            {
                if (PlayerManager.Instance.MovementData.FreezeOnInventory)
                    Time.timeScale = 1;

                root.style.display = DisplayStyle.None;
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                PlayerManager.Instance.InventoryData.InvokeOnInventoryStatusChange(false);
                return;
            }

            if (PlayerManager.Instance.MovementData.FreezeOnInventory)
                Time.timeScale = 0;

            root.style.display = DisplayStyle.Flex;
            UnityEngine.Cursor.lockState = CursorLockMode.Confined;
            PlayerManager.Instance.InventoryData.InvokeOnInventoryStatusChange(true);
        }
    }

}
