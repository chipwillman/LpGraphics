/* Generated by JSIL v0.7.9 build 31106. See http://jsil.org/ for more information. */ 
var $asm02 = JSIL.DeclareAssembly("Rifts.WebGL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");

JSIL.DeclareNamespace("Rifts");
JSIL.DeclareNamespace("Rifts.WebGL");
/* class Rifts.WebGL.Page */ 

(function Page$Members () {
  var $, $thisType;
  var $T00 = function () {
    return ($T00 = JSIL.Memoize($asm02.Rifts.WebGL.Page_AttributeCollection)) ();
  };
  var $T01 = function () {
    return ($T01 = JSIL.Memoize($asm02.Rifts.WebGL.Page_UniformCollection)) ();
  };
  var $T02 = function () {
    return ($T02 = JSIL.Memoize($asm02.Rifts.WebGL.Page_BufferCollection)) ();
  };
  var $T03 = function () {
    return ($T03 = JSIL.Memoize($asm02.Rifts.WebGL.Page_MatrixCollection)) ();
  };
  var $T04 = function () {
    return ($T04 = JSIL.Memoize($asm01.System.Boolean)) ();
  };
  var $T05 = function () {
    return ($T05 = JSIL.Memoize($asm01.System.Int32)) ();
  };
  var $T06 = function () {
    return ($T06 = JSIL.Memoize($asm01.System.Environment)) ();
  };
  var $T07 = function () {
    return ($T07 = JSIL.Memoize($asm01.System.String)) ();
  };
  var $T08 = function () {
    return ($T08 = JSIL.Memoize($asm01.System.IO.Path)) ();
  };
  var $T09 = function () {
    return ($T09 = JSIL.Memoize($asm01.System.Object)) ();
  };
  var $T0A = function () {
    return ($T0A = JSIL.Memoize($asm01.System.IO.File)) ();
  };
  var $T0B = function () {
    return ($T0B = JSIL.Memoize($asm01.System.Console)) ();
  };
  var $T0C = function () {
    return ($T0C = JSIL.Memoize($asm01.System.NotImplementedException)) ();
  };
  var $T0D = function () {
    return ($T0D = JSIL.Memoize($asm01.System.Single)) ();
  };
  var $T0E = function () {
    return ($T0E = JSIL.Memoize(System.Array.Of($asm01.System.Single))) ();
  };
  var $T0F = function () {
    return ($T0F = JSIL.Memoize(System.Array.Of($asm01.System.Object))) ();
  };
  var $T10 = function () {
    return ($T10 = JSIL.Memoize(System.Array.Of($asm01.System.UInt16))) ();
  };
  var $T11 = function () {
    return ($T11 = JSIL.Memoize($asm02.Rifts.WebGL.CubeData)) ();
  };
  var $T12 = function () {
    return ($T12 = JSIL.Memoize($asm01.System.Exception)) ();
  };
  var $T13 = function () {
    return ($T13 = JSIL.Memoize($asm02.Rifts.WebGL.Page_$l$gc__DisplayClass71)) ();
  };
  var $T14 = function () {
    return ($T14 = JSIL.Memoize($asm01.System.Action)) ();
  };
  var $T15 = function () {
    return ($T15 = JSIL.Memoize(System.Array.Of($asm01.System.Byte))) ();
  };
  var $T16 = function () {
    return ($T16 = JSIL.Memoize($asm01.System.Action$b1.Of($asm01.System.Object))) ();
  };
  var $S00 = function () {
    return ($S00 = JSIL.Memoize(new JSIL.ConstructorSignature($asm01.TypeRef("System.NotImplementedException"), [$asm01.TypeRef("System.String")]))) ();
  };

  function Page_Animate () {
    var now = ($T06().get_TickCount() | 0);
    if (($thisType.LastTime | 0) !== 0) {
      var elapsed = ((now - ($thisType.LastTime | 0)) | 0);
      $thisType.RotationX += +(((+$thisType.SpeedX * +elapsed) / 1000));
      $thisType.RotationY += +(((+$thisType.SpeedY * +elapsed) / 1000));
    }
    $thisType.LastTime = now;
  };

  function Page_CompileShader (filename) {

    var $label0 = 0;
  $labelgroup0: 
    while (true) {
      switch ($label0) {
        case 0: /* $entry0 */ 
          var extension = ($T08().GetExtension(filename).toLowerCase());
          var text = extension;
          if (text !== null) {
            if (!(text == "fs")) {
              if (!(text == "vs")) {
                $label0 = 1 /* goto IL_1B4 */ ;
                continue $labelgroup0;
              }
              var arg_1AC_2 = $thisType.GL;
              var shaderObject = arg_1AC_2.createShader($thisType.GL.VERTEX_SHADER);
            } else {
              shaderObject = $thisType.GL.createShader($thisType.GL.FRAGMENT_SHADER);
            }
            var shaderText = $T0A().ReadAllText(filename);
            $thisType.GL.shaderSource(shaderObject, shaderText);
            $thisType.GL.compileShader(shaderObject);
            var arg_399_3 = shaderObject;
            if (!$T04().$Cast($thisType.GL.getShaderParameter(arg_399_3, $thisType.GL.COMPILE_STATUS))) {
              var arg_477_2 = JSIL.GlobalNamespace.alert;
              arg_477_2($thisType.GL.getShaderInfoLog(shaderObject));
              var result = null;
            } else {
              $T0B().WriteLine(JSIL.ConcatString("Loaded ", filename));
              result = shaderObject;
            }
            return result;
          }

          $label0 = 1 /* goto IL_1B4 */ ;
          continue $labelgroup0;
        case 1: /* IL_1B4 */ 
          throw $S00().Construct(extension);

          break $labelgroup0;
      }
    }
  };

  function Page_DegreesToRadians (degrees) {
    return Math.fround(+((+degrees * 3.1415926535897931) / 180));
  };

  function Page_DrawScene () {
    $thisType.GL.viewport(0, 0, $thisType.Canvas.width, $thisType.Canvas.height);
    var arg_27D_2 = $thisType.GL.COLOR_BUFFER_BIT;
    $thisType.GL.clear(arg_27D_2 | $thisType.GL.DEPTH_BUFFER_BIT);
    $thisType.GLMatrix4.identity($thisType.Matrices.ModelView);
    $thisType.GLMatrix4.translate($thisType.Matrices.ModelView, JSIL.Array.New($T0D(), [0, 0, $thisType.Z]));
    var array = JSIL.Array.New($T0D(), 3);
    array[0] = 1;
    $thisType.GLMatrix4.rotate($thisType.Matrices.ModelView, $thisType.DegreesToRadians($thisType.RotationX), array);
    array = JSIL.Array.New($T0D(), 3);
    array[1] = 1;
    $thisType.GLMatrix4.rotate($thisType.Matrices.ModelView, $thisType.DegreesToRadians($thisType.RotationY), array);
    $thisType.GL.bindBuffer($thisType.GL.ARRAY_BUFFER, $thisType.Buffers.CubeVertexPositions);
    $thisType.GL.vertexAttribPointer(
      $thisType.Attributes.VertexPosition, 
      3, 
      $thisType.GL.FLOAT, 
      false, 
      0, 
      0
    );
    $thisType.GL.bindBuffer($thisType.GL.ARRAY_BUFFER, $thisType.Buffers.CubeVertexNormals);
    $thisType.GL.vertexAttribPointer(
      $thisType.Attributes.VertexNormal, 
      3, 
      $thisType.GL.FLOAT, 
      false, 
      0, 
      0
    );
    $thisType.GL.bindBuffer($thisType.GL.ARRAY_BUFFER, $thisType.Buffers.CubeTextureCoords);
    $thisType.GL.vertexAttribPointer(
      $thisType.Attributes.TextureCoord, 
      2, 
      $thisType.GL.FLOAT, 
      false, 
      0, 
      0
    );
    $thisType.GL.activeTexture($thisType.GL.TEXTURE0);
    $thisType.GL.bindTexture($thisType.GL.TEXTURE_2D, $thisType.CrateTexture);
    $thisType.GL.uniform1i($thisType.Uniforms.Sampler, 0);
    var lighting = $T04().$Cast($thisType.Document.getElementById("lighting").checked);
    $thisType.GL.uniform1i($thisType.Uniforms.UseLighting, (
        lighting
           ? 1
           : 0)
    );
    if (!lighting) {
      var arg_1126_4 = $T0D().Parse($thisType.Document.getElementById("ambientR").value);
      var arg_1126_5 = $T0D().Parse($thisType.Document.getElementById("ambientG").value);
      $thisType.GL.uniform3f($thisType.Uniforms.AmbientColor, arg_1126_4, arg_1126_5, $T0D().Parse($thisType.Document.getElementById("ambientB").value));
      var array2 = JSIL.Array.New($T09(), 3);
      var arg_124C_0 = array2;
      arg_124C_0[0] = $T0D().Parse($thisType.Document.getElementById("lightDirectionX").value);
      var arg_1365_0 = array2;
      arg_1365_0[1] = $T0D().Parse($thisType.Document.getElementById("lightDirectionY").value);
      var arg_147E_0 = array2;
      arg_147E_0[2] = $T0D().Parse($thisType.Document.getElementById("lightDirectionZ").value);
      var lightingDirection = array2;
      $thisType.GLVector3.normalize(lightingDirection, lightingDirection);
      $thisType.GLVector3.scale(lightingDirection, -1);
      $thisType.GL.uniform3fv($thisType.Uniforms.LightingDirection, lightingDirection);
      var arg_199C_4 = $T0D().Parse($thisType.Document.getElementById("directionalR").value);
      var arg_199C_5 = $T0D().Parse($thisType.Document.getElementById("directionalG").value);
      $thisType.GL.uniform3f($thisType.Uniforms.DirectionalColor, arg_199C_4, arg_199C_5, $T0D().Parse($thisType.Document.getElementById("directionalB").value));
    }
    $thisType.GL.bindBuffer($thisType.GL.ELEMENT_ARRAY_BUFFER, $thisType.Buffers.CubeIndices);
    $thisType.GL.uniformMatrix4fv($thisType.Uniforms.ProjectionMatrix, false, $thisType.Matrices.Projection);
    $thisType.GL.uniformMatrix4fv($thisType.Uniforms.ModelViewMatrix, false, $thisType.Matrices.ModelView);
    $thisType.GLMatrix4.toInverseMat3($thisType.Matrices.ModelView, $thisType.Matrices.Normal);
    $thisType.GLMatrix3.transpose($thisType.Matrices.Normal);
    $thisType.GL.uniformMatrix3fv($thisType.Uniforms.NormalMatrix, false, $thisType.Matrices.Normal);
    var arg_1E28_3 = $thisType.GL.TRIANGLES;
    var arg_1E28_4 = ($T11().Indices.length | 0);
    $thisType.GL.drawElements(arg_1E28_3, arg_1E28_4, $thisType.GL.UNSIGNED_SHORT, 0);
  };

  function Page_HandleKeys () {
    if ($thisType.HeldKeys[33]) {
      $thisType.Z -= 0.05;
    }
    if ($thisType.HeldKeys[34]) {
      $thisType.Z += 0.05;
    }
    if ($thisType.HeldKeys[37]) {
      $thisType.SpeedY -= 1;
    }
    if ($thisType.HeldKeys[39]) {
      $thisType.SpeedY += 1;
    }
    if ($thisType.HeldKeys[38]) {
      $thisType.SpeedX -= 1;
    }
    if ($thisType.HeldKeys[40]) {
      $thisType.SpeedX += 1;
    }
  };

  function Page_InitBuffers () {
    var arg_11C_3 = $thisType.GL.ARRAY_BUFFER;
    $thisType.GL.bindBuffer(arg_11C_3, $thisType.Buffers.CubeVertexPositions = $thisType.GL.createBuffer());
    var arg_23E_3 = $thisType.GL.ARRAY_BUFFER;
    $thisType.GL.bufferData(arg_23E_3, $T11().Positions, $thisType.GL.STATIC_DRAW);
    var arg_35F_3 = $thisType.GL.ARRAY_BUFFER;
    $thisType.GL.bindBuffer(arg_35F_3, $thisType.Buffers.CubeVertexNormals = $thisType.GL.createBuffer());
    var arg_481_3 = $thisType.GL.ARRAY_BUFFER;
    $thisType.GL.bufferData(arg_481_3, $T11().Normals, $thisType.GL.STATIC_DRAW);
    var arg_5A2_3 = $thisType.GL.ARRAY_BUFFER;
    $thisType.GL.bindBuffer(arg_5A2_3, $thisType.Buffers.CubeTextureCoords = $thisType.GL.createBuffer());
    var arg_6C4_3 = $thisType.GL.ARRAY_BUFFER;
    $thisType.GL.bufferData(arg_6C4_3, $T11().TexCoords, $thisType.GL.STATIC_DRAW);
    var arg_7E5_3 = $thisType.GL.ELEMENT_ARRAY_BUFFER;
    $thisType.GL.bindBuffer(arg_7E5_3, $thisType.Buffers.CubeIndices = $thisType.GL.createBuffer());
    var arg_907_3 = $thisType.GL.ELEMENT_ARRAY_BUFFER;
    $thisType.GL.bufferData(arg_907_3, $T11().Indices, $thisType.GL.STATIC_DRAW);
  };

  function Page_InitGL ($exception) {
    var gl = null;
    try {
      gl = $thisType.Canvas.getContext("experimental-webgl");
    } catch ($exception) {
    }
    if (gl) {
      $thisType.GL = gl;
      $T0B().WriteLine("Initialized WebGL");
      var result = true;
    } else {
      JSIL.GlobalNamespace.alert("Could not initialize WebGL");
      result = false;
    }
    return result;
  };

  function Page_InitMatrices () {
    $thisType.Matrices.ModelView = $T0E().$Cast($thisType.GLMatrix4.create());
    $thisType.Matrices.Projection = $T0E().$Cast($thisType.GLMatrix4.create());
    $thisType.Matrices.Normal = $T0E().$Cast($thisType.GLMatrix3.create());
    $thisType.GLMatrix4.perspective(
      45, 
      $thisType.Canvas.width / $thisType.Canvas.height, 
      0.1, 
      100, 
      $thisType.Matrices.Projection
    );
  };

  function Page_InitShaders () {
    var fragmentShader = $thisType.CompileShader("crate.fs");
    var vertexShader = $thisType.CompileShader("crate.vs");
    $thisType.ShaderProgram = $thisType.GL.createProgram();
    $thisType.GL.attachShader($thisType.ShaderProgram, vertexShader);
    $thisType.GL.attachShader($thisType.ShaderProgram, fragmentShader);
    $thisType.GL.linkProgram($thisType.ShaderProgram);
    if (!$T04().$Cast($thisType.GL.getProgramParameter($thisType.ShaderProgram, $thisType.GL.LINK_STATUS))) {
      JSIL.GlobalNamespace.alert("Could not link shader");
    } else {
      $thisType.GL.useProgram($thisType.ShaderProgram);
      $thisType.Attributes.VertexPosition = $thisType.GL.getAttribLocation($thisType.ShaderProgram, "aVertexPosition");
      $thisType.Attributes.VertexNormal = $thisType.GL.getAttribLocation($thisType.ShaderProgram, "aVertexNormal");
      $thisType.Attributes.TextureCoord = $thisType.GL.getAttribLocation($thisType.ShaderProgram, "aTextureCoord");
      $thisType.Uniforms.ProjectionMatrix = $thisType.GL.getUniformLocation($thisType.ShaderProgram, "uPMatrix");
      $thisType.Uniforms.ModelViewMatrix = $thisType.GL.getUniformLocation($thisType.ShaderProgram, "uMVMatrix");
      $thisType.Uniforms.NormalMatrix = $thisType.GL.getUniformLocation($thisType.ShaderProgram, "uNMatrix");
      $thisType.Uniforms.Sampler = $thisType.GL.getUniformLocation($thisType.ShaderProgram, "uSampler");
      $thisType.Uniforms.UseLighting = $thisType.GL.getUniformLocation($thisType.ShaderProgram, "uUseLighting");
      $thisType.Uniforms.AmbientColor = $thisType.GL.getUniformLocation($thisType.ShaderProgram, "uAmbientColor");
      $thisType.Uniforms.LightingDirection = $thisType.GL.getUniformLocation($thisType.ShaderProgram, "uLightingDirection");
      $thisType.Uniforms.DirectionalColor = $thisType.GL.getUniformLocation($thisType.ShaderProgram, "uDirectionalColor");
      $thisType.GL.enableVertexAttribArray($thisType.Attributes.VertexPosition);
      $thisType.GL.enableVertexAttribArray($thisType.Attributes.VertexNormal);
      $thisType.GL.enableVertexAttribArray($thisType.Attributes.TextureCoord);
    }
  };

  function Page_InitTexture ($exception) {
    var $closure0 = new ($T13())();
    $thisType.CrateTexture = $thisType.GL.createTexture();
    var arg_C4_0 = $closure0;
    arg_C4_0.imageElement = $thisType.Document.createElement("img");
    $closure0.imageElement.onload = $T14().New($closure0, $T13().prototype.$lInitTexture$gb__70);
    try {
      var imageBytes = $T0A().ReadAllBytes("crate.png");
      var objectUrl = JSIL.GlobalNamespace.JSIL.GetObjectURLForBytes(imageBytes, "image/png");
      $closure0.imageElement.src = objectUrl;
    } catch ($exception) {
      $T0B().WriteLine("Falling back to a second HTTP request for crate.png because Object URLs are not available");
      $closure0.imageElement.src = "Files/crate.png";
    }
  };

  function Page_Load () {
    $thisType.Document = JSIL.GlobalNamespace.document;
    $thisType.Canvas = $thisType.Document.getElementById("canvas");
    $thisType.GLVector3 = JSIL.GlobalNamespace.vec3;
    $thisType.GLMatrix4 = JSIL.GlobalNamespace.mat4;
    $thisType.GLMatrix3 = JSIL.GlobalNamespace.mat3;
    if ($thisType.InitGL()) {
      $thisType.InitMatrices();
      $thisType.InitShaders();
      $thisType.InitBuffers();
      $thisType.InitTexture();
      $thisType.GL.clearColor(0, 0, 0, 1);
      $thisType.GL.enable($thisType.GL.DEPTH_TEST);
      $thisType.Document.onkeydown = $T16().New($thisType, $thisType.OnKeyDown);
      $thisType.Document.onkeyup = $T16().New($thisType, $thisType.OnKeyUp);
      $thisType.Tick();
    }
  };

  function Page_OnKeyDown (e) {
    $thisType.HeldKeys[$T05().$Cast(e.keyCode)] = 1;
  };

  function Page_OnKeyUp (e) {
    $thisType.HeldKeys[$T05().$Cast(e.keyCode)] = 0;
  };

  function Page_Tick () {
    JSIL.GlobalNamespace.requestAnimFrame($T14().New($thisType, $thisType.Tick));
    $thisType.HandleKeys();
    $thisType.DrawScene();
    $thisType.Animate();
  };

  function Page_UploadTexture (textureHandle, imageElement) {
    $thisType.GL.pixelStorei($thisType.GL.UNPACK_FLIP_Y_WEBGL, true);
    $thisType.GL.bindTexture($thisType.GL.TEXTURE_2D, textureHandle);
    var arg_360_3 = $thisType.GL.TEXTURE_2D;
    var arg_360_5 = $thisType.GL.RGBA;
    var arg_360_6 = $thisType.GL.RGBA;
    $thisType.GL.texImage2D(
      arg_360_3, 
      0, 
      arg_360_5, 
      arg_360_6, 
      $thisType.GL.UNSIGNED_BYTE, 
      imageElement
    );
    var arg_4D0_3 = $thisType.GL.TEXTURE_2D;
    var arg_4D0_4 = $thisType.GL.TEXTURE_MAG_FILTER;
    $thisType.GL.texParameteri(arg_4D0_3, arg_4D0_4, $thisType.GL.LINEAR);
    var arg_640_3 = $thisType.GL.TEXTURE_2D;
    var arg_640_4 = $thisType.GL.TEXTURE_MIN_FILTER;
    $thisType.GL.texParameteri(arg_640_3, arg_640_4, $thisType.GL.LINEAR_MIPMAP_NEAREST);
    $thisType.GL.generateMipmap($thisType.GL.TEXTURE_2D);
    $thisType.GL.bindTexture($thisType.GL.TEXTURE_2D, null);
  };

  JSIL.MakeStaticClass("Rifts.WebGL.Page", true, [], function ($interfaceBuilder) {
    $ = $interfaceBuilder;

    $.Method({Static:true , Public:true }, "Animate", 
      JSIL.MethodSignature.Void, 
      Page_Animate
    );

    $.Method({Static:true , Public:true }, "CompileShader", 
      new JSIL.MethodSignature($.Object, [$.String]), 
      Page_CompileShader
    );

    $.Method({Static:true , Public:true }, "DegreesToRadians", 
      new JSIL.MethodSignature($.Single, [$.Single]), 
      Page_DegreesToRadians
    );

    $.Method({Static:true , Public:true }, "DrawScene", 
      JSIL.MethodSignature.Void, 
      Page_DrawScene
    );

    $.Method({Static:true , Public:true }, "HandleKeys", 
      JSIL.MethodSignature.Void, 
      Page_HandleKeys
    );

    $.Method({Static:true , Public:true }, "InitBuffers", 
      JSIL.MethodSignature.Void, 
      Page_InitBuffers
    );

    $.Method({Static:true , Public:true }, "InitGL", 
      JSIL.MethodSignature.Return($.Boolean), 
      Page_InitGL
    );

    $.Method({Static:true , Public:true }, "InitMatrices", 
      JSIL.MethodSignature.Void, 
      Page_InitMatrices
    );

    $.Method({Static:true , Public:true }, "InitShaders", 
      JSIL.MethodSignature.Void, 
      Page_InitShaders
    );

    $.Method({Static:true , Public:true }, "InitTexture", 
      JSIL.MethodSignature.Void, 
      Page_InitTexture
    );

    $.Method({Static:true , Public:true }, "Load", 
      JSIL.MethodSignature.Void, 
      Page_Load
    );

    $.Method({Static:true , Public:true }, "OnKeyDown", 
      JSIL.MethodSignature.Action($.Object), 
      Page_OnKeyDown
    )
      .Parameter(0, "e", function (_) {
          _.Attribute($asm05.TypeRef("System.Runtime.CompilerServices.DynamicAttribute"))
        });

    $.Method({Static:true , Public:true }, "OnKeyUp", 
      JSIL.MethodSignature.Action($.Object), 
      Page_OnKeyUp
    )
      .Parameter(0, "e", function (_) {
          _.Attribute($asm05.TypeRef("System.Runtime.CompilerServices.DynamicAttribute"))
        });

    $.Method({Static:true , Public:true }, "Tick", 
      JSIL.MethodSignature.Void, 
      Page_Tick
    );

    $.Method({Static:true , Public:true }, "UploadTexture", 
      new JSIL.MethodSignature(null, [$.Object, $.Object]), 
      Page_UploadTexture
    );

    $.Field({Static:true , Public:true }, "GL", $.Object)
      .Attribute($asm05.TypeRef("System.Runtime.CompilerServices.DynamicAttribute")); 
    $.Field({Static:true , Public:true }, "Document", $.Object)
      .Attribute($asm05.TypeRef("System.Runtime.CompilerServices.DynamicAttribute")); 
    $.Field({Static:true , Public:true }, "Canvas", $.Object)
      .Attribute($asm05.TypeRef("System.Runtime.CompilerServices.DynamicAttribute")); 
    $.Field({Static:true , Public:true }, "GLVector3", $.Object)
      .Attribute($asm05.TypeRef("System.Runtime.CompilerServices.DynamicAttribute")); 
    $.Field({Static:true , Public:true }, "GLMatrix3", $.Object)
      .Attribute($asm05.TypeRef("System.Runtime.CompilerServices.DynamicAttribute")); 
    $.Field({Static:true , Public:true }, "GLMatrix4", $.Object)
      .Attribute($asm05.TypeRef("System.Runtime.CompilerServices.DynamicAttribute")); 
    $.Field({Static:true , Public:true }, "ShaderProgram", $.Object); 
    $.Field({Static:true , Public:true }, "CrateTexture", $.Object); 
    $.Field({Static:true , Public:true , ReadOnly:true }, "Attributes", $asm02.TypeRef("Rifts.WebGL.Page+AttributeCollection")); 
    $.Field({Static:true , Public:true , ReadOnly:true }, "Uniforms", $asm02.TypeRef("Rifts.WebGL.Page+UniformCollection")); 
    $.Field({Static:true , Public:true , ReadOnly:true }, "Buffers", $asm02.TypeRef("Rifts.WebGL.Page+BufferCollection")); 
    $.Field({Static:true , Public:true , ReadOnly:true }, "Matrices", $asm02.TypeRef("Rifts.WebGL.Page+MatrixCollection")); 
    $.Field({Static:true , Public:true }, "HeldKeys", $jsilcore.TypeRef("System.Array", [$.Boolean]), function ($pi) {
        return JSIL.Array.New($asm01.System.Boolean, 255);
      }); 
    $.Field({Static:true , Public:true }, "LastTime", $.Int32, 0); 
    $.Field({Static:true , Public:true }, "Z", $.Single, -5); 
    $.Field({Static:true , Public:true }, "RotationX", $.Single); 
    $.Field({Static:true , Public:true }, "RotationY", $.Single); 
    $.Field({Static:true , Public:true }, "SpeedX", $.Single, 3); 
    $.Field({Static:true , Public:true }, "SpeedY", $.Single, -3); 
    function Page__cctor () {
      $thisType.Attributes = new ($T00())();
      $thisType.Uniforms = new ($T01())();
      $thisType.Buffers = new ($T02())();
      $thisType.Matrices = new ($T03())();
      $thisType.HeldKeys = JSIL.Array.New($T04(), 255);
      $thisType.LastTime = 0;
      $thisType.Z = -5;
      $thisType.SpeedX = 3;
      $thisType.SpeedY = -3;
    };


    $.Method({Static:true , Public:false}, ".cctor", 
      JSIL.MethodSignature.Void, 
      Page__cctor
    );

    return function (newThisType) { $thisType = newThisType; }; 
  });

})();

