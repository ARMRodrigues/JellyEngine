#version 330 core

in vec3 v_FragPos;
in vec3 v_Normal;
in vec4 v_Color;
in vec2 v_TexCoord;

out vec4 FragColor;

uniform vec3 u_LightDir = vec3(0.49098, -0.1891, -0.8504);
uniform vec3 u_ViewPos = vec3(0, 0, 10);
uniform sampler2D u_Texture0;

struct Light {
    vec3 direction;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

uniform Light u_Light;

void main()
{
    vec3 textureColor = texture(u_Texture0, v_TexCoord).rgb;

    // Ambient Lighting
    vec3 ambient = u_Light.ambient * textureColor * 0.6;

    // Diffuse Lighting
    vec3 norm = normalize(v_Normal);
    vec3 lightDir = normalize(-u_LightDir);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = u_Light.diffuse * diff * textureColor;

    // Specular Lighting
    vec3 viewDir = normalize(u_ViewPos - v_FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32.0);
    vec3 specular = u_Light.specular * spec * textureColor;

    // Final Color Calculation
    vec3 result = (ambient + diffuse + specular) * v_Color.rgb;
    FragColor = vec4(result, v_Color.a);
}
