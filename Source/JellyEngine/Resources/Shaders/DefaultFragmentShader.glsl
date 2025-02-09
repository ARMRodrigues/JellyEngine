#version 330 core

in vec3 FragPos;
in vec3 Normal;
in vec4 vertexColor;
in vec2 texCoord01;

out vec4 FragColor;

uniform vec3 uLightDir = vec3(0.49098, -0.1891, -0.8504);
uniform vec3 uViewPos = vec3(0, 0, 10);
uniform vec3 lightColor = vec3(1.0, 0.94, 0.76);
uniform vec3 objectColor = vec3(1.0);

uniform sampler2D u_Texture0;

struct Light {
    //vec3 position;
    vec3 direction;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

uniform Light light;

void main()
{
    //vec4 textureColor = texture(u_Texture0, texCoord01);

    // ambient
    vec3 ambient = light.ambient * texture(u_Texture0, texCoord01).rgb * 0.6f;
    vec3 norm = normalize(Normal);

    vec3 lightDir = normalize(-uLightDir);
    float diff = max(dot(norm, lightDir), 0.0f);
    vec3 diffuse = light.diffuse * diff * texture(u_Texture0, texCoord01).rgb;

    vec3 viewDir = normalize(uViewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);  
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32.0f);
    vec3 specular = light.specular * spec * texture(u_Texture0, texCoord01).rgb;

    vec3 result = ambient + diffuse + specular;
    FragColor = vec4(result, 1.0f);
}