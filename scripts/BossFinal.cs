using Godot;
using System;

public partial class BossFinal : CharacterBody2D
{
	private bool isDying = false;

	private String colorEscrito=""; 
	
	private const float RETROCESO_BALA=10;
	private const float RETROCESO_CHOQUE=100;
	public float VELOCIDAD=50;

	private Vector2 originalPos;
	[Export] public int color=4;
	
	public Player player;

	AnimatedSprite2D sprite;

	public override void _Ready()
	{

		// Obtener el nodo Sprite2D
		sprite = GetNode<AnimatedSprite2D>("Circulo");

		switch(color){
			case 0:
				colorEscrito="red";
				break;
			case 1:
				colorEscrito="orange";
				break;
			case 2:
				colorEscrito="yellow";
				break;
			case 3:
				colorEscrito="green";
				break;
			case 4:
				colorEscrito="blue";
				break;
			case 5:
				colorEscrito="purple";
				break;
			default:
				colorEscrito="black";
				break;
		}

		originalPos = GlobalPosition;
		
		var area = GetNode<Area2D>("DamageArea");
		area.BodyEntered += OnBodyEntered;
	}

	public void OnHitByBullet(Node bullet, Player player){
		if (bullet is Bullet bullet1)
		{
			if (bullet1.color != this.color){
				Vector2 retroceso = bullet1.Velocity.Normalized() * RETROCESO_BALA;
				Tween tween = GetTree().CreateTween();
				tween.TweenProperty(this, "global_position", GlobalPosition + retroceso, 0.1f);
			}
			else{
				isDying = true; // Activamos estado de muerte
				sprite.Play("demonio_" + colorEscrito + "_death");
				//sprite.AnimationFinished += () => QueueFree();
				sprite.AnimationFinished += () => {
				
					Node parentNode = GetParent().GetParent();

					if (parentNode is Mundo mundo)
					{
						mundo.EnemigoDerrotado();
					}
					else if (parentNode is Mundo2 mundo2)
					{
						mundo2.EnemigoDerrotado();
					}
					else if(parentNode is Mundo3 mundo3){
						mundo3.EnemigoDerrotado();
					}
					else if(parentNode is Mundo4 mundo4){
						mundo4.EnemigoDerrotado();
					} // Avisar al mundo
					QueueFree(); // Eliminar el enemigo
				};

				switch(player.nColores){
				case 2:
					if(player.color==2) player.color=4;
					else if(player.color==4) player.color=2;
					break;
				case 3:
					if(player.color==0) player.color=2;
					else if(player.color==2) player.color=4;
					else if(player.color==4) player.color=0;
					break;
				case 4:
					if(player.color==0) player.color=2;
					else if(player.color>1&&player.color<4) player.color++;
					else if(player.color==4) player.color=0; 
					break;
				case 6:
					if(player.color<5) player.color++;
					else player.color=0;
					break;
				default:
					break;
			}
			}
		}
	}
	
	private void OnBodyEntered(Node body){

			if (body is Player jugador){
				GD.Print("Jugador tocado por enemigo");
				jugador.takeDamage();
					
				Vector2 retroceso = (GlobalPosition - jugador.GlobalPosition).Normalized() * RETROCESO_CHOQUE;

				Tween tween = GetTree().CreateTween();
				tween.TweenProperty(this, "global_position", GlobalPosition + retroceso, 0.1f);
			}
		
	}
	
	public override void _PhysicsProcess(double delta){
	// Si está muriendo, no hacer nada más
	if (isDying)
		return;

	switch(color){
			case 0:
				colorEscrito="red";
				break;
			case 1:
				colorEscrito="orange";
				break;
			case 2:
				colorEscrito="yellow";
				break;
			case 3:
				colorEscrito="green";
				break;
			case 4:
				colorEscrito="blue";
				break;
			default:
				colorEscrito="purple";
				break;
		}

	// Vector dirección hacia el jugador
	Vector2 direction = (player.Position - Position).Normalized();

	// Mover hacia el jugador
	Position += direction * VELOCIDAD * (float)delta;

	// Determinar animación en base a la dirección
	string anim = "";
	bool flipH = false;

	if (direction.X < 0 && direction.Y < 0){
		anim = "demonio_" + colorEscrito + "_arriba_diagonal";
		flipH = false;
	}else if (direction.X > 0 && direction.Y < 0){
		anim = "demonio_" + colorEscrito + "_arriba_diagonal";
		flipH = true;
	}else if (direction.X < 0 && direction.Y > 0){
		anim = "demonio_" + colorEscrito + "_abajo_diagonal";
		flipH = false;
	}else if (direction.X > 0 && direction.Y > 0){
		anim = "demonio_" + colorEscrito + "_abajo_diagonal";
		flipH = true;
	}else if (direction.Y < 0){
		anim = "demonio_" + colorEscrito + "_arriba";
	}else if (direction.Y > 0){
		anim = "demonio_" + colorEscrito + "_abajo";
	}else if (direction.X < 0){
		anim = "demonio_" + colorEscrito + "_lado";
		flipH = false;
	}else if (direction.X > 0){
		anim = "demonio_" + colorEscrito + "_lado";
		flipH = true;
	}

	if (!string.IsNullOrEmpty(anim)){
		sprite.FlipH = flipH;
		if (sprite.Animation != anim)
			sprite.Play(anim);
	}
}

	
	
	
}
