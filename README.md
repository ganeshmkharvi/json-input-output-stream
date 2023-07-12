<p align="center">
  <h3 align="center">ReOrder Json file with primitive types ordered first including objects and arrays </h3>
  <p align="center">
    <a href="https://github.com/ganeshmkharvi/json-input-output-stream">Report Bug</a>
    ·
    <a href="https://github.com/ganeshmkharvi/json-input-output-stream""> Request Feature</a>
  </p>
</p>

# Background
When importing JSON data to the model we wish to create objects in an efficient manner. To be able to process incoming JSON data to create objects, we need to interpret all the JSON attributes first before we interpret the JSON objects and JSON arrays. This is because we need to instantiate
new objects and associations when interpreting JSON objects and arrays.