/* class Rifts.WebGL.Page+AttributeCollection */ 

(function AttributeCollection$Members () {
  var $, $thisType;
  function AttributeCollection__ctor () {
  };

  JSIL.MakeType({
      BaseType: $asm01.TypeRef("System.Object"), 
      Name: "Rifts.WebGL.Page+AttributeCollection", 
      IsPublic: false, 
      IsReferenceType: true, 
      MaximumConstructorArguments: 0, 
    }, function ($interfaceBuilder) {
    $ = $interfaceBuilder;

    $.Method({Static:false, Public:true }, ".ctor", 
      JSIL.MethodSignature.Void, 
      AttributeCollection__ctor
    );

    $.Field({Static:false, Public:true }, "VertexPosition", $.Object); 
    $.Field({Static:false, Public:true }, "VertexNormal", $.Object); 
    $.Field({Static:false, Public:true }, "TextureCoord", $.Object); 
    return function (newThisType) { $thisType = newThisType; }; 
  });

})();

/* class Rifts.WebGL.Page+UniformCollection */ 

(function UniformCollection$Members () {
  var $, $thisType;
  function UniformCollection__ctor () {
  };

  JSIL.MakeType({
      BaseType: $asm01.TypeRef("System.Object"), 
      Name: "Rifts.WebGL.Page+UniformCollection", 
      IsPublic: false, 
      IsReferenceType: true, 
      MaximumConstructorArguments: 0, 
    }, function ($interfaceBuilder) {
    $ = $interfaceBuilder;

    $.Method({Static:false, Public:true }, ".ctor", 
      JSIL.MethodSignature.Void, 
      UniformCollection__ctor
    );

    $.Field({Static:false, Public:true }, "ProjectionMatrix", $.Object); 
    $.Field({Static:false, Public:true }, "ModelViewMatrix", $.Object); 
    $.Field({Static:false, Public:true }, "NormalMatrix", $.Object); 
    $.Field({Static:false, Public:true }, "Sampler", $.Object); 
    $.Field({Static:false, Public:true }, "UseLighting", $.Object); 
    $.Field({Static:false, Public:true }, "AmbientColor", $.Object); 
    $.Field({Static:false, Public:true }, "LightingDirection", $.Object); 
    $.Field({Static:false, Public:true }, "DirectionalColor", $.Object); 
    return function (newThisType) { $thisType = newThisType; }; 
  });

})();

