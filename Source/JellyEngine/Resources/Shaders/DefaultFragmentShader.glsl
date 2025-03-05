#version 330 core

in vec3 v_FragPos;
in vec3 v_Normal;
in vec4 v_Color;
in vec2 v_TexCoord;

out vec4 FragColor;

uniform vec4 u_Color;
uniform vec3 u_ViewPosition;
uniform float u_Shininess;
uniform sampler2D u_Texture0;

struct Light {
    vec3 direction;
    float intensity;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

uniform Light u_Light;

void main()
{
    vec3 textureColor = texture(u_Texture0, v_TexCoord).rgb;

    vec3 norm = normalize(v_Normal);
    vec3 lightDir = normalize(-u_Light.direction);
    vec3 viewDir = normalize(u_ViewPosition - v_FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);

    vec3 ambient = u_Light.ambient * textureColor * u_Light.intensity;

    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = u_Light.diffuse * diff * textureColor;

    float spec = pow(max(dot(viewDir, reflectDir), 0.0), u_Shininess);
    vec3 specular = u_Light.specular * spec * textureColor;

    vec3 result = (ambient + diffuse + specular) * v_Color.rgb * u_Color.rgb;
    FragColor = vec4(result, v_Color.a * u_Color.a);
}
