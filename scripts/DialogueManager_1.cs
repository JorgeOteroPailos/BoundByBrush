using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class DialogueManager_1 : DialogueManagerBase {
	private List<DialogueLine> dialogueLines = new List<DialogueLine> {

		new DialogueLine("Iris", "Where am I?", false, true, null, "res://assets/VN/sadMC.png"),
		new DialogueLine(null, "The artist moved around the strange room. There were no windows, just 4 walls and a strange door.", false, false, null, "res://assets/VN/sadMC.png"),
		new DialogueLine(null, "Suddenly, the crackling sound of a small flame catched her attention.", false, false, null, "res://assets/VN/defaultMC.png"),
		new DialogueLine("Iris", "Who goes there?!", false, true, null, "res://assets/VN/angryMC.png"),
		new DialogueLine(null, "Before her eyes, from the fire below, appeared some kind of spirit, not bigger than a cat.", false, false, null, "res://assets/VN/defaultMC.png"),
		new DialogueLine("???", "Just a small flame, ma'am. A fellow condemned soul, just like you.", true, false, "res://assets/VN/surprised_spirit.png", "res://assets/VN/defaultMC.png"),
		new DialogueLine("Kenneth", "The name is Kenneth.", true, false, "res://assets/VN/default_spirit.png", "res://assets/VN/defaultMC.png"),
		new DialogueLine("Iris", "Well, Kenneth, you can call me Iris.", false, true, "res://assets/VN/default_spirit.png", "res://assets/VN/happyMC.png"),
		new DialogueLine("Iris", "Any idea how to get out of here?", false, true, "res://assets/VN/default_spirit.png", "res://assets/VN/motivatedMC.png"),
		new DialogueLine("Kenneth", "For now, it's better to focus on keeping you alive...", true, false, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/happyMC.png"),
		new DialogueLine("Kenneth", "Let me think...", true, false, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/happyMC.png"),
		new DialogueLine("Kenneth", "...", true, false, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/happyMC.png"),
		new DialogueLine("Kenneth", "...", true, false, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/happyMC.png"),
		new DialogueLine("Kenneth", "...", true, false, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/happyMC.png"),
		new DialogueLine("Kenneth", "...", true, false, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/sadMC.png"),
		new DialogueLine("Kenneth", "Oh, I know! Maybe we can do something with that art supplies of yours.", true, false, "res://assets/VN/surprised_spirit.png", "res://assets/VN/defaultMC.png"),
		new DialogueLine("Kenneth", "Look around. There is blue, there is yellow... You could use those pigments on your benefit, you are an artist!", true, false, "res://assets/VN/surprised_spirit.png", "res://assets/VN/motivatedMC.png"),
		new DialogueLine("Iris", "But... how?", false, true, "res://assets/VN/default_spirit.png", "res://assets/VN/defaultMC.png"),
		new DialogueLine("Kenneth", "Just shoot your paint to those demons. If you hit one with the correct color, it will disappear!", true, false, "res://assets/VN/surprised_spirit.png", "res://assets/VN/happyMC.png"),
		new DialogueLine("Kenneth", "Although, you shall be careful. If they touch you, you won’t come out unscathed.", true, false, "res://assets/VN/default_spirit.png", "res://assets/VN/defaultMC.png"),
		new DialogueLine("Kenneth", "If you get rid of them, we'll be able to get to the door.", true, false, "res://assets/VN/default_spirit.png", "res://assets/VN/defaultMC.png"),
		new DialogueLine("Kenneth", "Good luck, Iris!", true, false, "res://assets/VN/surprised_spirit.png", "res://assets/VN/defaultMC.png"),
		new DialogueLine("Iris", "Bring it on!", false, true, null, "res://assets/VN/angryMC.png"),
	};


	protected override async void ShowNextDialogue() {
		if (currentIndex >= dialogueLines.Count) {
			GD.Print("Fin del diálogo 1.");
			GetTree().ChangeSceneToFile("res://escenas/mundo.tscn");
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
}
