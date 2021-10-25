# Devlog 3 - Drag N' Drop

Implementei um Drag & Drop super simples só para um simples teste - O objetivo é poder arrastar os elementos da UI e fazer com que os elementos do mundo podem detectá-los e receber o input quando o jogador dropar o item do inventário em seu lugar.

A parte de arrastamento (Drag) do objeto foi super simples de implementar: Usando o EventSystems da Unity usando a interface `IDragHandler`, foi apenas preciso atualizar a posição do item conforme a posição do arrastamento do jogador:

```csharp
public class ItemMovement : MonoBehaviour, IDragHandler
{
	[SerializeField] private Transform _movingTransform;

	public void OnDrag(PointerEventData eventData)
	{
		_movingTransform.position = eventData.position;
	}
}
```

Já a parte do Drop foi um pouco mais complicada. Primeiro, como os elementos do mundo podem reconhecer que o jogador dropou um item em cima deles? Dropar elemento da UI em outro elemento da UI é fácil, só usar a interface `IDropHandler` no encaixe, mas esse não é o caso. Pensei em várias implementações como triggers usando circle colliders ou detecção através do `Overlay` da classe `Physics2D`... Mas algo mais simples me veio à cabeça: 

O jogo Slay the Spire (roguelike card-builder) possui uma checagem similar ao arrastar a carta em um inimigo no mundo: Uma UI em *worldspace* aparece como indicador em cima do inimigo e então a ação é executada quando o jogador dropa a carta. Então, ao mesmo tempo que o encaixe é um elemento dentro do mundo, posso aplicar um canvas em world space com uma imagem invisível para detecção do `OnDrop` da interface `IDragHandler`. 

```csharp
public class SlotDetection : MonoBehaviour, IDropHandler, IPointerEnterHandler
{
	public void OnDrop(PointerEventData eventData)
	{
		var dragObj = eventData.pointerDrag;
		Debug.Log($"Você dropou o {dragObj.name} no encaixe!");
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		var dragObj = eventData.pointerDrag;
		if(dragObj == null)
			return;

		Debug.Log(dragObj.name + " está entrando na área do encaixe.");
	}
}
```

Ao implementar isso, outro problema surgiu: O `OnDrop` do encaixe não era detectado. Pesquisei um pouco sobre isso online e percebi que preciso aplicar um `CanvasGroup` na engrenagem e desabilitar raycasts enquanto estou arrastando-o, e reativá-lo quando soltar.

```csharp
public class ItemMovement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[SerializeField] private Transform _movingTransform;
	[SerializeField] private CanvasGroup _canvasGroup;

	public void OnBeginDrag(PointerEventData eventData)
	{
		_canvasGroup.blocksRaycasts = false;
	}
	public void OnDrag(PointerEventData eventData)
	{
		_movingTransform.position = eventData.position;
	}
	public void OnEndDrag(PointerEventData eventData)
	{
		_canvasGroup.blocksRaycasts = true;
	}
}
```


Usei um simples `Debug.Log()` no console para checar se tudo estava funcionando direitinho. 

### Esse é o resultado até agora:

![Gameplay1](https://user-images.githubusercontent.com/68963406/138773331-24331756-ccfe-49e6-a5b4-23fe8e8e49d4.gif)

Quando a engrenagem é dropada em algum lugar que não deve, ela volta para a posição padrão em que estava.
```csharp
public class ItemMovement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[SerializeField] private Transform _movingTransform;
	[SerializeField] private CanvasGroup _canvasGroup;
	[SerializeField] private Vector2 _defaultPosition;

	public void OnBeginDrag(PointerEventData eventData)
	{
		_canvasGroup.blocksRaycasts = false;
	}
	public void OnDrag(PointerEventData eventData)
	{
		_movingTransform.position = eventData.position;
	}
	public void OnEndDrag(PointerEventData eventData)
	{
		_canvasGroup.blocksRaycasts = true;
		_movingTransform.anchoredPosition = _defaultPosition;
	}
}
```

![Gameplay2](https://user-images.githubusercontent.com/68963406/138773353-ce1b18ae-9fc8-4a92-960e-1887a866ae26.gif)

