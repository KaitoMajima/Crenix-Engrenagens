# Devlog 5 - Engrenagens nos Encaixes

Agora, a implementação do das engrenagens nos encaixes. Já temos a detecção da engrenagem ao ser dropada no encaixe, então agora fica super fácil para implementar algo baseado nesses **eventos**. Falando em eventos, adicionei um evento que é chamado na classe `SlotDetection` quando algo for dropado no encaixe:

```csharp
public class SlotDetection : MonoBehaviour, IDropHandler, IPointerEnterHandler
{
	private bool _occupied;

	public Action<Item> DropDetected;

	public void OnDrop(PointerEventData eventData)
	{
		if(_occupied)
			return;

		var dragObj = eventData.pointerDrag;
		if(!dragObj.TryGetComponent(out InventoryItem itemInstance))
			return;

		var item = itemInstance.Item;
		DropDetected?.Invoke(item);
		itemInstance.RemoveFromInventory();

		_occupied = true;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		var dragObj = eventData.pointerDrag;
		if(dragObj == null)
			return; 
	}
}
```
A variável booleana `occupied` serve para não preencher o mesmo slot duas vezes. Com esse evento podemos inscrevê-lo em outras classes, criarei uma classe chamada `GearSlot` para tudo relacionado à engrenagem:

```csharp
public class GearSlot : MonoBehaviour
{
	[SerializeField] private SlotDetection _slotDetection;
	[SerializeField] private GameObject _gearPrefab;
	[SerializeField] private Transform _gearSocketTransform;

	private WorldGear _currentGear;

	public Action GearInserted;

	private void Start()
	{
		_slotDetection.DropDetected += OnGearDetected;
	}

	private void OnGearDetected(Item item)
	{
		var gearObj = Instantiate(_gearPrefab, _gearSocketTransform);
		_currentGear = gearObj.GetComponent<WorldGear>();

		_currentGear.SetColor(item.spriteColor);
		GearInserted?.Invoke();

	}

	public void IntitiateGearRotation()
	{
		_currentGear.Rotate();
	}
}
```

A classe irá instanciar o prefab da engrenagem "do mundo" no Transform especificado. Criei um evento que é chamado quando a engrenagem é encaixada com sucesso,
e uma função `InitiateGearRotation()` para ser chamada quando todas as engrenagens forem encaixadas. 

A engrenagem possui um script `WorldGear`. É nele onde posso ajustar a cor do sprite e o movimento de rotação quando for necessário. 
P.S. A classe Rotate é uma classe customizada que exportei de uma package pessoal que contém integrações com o **DOTween**. Para mais informações, acesse a pasta Assets/\_src/Scripts/TweenControllers.

```csharp
public class WorldGear : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _gearSpriteRenderer;
	[SerializeField] private Rotate _rotateTween;

	public WorldGear SetColor(Color color)
	{
		_gearSpriteRenderer.color = color;
		return this;
	}

	public void Rotate()
	{
		_rotateTween.CallTween();
	}
}

```

![Inspector1](https://user-images.githubusercontent.com/68963406/138773932-12dd4a10-3579-46ef-9f5b-af9457ee537d.png)

(Também apliquei uma classe `Scale` que começa no OnEnable)

E com isso, já podemos encaixar as engrenagens em seus devidos lugares!
### Esse é o resultado até agora:
![Gameplay6](https://user-images.githubusercontent.com/68963406/138773948-b9268e52-f9d3-40be-9a37-2d23a0969871.gif)


Agora, precisamos de algum meio para detectar quando todas as engrenagens estiverem encaixadas. Para isso, criei uma classe chamada `GearSlotMaster`. Nela possui todas as referências aos slots (classe `GearSlot`), que recebe o evento quando são encaixadas corretamente. Se o número de engrenagens for igual ao número de slots, ou seja, todas as slots forem preenchidas, a classe irá iniciar o processo de rotação das engrenagens. 
Dei um pequeno intervalo de ativação para cada engrenagem usando uma coroutine para deixar um pouco mais interessante.
```csharp
public class GearSlotsMaster : MonoBehaviour
{
	[SerializeField] private List<GearSlot> _gearSlots;
	[SerializeField] private float _rotationInterval = 0.25f;

	private int _insertedGearCount;

	public Action AllInsertSequenceInitiated;
	public Action AllInsertSequenceFinished;

	private void Start()
	{
		foreach (var gearSlot in _gearSlots)
		{
			gearSlot.GearInserted += OnGearInserted;
		}

	}

	private void OnGearInserted()
	{
		_insertedGearCount++;

		if(_insertedGearCount >= _gearSlots.Count)
			StartCoroutine(InitiateRotatingSequence());

	}

	private IEnumerator InitiateRotatingSequence()
	{
		AllInsertSequenceInitiated?.Invoke();
		
		foreach (var gear in _gearSlots)
		{
			gear.IntitiateGearRotation();
			yield return new WaitForSeconds(_rotationInterval);
		}
		AllInsertSequenceFinished?.Invoke();

	}

}
```
### Esse é o resultado até agora:
![Gameplay7](https://user-images.githubusercontent.com/68963406/138773962-87747fb2-624e-4c50-a704-0994071bbf4e.gif)
