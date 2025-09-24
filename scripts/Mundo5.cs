using Godot;
using System;

public partial class Mundo5 : MundoBase
{
	
	[Export]
	public PackedScene CirculoScene;
	
	[Export]
	public int nColores=6;
	
	protected override String siguienteEscena=>"res://escenas/visual_novel_6.tscn";
	
	// Called when the node enters the scene tree for the first time.
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		
		nEnemigos=25;
		
		hayJefe=true;
		
		player = GetNode<Player>("personaje");
		player.world="res://escenas/mundo_5.tscn";
		
		var personaje=GetNode<CharacterBody2D>("personaje");
		if(personaje is Player player1){
			player1.SetNColores(6);
		}
		
		Node Enemigos = GetNode("Enemigos");

		foreach (Node child in Enemigos.GetChildren()){
			if (child is CharacterBody2D enemigo){
				enemigos.Add(enemigo);

				if(enemigo is Circulo circulo){
					
					if(circulo.esJefe){
						spawnearEnemigo();
						nEnemigo++;
						circulo.volverJefe();
						continue;
					}
					
					circulo.VELOCIDAD*=3.3f;
					if(nEnemigo%6==1){
						circulo.color = 2;
					}else if(nEnemigo%6==2){
						circulo.color=0;
					}else if(nEnemigo%6==3){
						circulo.color=3;
					}else if(nEnemigo%6==4){
						circulo.color=1;
					}else if(nEnemigo%6==5){
						circulo.color=5;
					}
				}

				spawnearEnemigo();
				nEnemigo++;
			}
		}
		
	}
	
}
