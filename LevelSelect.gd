extends Control


func _on_button_pressed() -> void:
	get_tree().change_scene_to_file("res://scenes/Sandbox.tscn")
	pass # Replace with function body.


func _on_assets_button_pressed() -> void:
	await self.ready
	var tree = get_tree()
	tree.change_scene_to_file("res://scenes/test.tscn")
	#await(get_tree().change_scene_to_file("res://scenes/test.tscn"))
	pass # Replace with function body.
