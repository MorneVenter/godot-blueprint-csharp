extends SceneTree

func _init() -> void:
	var path := OS.get_cmdline_user_args()[0]
	var id := ResourceUID.create_id()
	var err := ResourceSaver.set_uid(path, id)

	if err != OK:
		printerr("set_uid failed for ", path, ": ", error_string(err))
		quit(1)
		return

	print(ResourceUID.id_to_text(id))
	quit(0)
