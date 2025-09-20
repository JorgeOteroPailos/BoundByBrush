using Godot;
using System;
using System.Collections.Generic;

public abstract partial class MundoBase : Node2D
{
	protected Player player;
	
	protected int nEnemigo=0;
	
	protected Random random = new Random();
	
	protected List<CharacterBody2D> enemigos = new List<CharacterBody2D>();
	
	protected virtual int nEnemigos { get; set; }
	
	protected virtual String siguienteEscena { get; }
	
	public void EnemigoDerrotado() {
		GD.Print("Enemigo derrotado.");
		nEnemigos--;
		GD.Print($"Quedan {nEnemigos} enemigos.");
		if (nEnemigos <= 0)
		{
			GD.Print("Todos los enemigos derrotados. Cambio de escena...");
			GetTree().ChangeSceneToFile(siguienteEscena);
		}
	}
	
	protected void spawnearEnemigo(){
		// Instanciar una escena "Circulo.tscn"
		Circulo miCirculo = (Circulo)enemigos[nEnemigo];
		
		miCirculo.player=player;

	}
}
