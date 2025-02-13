extends Node

var gizmo = preload("res://Prefabs/Gizmo.tscn")
var chipPrefab = preload("res://Prefabs/Chip.tscn")

var matrixSlots: Array[Array] = []

enum SlotState {
	EMPTY,
	WHITE,
	BLACK
}

class Slot:
	func _init():
		slotState = SlotState.EMPTY
		position = Vector3.ZERO

	var slotState: SlotState
	var position: Vector3
	var chip: Node3D

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	MakeMatrix()
	MakeGrid()
	SpawnChips()
	
func MakeMatrix():
	#add columns to matrix (y)
	for i in range(8):
		matrixSlots.push_back([])
	#add rows/slots to matrix (x)
	for row in matrixSlots:
		for i in range(8):
			row.push_back(Slot.new())

func MakeGrid():
	#populate the grid slots
	var y = -3.5
	for row in matrixSlots: # y (actually z for some)
		var x = -3.5
		for space in row: # x
			space.slotState = SlotState.EMPTY
			space.position = Vector3(x, 0, y)
			x += 1
		y += 1

func SpawnChips():
	#spawn chips in place
	for row in matrixSlots:
		for space in row:
			var newChip = chipPrefab.instantiate()
			newChip.position = space.position
			newChip.visible = false
			space.chip = newChip
			add_child(space.chip)

func PlaceChip():
	pass

func FlipChip():
	pass

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta: float) -> void:
	pass
