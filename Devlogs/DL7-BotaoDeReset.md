# Devlog 7 - Botão de Reset

Finalmente, criarei um script `ResetButton` para o botão de reset. Para que o jogo volte ao seu estado padrão, é preciso reverter as ações do `Inventory`, `GearSlotMaster` e do `Nugget`. Então para cada uma dessas classes criei uma função para resetar seus valores. 

```csharp
public class ResetButton : MonoBehaviour
{
	[SerializeField] private Button _button;
	[SerializeField] private GearSlotsMaster _gearSlotsMaster;
	[SerializeField] private Inventory _inventory;
	[SerializeField] private Nugget _nugget;

	private void Start()
	{
		_button.onClick.AddListener(() => ResetGame());
	}

	public void ResetGame()
	{
		_gearSlotsMaster.ResetGears();
		_inventory.ResetInventory();
		_inventory.UpdateInventory();
		_nugget.TriggerInitialState();

	}
}
```
### Esse é o resultado final:
![Gameplay9](https://user-images.githubusercontent.com/68963406/138774292-af35fe36-75d3-4ef3-877f-76ce5262edb6.gif)

