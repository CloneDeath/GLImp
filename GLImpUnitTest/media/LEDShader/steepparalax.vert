uniform vec3 LightSource;
 
attribute vec4 tangent;
 
varying vec2 vUv;
varying vec3 vPosition;
 
varying vec3 vNormal;
varying vec3 vTangent;
varying vec3 vBinormal;
 
varying vec3 tsPosition;
varying vec3 tsCameraPosition;
varying vec3 tsLightSource;
 
void main( void ) {
 
    vUv = vec2(gl_TexCoord[0].x, gl_TexCoord[0].y);
    vPosition = position;
 
    gl_Position = projectionMatrix * modelViewMatrix * vec4(vPosition, 1.0);
 
    // Find & normalize the plane's normal, tangent, and binormal vectors
    vNormal = normalize( normal );
    vTangent = normalize( tangent.xyz );
    vBinormal = normalize( cross( vNormal, vTangent ) * tangent.w );
 
    // Convert vertice, camera, and light vector positions into tangent space
    mat3 TBNMatrix = mat3(vTangent, vBinormal, vNormal);
    tsPosition = position * TBNMatrix;
    tsCameraPosition = cameraPosition * TBNMatrix;
    tsLightSource = LightSource * TBNMatrix;
 
}