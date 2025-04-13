extends Control


func _on_button_pressed() -> void:
	get_tree().change_scene_to_file("res://scenes/Sandbox.tscn")
	pass # Replace with function body.


func _on_button_2_pressed() -> void:
	get_tree().change_scene_to_file("res://scenes/Level1.tscn")
	pass # Replace with function body.


func _on_button_3_pressed() -> void:
	get_tree().change_scene_to_file("res://scenes/Object_Enemy_test.tscn")
	pass # Replace with function body.
