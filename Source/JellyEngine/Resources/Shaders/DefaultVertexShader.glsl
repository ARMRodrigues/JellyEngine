#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout (location = 3) in vec4 aColor; 
layout (location = 4) in vec2 aTexCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec3 Normal;
out vec3 FragPos;

out vec4 vertexColor;
out vec2 texCoord01;


void main()
{
    vertexColor = aColor;
    texCoord01 = aTexCoord;
    FragPos = vec3(model * vec4(aPosition, 1.0));
    Normal = mat3(transpose(inverse(model))) * aNormal;
    //gl_Position = vec4(aPosition, 1.0);
    //gl_Position = model * vec4(aPosition, 1.0);
    gl_Position = projection * view * vec4(FragPos, 1.0);
}