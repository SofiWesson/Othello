extends Node

var gizmo = preload("res://Prefabs/Gizmo.tscn")
var chipPrefab = preload("res://Prefabs/Chip.tscn")
var selectionCollider = preload("res://Prefabs/SelectionCollider.tscn")

var p1ChipReserve: ChipReserve
var p2ChipReserve: ChipReserve

var matrixSlots: Array[Array] = []

var isPlayerOnesTurn: bool

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
	var selectionCollider: Node3D
	var chip: Node3D

class ChipReserve:
	var chipReserve: Node3D
	var chips: Array
	var chipSpacing = 0.25

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	isPlayerOnesTurn = true

	MakeMatrix()
	MakeGrid()
	MakeChipReserves()
	
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
	for row in matrixSlots: # y (actually z for some reason)
		var x = -3.5
		for space in row: # x
			space.slotState = SlotState.EMPTY
			space.position = Vector3(x, 0, y)
			space.selectionCollider = selectionCollider.instantiate()
			space.selectionCollider.position = space.position
			x += 1
		y += 1

func MakeChipReserves():
	#set up the chip reserves for player 1
	p1ChipReserve = ChipReserve.new()
	p1ChipReserve.chipReserve = get_node("P1ChipReserve")
	SpawnChips(p1ChipReserve)
	#set up the chip reserves for player 2
	p2ChipReserve = ChipReserve.new()
	p2ChipReserve.chipReserve = get_node("P2ChipReserve")
	SpawnChips(p2ChipReserve)

func SpawnChips(chipReserve: ChipReserve):
	#spawn 32 chips in each reserve
	for i in range(32):
		var newChip = chipPrefab.instantiate()
		chipReserve.chipReserve.add_child(newChip)
		newChip.position = Vector3(0.15 + (i * chipReserve.chipSpacing), 0, 0)
		var rad = deg_to_rad(90)
		newChip.rotate_z(rad)
		chipReserve.chips.push_back(newChip)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta: float) -> void:
	pass

func PlaceChip():
	pass

func FlipChip():
	pass