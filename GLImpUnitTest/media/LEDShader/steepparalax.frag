uniform sampler2D base;
uniform sampler2D map;
 
uniform vec3 ambientColor;
uniform vec3 diffuseColor;
uniform vec3 specularColor;
uniform float shininess;
uniform float scale;
uniform float shadow;
 
varying vec3 vNormal;
varying vec3 vTangent;
varying vec3 vBinormal;
 
varying vec3 vPosition;
varying vec2 vUv;
 
varying vec3 tsPosition;
varying vec3 tsCameraPosition;
varying vec3 tsLightSource;
 
void main()
{
    float bumpScale = scale;
 
    // normalize the other tangent space vectors
    vec3 viewVector = normalize(tsCameraPosition - tsPosition);
    vec3 lightVector = normalize(tsLightSource - tsPosition);
 
    vec3 tsE = normalize(tsCameraPosition);
 
    const float numSteps = 30.0; // How many steps the UV ray tracing should take
    float height = 1.0;
    float step = 1.0 / numSteps;
 
    vec2 offset = vUv;
    vec4 NB = texture2D(map, offset);
 
    vec2 delta = vec2(-tsE.x, -tsE.y) * bumpScale / (tsE.z * numSteps);
 
    // find UV offset
    for (float i = 0.0; i < numSteps; i++) {
        if (NB.a < height) {
            height -= step;
            offset += delta;
            NB = texture2D(map, offset);
        } else {
            break;
        }
    }
 
    vec3 color = texture2D(base, offset).rgb;
 
        vec3 normal = texture2D(map, offset).rgb * 2.0 - 1.0;
 
    // calculate this pixel's lighting
        float nxDir = max(0.0, dot(normal, lightVector));
        vec3 ambient = ambientColor * color;
 
        float specularPower = 0.0;
        if(nxDir != 0.0)
        {
                vec3 halfVector = normalize(lightVector + viewVector);
                float nxHalf = max(0.0, dot(normal, halfVector));
                specularPower = pow(nxHalf, shininess);
        }
        vec3 specular = specularColor * specularPower;
 
        vec3 pixel_color = ambient + (diffuseColor * nxDir * color) + specular;
 
    // find shadowing if enabled
        if (shadow == 1.0) {
            vec2 shadow_offset = offset;
        vec3 tsH = normalize(lightVector + tsE);
        float NdotL = max(0.0, dot(normal, lightVector));
 
        float selfShadow = 0.0;
 
        if (NdotL > 0.0) {
 
            const float numShadowSteps = 10.0;
            step = 1.0 / numShadowSteps;
            delta = vec2(lightVector.x, lightVector.y) * bumpScale / (numShadowSteps * lightVector.z);
 
            height = NB.a + step * .1;
 
            for (float i = 0.0; i < numShadowSteps; i++) {
                if (NB.a < height && height < 1.0) {
                    height += step;
                    shadow_offset += delta;
                    NB = texture2D(map, shadow_offset);
                } else {
                    break;
                }
            }
 
            selfShadow = float(NB.a < height);
 
        }
 
        if (selfShadow == 0.0) {
        pixel_color *= .5;
        }
    }
 
    gl_FragColor = vec4(pixel_color, 1.0);
 
    if (offset.x < 0.0 || offset.x > 1.0 || offset.y < 0.0 || offset.y > 1.0) {
        gl_FragColor.a = 0.0;
    }
}