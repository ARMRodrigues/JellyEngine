#version 330 core

layout (location = 0) in vec3 a_Position;
layout (location = 1) in vec3 a_Normal;
layout (location = 3) in vec4 a_Color;
layout (location = 4) in vec2 a_TexCoord;

uniform mat4 u_Model;
uniform mat4 u_View;
uniform mat4 u_Projection;

out vec3 v_FragPos;
out vec3 v_Normal;
out vec4 v_Color;
out vec2 v_TexCoord;

void main()
{
    v_FragPos = vec3(u_Model * vec4(a_Position, 1.0));
    v_Normal = mat3(transpose(inverse(u_Model))) * a_Normal;
    v_Color = a_Color;
    v_TexCoord = a_TexCoord;
    
    gl_Position = u_Projection * u_View * vec4(v_FragPos, 1.0);
}