/* class Rifts.WebGL.Page+BufferCollection */ 

(function BufferCollection$Members () {
  var $, $thisType;
  function BufferCollection__ctor () {
  };

  JSIL.MakeType({
      BaseType: $asm01.TypeRef("System.Object"), 
      Name: "Rifts.WebGL.Page+BufferCollection", 
      IsPublic: false, 
      IsReferenceType: true, 
      MaximumConstructorArguments: 0, 
    }, function ($interfaceBuilder) {
    $ = $interfaceBuilder;

    $.Method({Static:false, Public:true }, ".ctor", 
      JSIL.MethodSignature.Void, 
      BufferCollection__ctor
    );

    $.Field({Static:false, Public:true }, "CubeVertexPositions", $.Object); 
    $.Field({Static:false, Public:true }, "CubeVertexNormals", $.Object); 
    $.Field({Static:false, Public:true }, "CubeTextureCoords", $.Object); 
    $.Field({Static:false, Public:true }, "CubeIndices", $.Object); 
    return function (newThisType) { $thisType = newThisType; }; 
  });

})();

/* class Rifts.WebGL.Page+MatrixCollection */ 

(function MatrixCollection$Members () {
  var $, $thisType;
  function MatrixCollection__ctor () {
  };

  JSIL.MakeType({
      BaseType: $asm01.TypeRef("System.Object"), 
      Name: "Rifts.WebGL.Page+MatrixCollection", 
      IsPublic: false, 
      IsReferenceType: true, 
      MaximumConstructorArguments: 0, 
    }, function ($interfaceBuilder) {
    $ = $interfaceBuilder;

    $.Method({Static:false, Public:true }, ".ctor", 
      JSIL.MethodSignature.Void, 
      MatrixCollection__ctor
    );

    $.Field({Static:false, Public:true }, "Projection", $jsilcore.TypeRef("System.Array", [$.Single])); 
    $.Field({Static:false, Public:true }, "ModelView", $jsilcore.TypeRef("System.Array", [$.Single])); 
    $.Field({Static:false, Public:true }, "Normal", $jsilcore.TypeRef("System.Array", [$.Single])); 
    return function (newThisType) { $thisType = newThisType; }; 
  });

})();

