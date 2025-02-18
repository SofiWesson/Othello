extends Node

var isCursorInSlot: bool

var selectedVisualiser

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	isCursorInSlot = false
	selectedVisualiser = get_node("SelectedVisualiser")
	selectedVisualiser.visible = false

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

func _on_static_body_3d_mouse_exited() -> void:
	isCursorInSlot = true
	selectedVisualiser.visible = true

func _on_static_body_3d_mouse_entered() -> void:
	isCursorInSlot = false
	selectedVisualiser.visible = false