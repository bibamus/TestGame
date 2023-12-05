extends Node2D

class_name Main

var player_scene = preload("res://scenes/player.tscn")
@onready var world :Node = $World


func _ready():
	pass




func _on_world_world_loaded():
	var player = player_scene.instantiate()
	add_child(player)