/* class Rifts.WebGL.Page+<>c__DisplayClass71 */ 

(function $l$gc__DisplayClass71$Members () {
  var $, $thisType;
  var $T00 = function () {
    return ($T00 = JSIL.Memoize($asm02.Rifts.WebGL.Page)) ();
  };

  function $l$gc__DisplayClass71__ctor () {
  };

  function $l$gc__DisplayClass71_$lInitTexture$gb__70 () {
    $T00().UploadTexture($T00().CrateTexture, this.imageElement);
  };

  JSIL.MakeType({
      BaseType: $asm01.TypeRef("System.Object"), 
      Name: "Rifts.WebGL.Page+<>c__DisplayClass71", 
      IsPublic: false, 
      IsReferenceType: true, 
      MaximumConstructorArguments: 0, 
    }, function ($interfaceBuilder) {
    $ = $interfaceBuilder;

    $.Method({Static:false, Public:true }, ".ctor", 
      JSIL.MethodSignature.Void, 
      $l$gc__DisplayClass71__ctor
    );

    $.Method({Static:false, Public:true }, "$lInitTexture$gb__70", 
      JSIL.MethodSignature.Void, 
      $l$gc__DisplayClass71_$lInitTexture$gb__70
    );

    $.Field({Static:false, Public:true }, "imageElement", $.Object); 
    return function (newThisType) { $thisType = newThisType; }; 
  })
    .Attribute($asm01.TypeRef("System.Runtime.CompilerServices.CompilerGeneratedAttribute"));

})();

