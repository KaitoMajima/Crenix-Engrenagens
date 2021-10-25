# Devlog 4 - Inventário

Agora, preciso implementar o sistema do inventário interno. Para poder trocar as peças de lugar, ou substituir o lugar de uma peça para a outra, precisamos ter uma espécie de sistema de inventário para categorizar os índices das peças.

Primeiramente, criei um `ScriptableObject` como um container de dados para os itens no inventário na classe `Item`:

```csharp
[CreateAssetMenu(fileName = "New Item", menuName = "KaitoMajima/Item")]
public class Item : ScriptableObject
{
	public Sprite itemSprite;
	public Color spriteColor = Color.white;
}
```

A classe guarda o sprite do item e a sua cor, nada de mais. Como a classe é derivada de um `ScriptableObject`, preciso criar os itens em uma pasta:
![Project2](https://user-images.githubusercontent.com/68963406/138773526-e81b13c4-1fc8-49bc-b3ce-247bed587ccb.png)

Separei o inventário em duas partes - O sistema interno do inventário e sistema da UI do inventário. Dessa maneira, posso fazer um sistema onde a UI reage pelas mudanças do inventário interno, se "inscrevendo" a um evento `InventoryUpdated`.

```csharp
public class Inventory : MonoBehaviour
{
	[SerializeField] private List<Item> _items;
	public Action<List<Item>> InventoryUpdated;
	
	public void UpdateInventory()
	{
		InventoryUpdated?.Invoke(_items);
	}
	public bool AddItem(Item item, int index)
	{
		if(_items[index] != null)
		{
			Debug.LogWarning("Já existe um item nesse slot!");
			return false;
		}

 		_items[index] = item;
		return true;
	}
	public bool RemoveItem(int index)
	{
		_items[index] = null;
		return true;
	}
}
```

E então fui adicionando várias funções como acima para o gerenciamento do inventário:

```csharp
public bool SwapItem(Item swappingItem, int originalIndex, int newIndex)
{
	var swappedItem = _items[newIndex];

	RemoveItem(newIndex);
	RemoveItem(originalIndex);

	AddItem(swappingItem, newIndex);
	AddItem(swappedItem, originalIndex);

	return true;
}
```

A classe `InventoryUI` instancia um número de slots fixo determinado pela classe `Inventory`. E então, a cada vez que o inventário é atualizado, a classe chama cada slot para atualizar o item dentro dela.

```csharp
public class InventoryUI : MonoBehaviour
{
	[SerializeField] private Inventory _inventory;
	[SerializeField] private GameObject _slotPrefab;
	[SerializeField] private Transform _slotsTransform;

	private List<InventorySlot> _currentInventorySlots;

	private void Awake()
	{
		InitializeSlots();
		_inventory.InventoryUpdated += OnInventoryUpdated;
	}

	private void InitializeSlots()
	{
		_currentInventorySlots = new List<InventorySlot>();

		for (int i = 0; i < _inventory.Capacity; i++)
		{
			var slotObj = Instantiate(_slotPrefab, _slotsTransform);
			var slot = slotObj.GetComponent<InventorySlot>();

			slot.SetInventory(_inventory).
				SetIndex(i).
				Initialize();
			
			_currentInventorySlots.Add(slot);
		}
	}

	private void OnInventoryUpdated(List<InventoryItem> items)
	{
		for (int i = 0; i < _currentInventorySlots.Count; i++)
		{
			var slot = _currentInventorySlots[i];

			slot.SetItem(items[i]);
		}
	}
}
```
Cada slot do inventário possui uma classe `InventorySlot`, e cada instância de um item, uma classe `InventoryItem`.
No `InventorySlot`, o slot recebe um evento `OnDrop` que o jogador pode executar ao soltar uma engrenagem no slot. A classe recebe o índice do objeto dropado e troca com o objeto que está em baixo. Se não houver um objeto em baixo, é como se o objeto apenas trocasse de lugar.

```csharp
public class InventorySlot : MonoBehaviour, IDropHandler
{
	private Inventory _inventory;
	private InventoryItem _defaultItemInstance;

	public int index;

	public InventorySlot SetInventory(Inventory inventory)
	{
		_inventory = inventory;
		return this;
	}
	public InventorySlot SetIndex(int index)
	{
		this.index = index;
		return this;
	}
	public InventorySlot Initialize()
	{
		var itemInstanceObj = Instantiate(_defaultItemPrefab, transform);
		_defaultItemInstance = itemInstanceObj.GetComponent<InventoryItem>();

		return this;
	}
	public void OnDrop(PointerEventData eventData)
	{
		var dragObj = eventData.pointerDrag;

		if(dragObj == null)
			return;

		if(!dragObj.TryGetComponent(out InventoryItem itemInstance))
			return;

		var item = itemInstance.Item;
		var instanceIndex = itemInstance.currentSlot.index;

		_inventory.SwapItem(item, instanceIndex, index);
	}

}
```
E por fim, as engrenagens são removidas do inventário quando dropadas no encaixe. 


```csharp
public class SlotDetection : MonoBehaviour, IDropHandler, IPointerEnterHandler
{
	public void OnDrop(PointerEventData eventData)
	{
		var dragObj = eventData.pointerDrag;
		if(!dragObj.TryGetComponent(out InventoryItem itemInstance))
			return;

		itemInstance.RemoveFromInventory();

		Debug.Log($"Você dropou o {dragObj.name} no encaixe!");

	}
}
```
```csharp
public class InventoryItem : MonoBehaviour
{
	private Item _item;
	public Item Item {get => _item;}
	
	private InventorySlot _currentSlot;
	public InventorySlot CurrentSlot {get => _currentSlot;}

	private Inventory _inventory;

	public InventoryItem SetInventory(Inventory inventory)
	{
		_inventory = inventory;
		return this;
	}

	public InventoryItem SetSlot(InventorySlot slot)
	{
		_currentSlot = slot;
		return this;
	}
	
	public InventoryItem SetInfo(Item item)
	{
		_item = item;

		if(item == null)
		{
			_spriteImage.enabled = false;
			return this;
		}
		_spriteImage.enabled = true;
		_spriteImage.sprite = item.itemSprite;
		_spriteImage.color = item.spriteColor;

		return this;
	
 	}
	public void RemoveFromInventory()
	{
		_inventory.RemoveItem(_currentSlot.index);
		SetInfo(null);
	}
}
```

Um último problema que precisa ser resolvido é que as engrenagens ficam debaixo de outros elementos da UI quando arrastadas. 

![Gameplay3](https://user-images.githubusercontent.com/68963406/138773603-486c8d0e-5a4b-4091-a696-c142ae8983c6.gif)

Isso acontece pois dependendo do seu nível na hierarquia, elas são renderizadas primeiro, então por exemplo a Engrenagem 3 sempre ficará debaixo da Engrenagem 4.

Para consertar isso, a classe `ItemMovement` precisa de uma dependência a um Transform de um Canvas que fica em cima dos demais elementos. Ao arrastar a engrenagem, ela ficará por cima de todos os elementos da UI, e ao soltar, ela volta ao Transform original.

![Hierarchy1](https://user-images.githubusercontent.com/68963406/138773656-14ddb185-c69f-4893-992c-ba6f862227c2.png)

```csharp
// Classe ItemMovement

public Transform draggingObjectsTransform;

public void OnBeginDrag(PointerEventData eventData)
{
	_canvasGroup.blocksRaycasts = false;
	
	_movingTransform.SetParent(draggingObjectsTransform);
}

public void OnEndDrag(PointerEventData eventData)
{
	_canvasGroup.blocksRaycasts = true;

	_movingTransform.SetParent(_originalTransform);
	_movingTransform.anchoredPosition = _defaultPosition;

}
```
E com isso concluímos o inventário.
### Esse é o resultado até agora:
![Gameplay4](https://user-images.githubusercontent.com/68963406/138773679-591a49c6-bc25-4cb1-92d4-379a246d064e.gif)
![Gameplay5](https://user-images.githubusercontent.com/68963406/138773690-f3f2f3c6-a20b-42cf-bbd1-08ac48419de3.gif)


