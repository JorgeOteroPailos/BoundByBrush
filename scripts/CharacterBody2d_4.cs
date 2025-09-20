using Godot;
using System;

public partial class CharacterBody2d_4 : CharacterBody2D
{
	[Export] public float Speed = 100f;

	private AnimatedSprite2D _sprite;
	private AnimatedSprite2D llave;
	private Vector2 _direction = Vector2.Down;
	
	private bool puerta=false;
	
	private Timer _timer;

	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		llave = GetParent().GetNode<RigidBody2D>("RigidBody2D").GetNode<AnimatedSprite2D>("llave");
		
		llave.Play("default");
		_sprite.Play("bajar"); // Empieza moviéndose hacia abajo	
	}

	public override void _PhysicsProcess(double delta)
	{
		Velocity = _direction * Speed;

		// move_and_collide devuelve información si hubo colisión
		var collision = MoveAndCollide(Velocity * (float)delta);

		if (collision != null&&!puerta){
			GetParent().GetNode<RigidBody2D>("RigidBody2D").Visible=false;
			// Invertimos dirección
			_direction = -_direction;
			puerta=true;

			// Cambiamos animación según dirección
			if (_direction == Vector2.Up)
				_sprite.Play("subir");
			else
				_sprite.Play("bajar");
		}else if(collision!=null&&puerta){
			Velocity=_direction*0;
			GetParent().GetNode<Sprite2D>("Candado").Visible=false;
			_sprite.Play("idle");
			
			_timer = new Timer();
			_timer.WaitTime = 2.0f;
			_timer.OneShot = true;
			_timer.Timeout += abrirPuerta;
			AddChild(_timer);
			
			_timer.Start();
		}
	}
	
	private void abrirPuerta(){
		GetParent().GetNode<StaticBody2D>("StaticBody2D").GetNode<Sprite2D>("Puerta").Texture=GD.Load<Texture2D>("res://assets/Puerta_abierta.png");
		GetTree().ChangeSceneToFile("res://escenas/visual_novel_5.tscn");
	}
}
