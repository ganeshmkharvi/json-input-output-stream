# json-input-output-stream
Process Json file and return Json output stream such that the primitive properties (such as string, number, and boolean) are at the start of the object.

# Background
When importing JSON data to the model we wish to create objects in an efficient manner. To be able to process incoming JSON data to create objects, we need to interpret all the JSON attributes first before we interpret the JSON objects and JSON arrays. This is because we need to instantiate
new objects and associations when interpreting JSON objects and arrays.

