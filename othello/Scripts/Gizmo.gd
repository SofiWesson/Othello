extends Node

var mesh
var colour


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	mesh = get_node("MeshInstance3D")


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta: float) -> void:
	pass
