using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class DialogueManager_2 : Node {
	[Export] public Label DialogueLabel;
	[Export] public Label NameLabel;
	[Export] public Sprite2D CharacterLeft;
	[Export] public Sprite2D CharacterRight;
	[Export] public TextureRect TheBackground;

	private int currentIndex = 0;
	private bool isTyping = false;
	private string currentText = "";

	private List<DialogueLine> dialogueLines = new List<DialogueLine> {
		new DialogueLine("Kenneth", "Iris, you did it!", true, false, "res://assets/VN/default_spirit.png", "res://assets/VN/angryMC.png"),
		new DialogueLine("Kenneth", "That was amazing, you were like 'pow pow' and then 'eat that blue paint, you demon'!", true, false, "res://assets/VN/surprised_spirit.png", "res://assets/VN/defaultMC.png"),
		new DialogueLine("Iris", "Hahaha, I guess I did.", false, true, "res://assets/VN/default_spirit.png", "res://assets/VN/happyMC.png"),
		new DialogueLine("Kenneth", "Look, this room... It looks like it has your previous two colors, plus red.", true, false, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/defaultMC.png"),
		new DialogueLine("Kenneth", "I like red pretty much. Red is the color of passion!", true, false, "res://assets/VN/surprised_spirit.png", "res://assets/VN/defaultMC.png"),
		new DialogueLine("Iris", "And blood.", false, true, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/angryMC.png"),
	};

	public override void _Ready() {
		ShowNextDialogue();
	}

	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed) {
			if (isTyping) {
				SkipTyping();
			} else {
				ShowNextDialogue();
			}
		}
	}

	private async void ShowNextDialogue() {
		if (currentIndex >= dialogueLines.Count) {
			GD.Print("Fin del diálogo 0.");
			GetTree().ChangeSceneToFile("res://escenas/mundo_2.tscn");
			return;
		}

		var line = dialogueLines[currentIndex];

		// Cambio de fondo en línea 11
		if (currentIndex == 11) {
			var newBackground = ResourceLoader.Load<Texture2D>("res://assets/Fondo_Noche_Estrellada.png");
			if (newBackground == null) {
				GD.PrintErr("Error: no se cargó la textura de fondo");
			} else {
				TheBackground.Texture = newBackground;
				GD.Print("Fondo cambiado correctamente en línea 11");
			}
		}

		NameLabel.Text = line.Speaker;
		DialogueLabel.Text = "";
		currentText = line.Text;

		if (!string.IsNullOrEmpty(line.LeftSpritePath)) {
			CharacterLeft.Texture = GD.Load<Texture2D>(line.LeftSpritePath);
			CharacterLeft.Visible = true;
		} else {
			CharacterLeft.Visible = false;
		}

		if (!string.IsNullOrEmpty(line.RightSpritePath)) {
			CharacterRight.Texture = GD.Load<Texture2D>(line.RightSpritePath);
			CharacterRight.Visible = true;
		} else {
			CharacterRight.Visible = false;
		}

		CharacterLeft.Modulate = line.LeftActive ? Colors.White : new Color(1, 1, 1, 0.75f);
		CharacterRight.Modulate = line.RightActive ? Colors.White : new Color(1, 1, 1, 0.75f);

		isTyping = true;
		foreach (char c in currentText) {
			DialogueLabel.Text += c;
			await ToSignal(GetTree().CreateTimer(0.03f), "timeout");
			if (!isTyping) break;
		}

		DialogueLabel.Text = currentText;
		isTyping = false;
		currentIndex++;
	}

	private void SkipTyping() {
		DialogueLabel.Text = currentText;
		isTyping = false;
	}

	private class DialogueLine {
		public string Speaker;
		public string Text;
		public bool LeftActive;
		public bool RightActive;
		public string LeftSpritePath;
		public string RightSpritePath;

		public DialogueLine(string speaker, string text, bool leftActive, bool rightActive,
							string leftSpritePath = null, string rightSpritePath = null) {
			Speaker = speaker;
			Text = text;
			LeftActive = leftActive;
			RightActive = rightActive;
			LeftSpritePath = leftSpritePath;
			RightSpritePath = rightSpritePath;
		}
	}
}
