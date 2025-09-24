using Godot;
using System;
using System.Collections.Generic;

public abstract partial class MundoBase : Node2D
{
	protected Player player;
	
	[Export] protected bool hayJefe=false;
	
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
		}else if (hayJefe){
			if(nEnemigos==1){
				player.cambiarColor(true);
			}
		}
		
	}
	
	protected void spawnearEnemigo(){
		// Instanciar una escena "Circulo.tscn"
		Circulo miCirculo = (Circulo)enemigos[nEnemigo];
		
		miCirculo.player=player;

	}
}
