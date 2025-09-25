using Godot;
using System;

public partial class Circulo : CharacterBody2D
{
	private bool isDying = false;
	
	[Export] public bool esJefe = false;

	public String colorEscrito=""; 
	
	private const float RETROCESO_BALA=10;
	private const float RETROCESO_CHOQUE=100;
	public float VELOCIDAD=50;

	private Vector2 originalPos;
	[Export] public int color=4;
	[Export] private CollisionShape2D collisionShape2D;
	
	public Player player;

	AnimatedSprite2D sprite;

	public override void _Ready()
	{

		// Obtener el nodo Sprite2D
		sprite = GetNode<AnimatedSprite2D>("Circulo");

		originalPos = GlobalPosition;
		
		var area = GetNode<Area2D>("DamageArea");
		area.BodyEntered += OnBodyEntered;
		
		if (esJefe){
			volverJefe();
		}
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
					
					 //Detener colisiones inmediatamente 
					collisionShape2D.SetDeferred("disabled", true);
					
					MundoBase parentNode = (MundoBase)GetParent().GetParent();
					
					parentNode.EnemigoDerrotado();
					
					QueueFree(); // Eliminar el enemigo
				};
				
				

			}
		}
	}
	
	private void OnBodyEntered(Node body){

			if (body is Player jugador){
				GD.Print("Jugador tocado por enemigo "+colorEscrito);
				if(isDying==true){
					return;
				}
				
				jugador.takeDamage();
				Vector2 retroceso = (GlobalPosition - jugador.GlobalPosition).Normalized() * RETROCESO_CHOQUE;
				var tween = CreateTween();
				tween.TweenProperty(this, "global_position", GlobalPosition + retroceso, 0.1f);
			}
		
	}
	
	public override void _PhysicsProcess(double delta){
		// Si está muriendo, no hacer nada más
		if (isDying)
			return;

		// Vector dirección hacia el jugador
		Vector2 direction = (player.Position - Position).Normalized();
		
		Vector2 separation = Vector2.Zero;
		float minDistance = 40f; // Distancia mínima entre enemigos

		foreach (Node other in GetParent().GetChildren()){
			if (other is Circulo otro && otro != this && !otro.isDying){
				float dist = GlobalPosition.DistanceTo(otro.GlobalPosition);
				if (dist < minDistance && dist > 0){
					// Empujar en dirección contraria
					separation += (GlobalPosition - otro.GlobalPosition).Normalized() * (minDistance - dist);
				}
			}
		}

		Vector2 finalDir = (direction + separation.Normalized() * 0.5f).Normalized();

		Position += finalDir * VELOCIDAD * (float)delta;

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
	
	public void volverJefe(){
		this.VELOCIDAD/=1.5f;

		
		GD.Print("el jefe ahora es negro y color"+this.color);
		
		// Triplicar el tamaño del sprite
		this.Scale *= new Vector2(3, 3);
		
		// Ajustar el tamaño de la hitbox también
		if (collisionShape2D.Shape is CircleShape2D circle){
			//circle.Radius *= 3;
		}else if (collisionShape2D.Shape is RectangleShape2D rect){
			//rect.Size *= new Vector2(1, 1);
		}
	}
}
