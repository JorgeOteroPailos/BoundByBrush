using Godot;
using System;

public partial class Inicio : Node
{
	private CanvasLayer _pauseMenu;
	
	private Button paintButton;
	private Button salirButton;
	private Button optionsButton;
	
	private Button muchoSonidoButton;
	private Button sonidoParcialButton;
	private Button pocoSonidoButton;
	private Button mutedButton;
	private Button ajustesButton;
	private Button logrosButton;
	
	private VBoxContainer contenedor;
	private VBoxContainer contenedor2;
	private VBoxContainer contenedor3;
	
	private bool seve=false;

	public override void _Ready()
	{
		// Obtener referencia al menú de pausa
		_pauseMenu = GetNode<CanvasLayer>("optionsMenu");
		_pauseMenu.Visible = false;
		
		paintButton = GetNode<Button>("paint");
		salirButton = GetNode<Button>("exit");
		optionsButton = GetNode<Button>("opts");
		
		muchoSonidoButton = GetNode<Button>("optionsMenu/VBoxContainer2/muchoSonido");
		sonidoParcialButton = GetNode<Button>("optionsMenu/VBoxContainer2/sonidoParcial");
		pocoSonidoButton = GetNode<Button>("optionsMenu/VBoxContainer2/pocoSonido");
		mutedButton = GetNode<Button>("optionsMenu/VBoxContainer3/muted");
		ajustesButton = GetNode<Button>("optionsMenu/VBoxContainer3/ajustes");
		logrosButton = GetNode<Button>("optionsMenu/VBoxContainer3/logros");

		muchoSonidoButton.Pressed += OnMuchoVolumenPressed;
		sonidoParcialButton.Pressed += OnMedioVolumenPressed;
		pocoSonidoButton.Pressed += OnPocoVolumenPressed;
		mutedButton.Pressed += OnMutedPressed;
		ajustesButton.Pressed += OnAjustesPressed;
		logrosButton.Pressed += OnLogrosPressed;

		optionsButton.Pressed += OnBotonPausaPressed; // <- ButtonDown es inmediato al click
		paintButton.Pressed += OnBotonPaintPressed; // <- ButtonDown es inmediato al click
		salirButton.Pressed += OnBotonExitPressed; // <- ButtonDown es inmediato al click
		
		var optionButton = GetNode<CanvasLayer>("optionsMenu").GetNode<VBoxContainer>("VBoxContainer2").GetNode<OptionButton>("pantalla");
		// Añadir el "texto inicial"
		optionButton.AddItem("Screen size selector");
		optionButton.SetItemDisabled(0, true); // Deshabilitar para que no sea seleccionable

		// Añadir opciones reales
		optionButton.AddItem("1280 x 720");
		optionButton.AddItem("1920 x 1080");
		optionButton.AddItem("Fullscreen");
		optionButton.ItemSelected += OnScreenOptionSelected;

	}

	private void OnBotonPausaPressed()
	{
		GD.Print("Botón pulsado, mostrando menú de opciones");
		if(seve){
			_pauseMenu.Visible = false;
			seve=false;	
		} 
		else{
			_pauseMenu.Visible = true;
			seve=true;
		}
	}
	
	private void OnBotonExitPressed()
	{
		GetTree().Quit();
	}
	
	private void OnBotonPaintPressed()
	{
		// Cambiar escena a novelaVisual0
		Estado.nivel=1;
		Estado.indiceNovela=0;
		GetTree().ChangeSceneToFile("res://escenas/visual_novel.tscn");
			return;
		
	}
	private void OnMuchoVolumenPressed(){
		
	}
	private void OnMedioVolumenPressed(){
		
	}
	private void OnPocoVolumenPressed(){
		
	}
	private void OnMutedPressed(){
		
	}
	private void OnAjustesPressed(){
		
	}
	private void OnLogrosPressed(){
		
	}
	
	private void OnScreenOptionSelected(long index){
	switch (index)
	{
		case 1: // 1280x720
			DisplayServer.WindowSetSize(new Vector2I(1280, 720));
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
			break;
		case 2: // 1920x1080
			DisplayServer.WindowSetSize(new Vector2I(1920, 1080));
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
			break;
		case 3: // Pantalla completa
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
			break;
	}
	}
}
