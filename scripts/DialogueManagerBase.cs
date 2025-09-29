using Godot;
using System;

public abstract partial class DialogueManagerBase : Node{
	[Export] public Label DialogueLabel;
	[Export] public Label NameLabel;
	[Export] public Sprite2D CharacterLeft;
	[Export] public Sprite2D CharacterRight;
	[Export] public TextureRect TheBackground;

	protected int currentIndex = 0;
	protected bool isTyping = false;
	protected string currentText = "";
	
	public override void _Input(InputEvent @event)
	{
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
		protected abstract void ShowNextDialogue();
		
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
}
