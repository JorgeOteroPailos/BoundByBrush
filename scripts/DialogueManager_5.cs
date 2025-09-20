using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class DialogueManager_5 : Node {
	[Export] public Label DialogueLabel;
	[Export] public Label NameLabel;
	[Export] public Sprite2D CharacterLeft;
	[Export] public Sprite2D CharacterRight;
	[Export] public TextureRect TheBackground;

	private int currentIndex = 0;
	private bool isTyping = false;
	private string currentText = "";

	private List<DialogueLine> dialogueLines = new List<DialogueLine> {
		new DialogueLine(null, "A thunderous roar erupts through the chamber as she arrives: It’s the most horrific sound the artist’s mortal ears have ever heard. The ever-friendly flame hesitates, trailing behind. Every step he takes feels heavier than the last.", false, false, null, null),
		new DialogueLine("Iris", "This can’t be…", false, true, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/sadMC.png"),
		new DialogueLine("Kenneth", "Iris…", true, false, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/sadMC.png"),
		new DialogueLine(null, "The flame flickers. Remaining upright demands every ounce of his strength.", false, false, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/sadMC.png"),
		new DialogueLine("Iris", "Kenneth!", false, true, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/sadMC.png"),
		new DialogueLine("Iris", "What is it? Tell me, please!", false, true, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/sadMC.png"),
		new DialogueLine("Kenneth", "We came so far… Too far. It’s draining me. I can’t go on…", true, false, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/sadMC.png"),
		new DialogueLine("Iris", "But, I don’t want to leave you here… I can wait for you. We’ll go slowly.", false, true, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/defaultMC.png"),
		new DialogueLine("Kenneth", "It’s no use, Iris. I was never meant to leave this place.", true, false, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/sadMC.png"),
		new DialogueLine("Kenneth", "You are a human. Still full of life and full of dreams.", true, false, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/happyMC.png"),
		new DialogueLine("Kenneth", "I am naught but the wretched remains of a sinner, cursed to linger beyond redemption.", true, false, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/sadMC.png"),
		new DialogueLine("Iris", "Kenneth...", false, true, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/sadMC.png"),
		new DialogueLine("Kenneth", "Now go, artist. Go, go, GO!", true, false, "res://assets/VN/surprised_spirit.png", "res://assets/VN/angryMC.png"),
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
			GetTree().ChangeSceneToFile("res://escenas/mundo_5.tscn");
			return;
		}

		var line = dialogueLines[currentIndex];

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
