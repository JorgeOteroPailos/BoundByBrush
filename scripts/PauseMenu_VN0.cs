using Godot;
using System;

public partial class PauseMenu_VN0 : CanvasLayer
{
	
	private Button continuarButton;
	private Button salirButton;
	private Button optionsButton;
	private Button menuButton;
	
	private Button muchoSonidoButton;
	private Button sonidoParcialButton;
	private Button pocoSonidoButton;
	private Button mutedButton;
	private Button ajustesButton;
	private Button logrosButton;
	
	private bool flagOptions=false;
	
	private Vector2 position;
	private VBoxContainer contenedor;
	private VBoxContainer contenedor2;
	private VBoxContainer contenedor3;

	public override void _Ready(){
		
		contenedor=GetNode<VBoxContainer>("VBoxContainer");
		contenedor2=GetNode<VBoxContainer>("VBoxContainer2");
		contenedor3=GetNode<VBoxContainer>("VBoxContainer3");
		
		contenedor2.Visible=false;
		contenedor3.Visible=false;
		
		position=contenedor.Position;
		
		continuarButton = GetNode<Button>("VBoxContainer/Continuar");
		optionsButton = GetNode<Button>("VBoxContainer/Options");
		menuButton = GetNode<Button>("VBoxContainer/Menu");
		salirButton = GetNode<Button>("VBoxContainer/Salir");
		
		muchoSonidoButton = GetNode<Button>("VBoxContainer2/muchoSonido");
		sonidoParcialButton = GetNode<Button>("VBoxContainer2/sonidoParcial");
		pocoSonidoButton = GetNode<Button>("VBoxContainer2/pocoSonido");
		mutedButton = GetNode<Button>("VBoxContainer3/muted");
		ajustesButton = GetNode<Button>("VBoxContainer3/ajustes");
		logrosButton = GetNode<Button>("VBoxContainer3/logros");

		continuarButton.Pressed += OnContinuarPressed;
		optionsButton.Pressed += OnOptionsPressed;
		menuButton.Pressed += OnMenuPressed;
		salirButton.Pressed += OnSalirPressed;

		muchoSonidoButton.Pressed += OnMuchoVolumenPressed;
		sonidoParcialButton.Pressed += OnMedioVolumenPressed;
		pocoSonidoButton.Pressed += OnPocoVolumenPressed;
		mutedButton.Pressed += OnMutedPressed;
		ajustesButton.Pressed += OnAjustesPressed;
		logrosButton.Pressed += OnLogrosPressed;
		
		Visible = false; // Empieza oculto
		ProcessMode = ProcessModeEnum.Always; // Para que reciba input en pausa
		
		var optionButton = GetNode<OptionButton>("VBoxContainer2/pantalla");
		// Añadir el "texto inicial"
		optionButton.AddItem("Screen size selector");
		optionButton.SetItemDisabled(0, true); // Deshabilitar para que no sea seleccionable

		// Añadir opciones reales
		optionButton.AddItem("1280 x 720");
		optionButton.AddItem("1920 x 1080");
		optionButton.AddItem("Fullscreen");
		optionButton.ItemSelected += OnScreenOptionSelected;
		
	}

	public void ShowMenu()
	{
		Visible = true;
		GetTree().Paused = true;
	}

	public void HideMenu()
	{
		Visible = false;
		GetTree().Paused = false;
	}

	private void OnContinuarPressed(){
		HideMenu();
	}
	
	private void OnOptionsPressed(){
		if(flagOptions){
			contenedor.GlobalPosition=position;
			contenedor2.Visible=false;
			contenedor3.Visible=false;
			
			flagOptions=false;
		}else{
			var pos = contenedor.GlobalPosition;
			pos.X = position.X - 200;
			contenedor.GlobalPosition = pos;
			
			contenedor2.Visible=true;
			contenedor3.Visible=true;
			
			flagOptions=true;
		}
	}
	
	private void OnMenuPressed(){
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://escenas/inicio.tscn");
		return;
	}
	
	private void OnSalirPressed(){
		GetTree().Quit();
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
