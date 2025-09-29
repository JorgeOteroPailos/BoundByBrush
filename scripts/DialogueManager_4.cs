using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class DialogueManager_4 : DialogueManagerBase {
	private List<DialogueLine> dialogueLines = new List<DialogueLine> {
		new DialogueLine("Iris", "That door… It had two more colors.", false, true, "res://assets/VN/default_spirit.png", "res://assets/VN/defaultMC.png"),
		new DialogueLine("Kenneth", "It seems like your color-wheel is getting bigger!", true, false, "res://assets/VN/surprised_spirit.png", "res://assets/VN/defaultMC.png"),
		new DialogueLine("Kenneth", "We are close to the exit, Iris. I can feel it.", true, false, "res://assets/VN/default_spirit.png", "res://assets/VN/defaultMC.png"),
		new DialogueLine("Iris", "Each time it gets more complicated.", false, true, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/sadMC.png"),
		new DialogueLine("Kenneth", "I’m a bit scared, artist. There are so many demons here…", true, false, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/sadMC.png"),
		new DialogueLine("Iris", "Then we’ll give them a show.", false, true, "res://assets/VN/surprised_spirit.png", "res://assets/VN/motivatedMC.png"),
	};

	protected override async void ShowNextDialogue() {
		if (currentIndex >= dialogueLines.Count) {
			GD.Print("Fin del diálogo 0.");
			GetTree().ChangeSceneToFile("res://escenas/mundo_4.tscn");
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
}
