
var jsilConfig = {
    manifests: [
        "Scripts/RiftGL.dll",
        "Scripts/Files/RiftGL.csproj"
    ],

    readOnlyStorage: true,

    scriptRoot: "Scripts/",
    libraryRoot: "Scripts/Libraries/",
    fileRoot: "Scripts/Files/",
    contentRoot: "Content/",
    fileVirtualRoot: ""
};

function runMain() {
    var asm = JSIL.GetAssembly("RiftGL");
    asm.RiftGL.Page.Load();
}

//if (window.addEventListener) {
//    window.addEventListener('load', runMain, false); //W3C
//}
//else {
//    window.attachEvent('onload', runMain); //IE
//}