/* class Rifts.WebGL.CubeData */ 

(function CubeData$Members () {
  var $, $thisType;
  var $T00 = function () {
    return ($T00 = JSIL.Memoize($asm01.System.Single)) ();
  };
  var $T01 = function () {
    return ($T01 = JSIL.Memoize($asm01.System.UInt16)) ();
  };

  JSIL.MakeStaticClass("Rifts.WebGL.CubeData", true, [], function ($interfaceBuilder) {
    $ = $interfaceBuilder;

    $.Field({Static:true , Public:true , ReadOnly:true }, "Positions", $jsilcore.TypeRef("System.Array", [$.Single])); 
    $.Field({Static:true , Public:true , ReadOnly:true }, "Normals", $jsilcore.TypeRef("System.Array", [$.Single])); 
    $.Field({Static:true , Public:true , ReadOnly:true }, "TexCoords", $jsilcore.TypeRef("System.Array", [$.Single])); 
    $.Field({Static:true , Public:true , ReadOnly:true }, "Indices", $jsilcore.TypeRef("System.Array", [$.UInt16])); 
    function CubeData__cctor () {
      $thisType.Positions = JSIL.Array.New($T00(), [-1, -1, 1, 1, -1, 1, 1, 1, 1, -1, 1, 1, -1, -1, -1, -1, 1, -1, 1, 1, -1, 1, -1, -1, -1, 1, -1, -1, 1, 1, 1, 1, 1, 1, 1, -1, -1, -1, -1, 1, -1, -1, 1, -1, 1, -1, -1, 1, 1, -1, -1, 1, 1, -1, 1, 1, 1, 1, -1, 1, -1, -1, -1, -1, -1, 1, -1, 1, 1, -1, 1, -1]);
      $thisType.Normals = JSIL.Array.New($T00(), [0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0]);
      $thisType.TexCoords = JSIL.Array.New($T00(), [0, 0, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 1, 1, 1, 1, 0, 1, 0, 0, 1, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1]);
      $thisType.Indices = JSIL.Array.New($T01(), [0, 1, 2, 0, 2, 3, 4, 5, 6, 4, 6, 7, 8, 9, 10, 8, 10, 11, 12, 13, 14, 12, 14, 15, 16, 17, 18, 16, 18, 19, 20, 21, 22, 20, 22, 23]);
    };


    $.Method({Static:true , Public:false}, ".cctor", 
      JSIL.MethodSignature.Void, 
      CubeData__cctor
    );

    return function (newThisType) { $thisType = newThisType; }; 
  });

})();

