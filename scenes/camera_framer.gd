extends Camera3D

const ANCHOR_POINT := Vector3(0, 0, 0)  # point you want near the bottom in world space
const TARGET_MARGIN_PX := 40            # how many pixels above bottom edge

func _process(_delta):
	var viewport_size = get_viewport().size
	var anchor_screen = unproject_position(ANCHOR_POINT)

	# If anchor is not valid (behind camera), bail
	if anchor_screen.z <= 0.0:
		return

	var current_y_px = anchor_screen.y
	var target_y_px = viewport_size.y - TARGET_MARGIN_PX
	var diff_px = target_y_px - current_y_px

	# Convert a small pixel error into a small world-space vertical nudge.
	# scale_factor is arbitrary; tune until it feels stable.
	var scale_factor := 0.02
	global_position.y += diff_px * scale_factor


3440 x 1440