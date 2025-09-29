using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class DialogueManager_6 : DialogueManagerBase {

	private List<DialogueLine> dialogueLines = new List<DialogueLine> {
		new DialogueLine("Kenneth", "I knew you could do it, Iris.", true, false, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/angryMC.png"),
		new DialogueLine("Iris", "I won’t forget your aid, little flame.", false, true, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/sadMC.png"),
		new DialogueLine("Iris", "To me, you are much more than a sinner.", false, true, "res://assets/VN/default_spirit.png", "res://assets/VN/happyMC.png"),
		new DialogueLine("Iris", "You were a friend, and for that, my gratitude is yours.", false, true, "res://assets/VN/default_spirit.png", "res://assets/VN/happyMC.png"),
		new DialogueLine("Kenneth", "I am the one who should be grateful, artist.", true, false, "res://assets/VN/default_spirit.png", "res://assets/VN/happyMC.png"),
		new DialogueLine("Kenneth", "You reminded me of something important…", true, false, "res://assets/VN/default_spirit.png", "res://assets/VN/happyMC.png"),
		new DialogueLine(null, "For a moment, the flame lifted into the air, bathed in a soft white glow. It was transcending, and in that light, it found peace.", false, false, "res://assets/VN/transcended_spirit.png", "res://assets/VN/defaultMC.png"),
		new DialogueLine(null, "The light started vanishing, together with the artist's loyal friend. Hell was no place for a redeemed soul like his.", false, false, "res://assets/VN/transcended_spirit.png", "res://assets/VN/defaultMC.png"),
		new DialogueLine("Iris", "Goodbye, friend. It’s time for me to get out.", false, true, null, "res://assets/VN/happyMC.png"),
		
		new DialogueLine(null, "The artist stepped back into the world not as she once was, but as something more: scarred, tempered, and unbroken.", false, false, null, "res://assets/VN/defaultMC.png"),
		new DialogueLine(null, "Still, hope stayed by her side, carried in the memory of a gentle flame.", false, false, null, "res://assets/VN/happyMC.png"),
		new DialogueLine(null, "Iris’ next painting became the most revered of her life. A piece that captured the spirit of friendship, joy, resilience, and redemption.", false, false, null, "res://assets/VN/happyMC.png"),
		new DialogueLine(null, "‘The portrait of a friend’.", true, true, "res://assets/VN/transcended_spirit.png", "res://assets/VN/happyMC.png"),
	};
	
	protected override async void ShowNextDialogue() {
		if (currentIndex >= dialogueLines.Count) {
			GD.Print("Fin del diálogo 0.");
			GetTree().ChangeSceneToFile("res://escenas/credits.tscn");
			return;
		}

		var line = dialogueLines[currentIndex];

		if (currentIndex == 9) {
			var newBackground = ResourceLoader.Load<Texture2D>("res://assets/forestbg.png");
			if (newBackground == null) {
				GD.PrintErr("Error: no se cargó la textura de fondo");
			} else {
				TheBackground.Texture = newBackground;
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
