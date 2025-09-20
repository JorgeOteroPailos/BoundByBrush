using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class DialogueManager_0 : Node {
	[Export] public Label DialogueLabel;
	[Export] public Label NameLabel;
	[Export] public Sprite2D CharacterLeft;
	[Export] public Sprite2D CharacterRight;
	[Export] public TextureRect TheBackground;

	private int currentIndex = 0;
	private bool isTyping = false;
	private string currentText = "";

	private List<DialogueLine> dialogueLines = new List<DialogueLine> {
		new DialogueLine(null, "They say the Devil bows to no one. He rules with pride and defiance, and he needs neither permission nor praise.", false, false, null, null),
		new DialogueLine(null, "For ages, nothing stirred him: no prayer, no curse, no soul worth a second glance.", false, false, null, null),
		new DialogueLine(null, "But eventually, his boredom and curiosity drew him to a mortal unlike any other. She came with no crown, no army, just a simple brush. A painter whose strokes flayed deception, leaving only the raw, bleeding truth beneath.", false, false, null, null),
		new DialogueLine(null, "Fascinated, the Devil demanded a portrait. And she delivered.", false, false, null, null),
		new DialogueLine(null, "What stared back at him from the canvas was no majestic demon lord, no master of sin. It was him, unmasked, monstrous, and small.", false, false, null, null),
		new DialogueLine("Devil", "What is the meaning of this?!", true, false, "res://assets/VN/angry_demon.png", null),
		new DialogueLine("Iris", "This is your true self. I am faithful to the strokes of my brush.", false, true, "res://assets/VN/angry_demon.png", "res://assets/VN/defaultMC.png"),
		new DialogueLine("Devil", "That... That hideous creature cannot be me!", true, false, "res://assets/VN/angry_demon.png", "res://assets/VN/defaultMC.png"),
		new DialogueLine("Iris", "My apologies. Yet, please understand that my brush only speaks truth.", false, true, "res://assets/VN/angry_demon.png", "res://assets/VN/sadMC.png"),
		new DialogueLine(null, "Ashamed and enraged, he banished the painter to the depths of her own personal Hell.", false, false, "res://assets/VN/angry_demon.png", "res://assets/VN/sadMC.png"),
		new DialogueLine("Devil", "Foolish kid. That will teach her.", true, false, "res://assets/VN/default_demon.png", null),
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
			GetTree().ChangeSceneToFile("res://escenas/despertar.tscn");
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
