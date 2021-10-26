# Devlog 6 - Nugget States


O Nugget precisa reagir aos encaixe das engrenagens. Ele só pode dizer que a task está concluída quando todas engrenagens estarem girando, e entrar um estado "feliz". Então para isso, o Nugget se inscreve aos eventos da classe `GearSlotsMaster` e muda os estados conforme os eventos são chamados.

Apliquei um simples Finite State Machine com enums para controlar o estado do Nugget e algumas animações com tweens como bônus hehe:

```csharp
public class Nugget : MonoBehaviour
{
	[SerializeField] private GearSlotsMaster _gearSlotsMaster;
	[SerializeField] private TextMeshProUGUI _bubbleText;
	[SerializeField] private List<TweenController> _happyAnimationTweens;

	private Vector3 _initialLocalScale;
	private Vector3 _initialPosition;

	[Multiline(2)] [SerializeField] private string _initialText;
	[Multiline(2)] [SerializeField] private string _waitingText;
	[Multiline(2)] [SerializeField] private string _finishedText;

	public enum NuggetState
	{
		Initial,
		Waiting,
		Finished
	}

	private NuggetState _nuggetState = NuggetState.Initial;
	private void Start()
	{
		_initialLocalScale = transform.localScale;
		_initialPosition = transform.position;

		_bubbleText.SetText(_initialText);
		_gearSlotsMaster.AllInsertSequenceInitiated += TriggerWaitState;
		_gearSlotsMaster.AllInsertSequenceFinished += TriggerFinishedState;
	}

	public void TriggerInitialState() => SwitchState(NuggetState.Initial);
	public void TriggerWaitState() => SwitchState(NuggetState.Waiting);
	public void TriggerFinishedState() => SwitchState(NuggetState.Finished);

	private void SwitchState(NuggetState state)
	{
		_nuggetState = state;

		switch (_nuggetState)
		{
			case NuggetState.Initial:
				foreach (var tween in _happyAnimationTweens)
				{
					tween.KillTween(true);
				}
				transform.localScale = _initialLocalScale;
				transform.position = _initialPosition;

				_bubbleText.SetText(_initialText);
				break;

			case NuggetState.Waiting:
				_bubbleText.SetText(_waitingText);
				break;
			
			case NuggetState.Finished:
				_bubbleText.SetText(_finishedText);

				foreach (var tween in _happyAnimationTweens)
				{
					tween.CallTween();
				}
				break;
		}
	}
}
```
### Esse é o resultado até agora:
![Gameplay8](https://user-images.githubusercontent.com/68963406/138774134-a1515107-82f4-4b75-963e-e1d0ce5355a3.gif)

