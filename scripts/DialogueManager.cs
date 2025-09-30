using Godot;
using System;
using System.Collections.Generic;

public partial class DialogueManager : Node
{
	[Export] public Label DialogueLabel;
	[Export] public Label NameLabel;
	[Export] public Sprite2D CharacterLeft;
	[Export] public Sprite2D CharacterRight;
	[Export] public TextureRect TheBackground;
	
	private static int indiceLinea=0;
	private static int[] lineasPorEscena={11, 23, 6, 7, 6, 13, 13};
	//linea en la que cambiamos de fondo en medio del diálogo. Si no lo hacemos, -1
	private static int[] cambioDeFondo={11, -1, 11, 11, 11, -1, 9};
	private static string[] fondoACambiar={"Fondo_Noche_Estrellada.png", null, "Fondo_Noche_Estrellada.png", "Fondo_Noche_Estrellada.png", "Fondo_Noche_Estrellada.png", null, "forestbg.png"};
	
	private static string[] siguienteEscena={"despertar", "mundo", "mundo_2", "mundo_3", "mundo_4", "mundo_5", "credits"};
	
	protected bool isTyping = false;
	protected string currentText = "";
	
	public override void _Input(InputEvent @event){
		if ((@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed) ||
			(@event is InputEventKey keyEvent && keyEvent.Pressed && keyEvent.Keycode == Key.Space))
		{
			if (isTyping)
				SkipTyping();
			else
				ShowNextDialogue();
		}
	}

	public override void _Ready() {
		ShowNextDialogue();
	}
	
	private void SkipTyping() {
		DialogueLabel.Text = currentText;
		isTyping = false;
	}
	
	// Método que cada subclase implementará
	private async void ShowNextDialogue() {
		if (indiceLinea >= lineasPorEscena[Estado.indiceNovela]) {
			GD.Print("Fin del diálogo "+Estado.indiceNovela);
			GetTree().ChangeSceneToFile("res://escenas/"+siguienteEscena[Estado.indiceNovela]+".tscn");
			dialogueLines.RemoveRange(0, lineasPorEscena[Estado.indiceNovela]);
			Estado.indiceNovela++;
			return;
		}
		
		// Cambio de fondo en línea q toque
		if (indiceLinea == cambioDeFondo[Estado.indiceNovela]) {
			var newBackground = ResourceLoader.Load<Texture2D>("res://assets/fondos/"+fondoACambiar[Estado.indiceNovela]);
			if (newBackground == null) {
				GD.PrintErr("Error: no se cargó la textura de fondo");
			} else {
				TheBackground.Texture = newBackground;
				GD.Print("Fondo cambiado correctamente en línea "+indiceLinea+ " a "+fondoACambiar[Estado.indiceNovela]);
			}
		}
		
		var linea = dialogueLines[indiceLinea];
		
		if(NameLabel==null){
			GD.Print("nameLabel es null");
		}
		
		NameLabel.Text = linea.Speaker;
		DialogueLabel.Text = "";
		currentText = linea.Text;
		
		if (!string.IsNullOrEmpty(linea.LeftSpritePath)) {
			CharacterLeft.Texture = GD.Load<Texture2D>(linea.LeftSpritePath);
			CharacterLeft.Visible = true;
		} else {
			CharacterLeft.Visible = false;
		}

		if (!string.IsNullOrEmpty(linea.RightSpritePath)) {
			CharacterRight.Texture = GD.Load<Texture2D>(linea.RightSpritePath);
			CharacterRight.Visible = true;
		} else {
			CharacterRight.Visible = false;
		}

		CharacterLeft.Modulate = linea.LeftActive ? Colors.White : new Color(1, 1, 1, 0.75f);
		CharacterRight.Modulate = linea.RightActive ? Colors.White : new Color(1, 1, 1, 0.75f);

		isTyping = true;
		foreach (char c in currentText) {
			DialogueLabel.Text += c;
			await ToSignal(GetTree().CreateTimer(0.03f), "timeout");
			if (!isTyping) break;
		}

		DialogueLabel.Text = currentText;
		isTyping = false;
		indiceLinea++;
	}
	
	protected class DialogueLine {
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
		
		new DialogueLine("Kenneth", "Iris, you did it!", true, false, "res://assets/VN/default_spirit.png", "res://assets/VN/angryMC.png"),
		new DialogueLine("Kenneth", "That was amazing, you were like 'pow pow' and then 'eat that blue paint, you demon'!", true, false, "res://assets/VN/surprised_spirit.png", "res://assets/VN/defaultMC.png"),
		new DialogueLine("Iris", "Hahaha, I guess I did.", false, true, "res://assets/VN/default_spirit.png", "res://assets/VN/happyMC.png"),
		new DialogueLine("Kenneth", "Look, this room... It looks like it has your previous two colors, plus red.", true, false, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/defaultMC.png"),
		new DialogueLine("Kenneth", "I like red pretty much. Red is the color of passion!", true, false, "res://assets/VN/surprised_spirit.png", "res://assets/VN/defaultMC.png"),
		new DialogueLine("Iris", "And blood.", false, true, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/angryMC.png"),
	
		new DialogueLine("Kenneth", "Good job, Iris.", true, false, "res://assets/VN/default_spirit.png", "res://assets/VN/defaultMC.png"),
		new DialogueLine("Kenneth", "If you keep it up, no one could stop you. Not even the Devil himself!", true, false, "res://assets/VN/surprised_spirit.png", "res://assets/VN/happyMC.png"),
		new DialogueLine("Iris", "You really are the greatest optimist, Kenneth.", false, true, "res://assets/VN/default_spirit.png", "res://assets/VN/happyMC.png"),
		new DialogueLine("Kenneth", "What can I say? I like to see the good side of everything.", true, false, "res://assets/VN/default_spirit.png", "res://assets/VN/happyMC.png"),
		new DialogueLine("Iris", "Talking about good things, this room looks like it has some green.", false, true, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/defaultMC.png"),
		new DialogueLine("Kenneth", "Some green demons too…", true, false, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/sadMC.png"),
		new DialogueLine("Iris", "Let’s get rid of them, Kenneth!", false, true, "res://assets/VN/surprised_spirit.png", "res://assets/VN/motivatedMC.png"),
		
		new DialogueLine("Iris", "That door… It had two more colors.", false, true, "res://assets/VN/default_spirit.png", "res://assets/VN/defaultMC.png"),
		new DialogueLine("Kenneth", "It seems like your color-wheel is getting bigger!", true, false, "res://assets/VN/surprised_spirit.png", "res://assets/VN/defaultMC.png"),
		new DialogueLine("Kenneth", "We are close to the exit, Iris. I can feel it.", true, false, "res://assets/VN/default_spirit.png", "res://assets/VN/defaultMC.png"),
		new DialogueLine("Iris", "Each time it gets more complicated.", false, true, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/sadMC.png"),
		new DialogueLine("Kenneth", "I’m a bit scared, artist. There are so many demons here…", true, false, "res://assets/VN/disappointed_spirit.png", "res://assets/VN/sadMC.png"),
		new DialogueLine("Iris", "Then we’ll give them a show.", false, true, "res://assets/VN/surprised_spirit.png", "res://assets/VN/motivatedMC.png"),
	
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
}
