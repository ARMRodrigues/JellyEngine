#version 330 core

out vec4 FragColor;

in vec2 TexCoord;

uniform vec3 color;
uniform sampler2D spriteTexture;

void main()
{
    vec4 tex_Color = texture2D(spriteTexture, TexCoord);
    FragColor = vec4(tex_Color.rgb * color, tex_Color.a);
}