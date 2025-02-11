extends Node

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

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	MakeMatrix()
	MakeGrid()
func MakeMatrix():
	#add columns to matrix (y)
	for i in range(8):
		matrixSlots.push_back([])
	#add rows/slots to matrix (x)
	for row in matrixSlots:
		for i in range(8):
			row.push_back(Slot.new())

func MakeGrid():
	var y: int = 0
	for row in matrixSlots: # y
		var x: int = 0
		for space in row: # x
			space.slotState = SlotState.EMPTY
			space.position = Vector3(x, y, 0)
			x += 1
		y += 1

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta: float) -> void:
	pass
