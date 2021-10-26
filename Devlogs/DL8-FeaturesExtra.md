# Devlog 8 - Features Extra

Agora, falta adicionar algumas restrições que esqueci de levar em conta.

- As engrenagens superiores devem girar em sentido horário, e as inferiores em
sentido anti-horário.

Inverti a orientação do tween de rotação para chegar a esse efeito, mas isso é decidido por cada slot, então o slot passa esse parâmetro para a engrenagem instanciada.

```csharp
public class GearSlot : MonoBehaviour
{
	[SerializeField] private bool _reverseRotationForGears;
	private WorldGear _currentGear;

	private void Start()
	{
		_slotDetection.DropDetected += OnGearDetected;
	}

	public void IntitiateGearRotation()
	{
		_currentGear.Rotate(_reverseRotationForGears);
	}

}
```
```csharp
public class WorldGear : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _gearSpriteRenderer;
	[SerializeField] private Rotate _rotateTween;

	public void Rotate(bool clockwise)
	{
		_rotateTween.tweenSettings.isInverted = clockwise;
		_rotateTween.CallTween();
	}
}
```
![Gameplay10](https://user-images.githubusercontent.com/68963406/138774403-2fa87c1d-f1c7-4b9d-b5ea-ab23abdac8b5.gif)


- Ao remover qualquer engrenagem, todas devem parar de girar.

Primeiramente, fiz uma detecção de clique na classe `RemovalDetection`, parecido com a classe `SlotDetection`:

```csharp
public class RemovalDetection : MonoBehaviour, IPointerClickHandler
{
	public Action RemoveCalled;
	public void OnPointerClick(PointerEventData eventData)
	{
		RemoveCalled?.Invoke();
	}
}
```
Os eventos sobem até a classe `GearSlotMaster`:
```csharp
public class GearSlot : MonoBehaviour
{
	[SerializeField] private SlotDetection _slotDetection;
	[SerializeField] private RemovalDetection _removalDetection;

	private WorldGear _currentGear;

	public Action GearInserted;
	public Action<Item> GearRemoved;

	private void Start()
	{
		_slotDetection.DropDetected += OnGearDetected;
		_removalDetection.RemoveCalled += OnRemoveGearCalled;
	}

	private void OnRemoveGearCalled()
	{
		if(_currentGear == null)
			return;

		var item = _currentGear.item;

		GearRemoved?.Invoke(item);
		ResetGear();
	}
	public void ResetGear()
	{
		if(_currentGear != null)
			Destroy(_currentGear.gameObject);

		_currentGear = null;
		_slotDetection.Occupied = false;

	}
	public void StopGearRotation()
	{
		if(_currentGear == null) 
			return;

		_currentGear.StopRotating();
	}
}
```

Precisei colocar uma dependência ao `Inventory` na classe `GearSlotMaster` para retornar a engrenagem para o inventário do jogador. E então, todas as engrenagens param de girar se ao menos uma engrenagem for removida. 

```csharp
public class GearSlotsMaster : MonoBehaviour
{
	[SerializeField] private List<GearSlot> _gearSlots;
	[SerializeField] private Inventory _inventory;

	private int _insertedGearCount;
	private Coroutine _rotatingSequence;

	private void Start()
	{
		foreach (var gearSlot in _gearSlots)
		{
			gearSlot.GearInserted += OnGearInserted;
			gearSlot.GearRemoved += OnGearRemoved;
		}
	}

	private void OnGearRemoved(Item item)
	{
		_insertedGearCount--;

		if(_rotatingSequence != null)
			StopCoroutine(_rotatingSequence);

		_inventory.AddItem(item);
		_inventory.UpdateInventory();

		foreach (var slot in _gearSlots)
		{
			slot.StopGearRotation();
		}

	}

}
```
![Gameplay11](https://user-images.githubusercontent.com/68963406/138774424-2c464e1f-4ecd-4723-b820-496d07fc2422.gif)

