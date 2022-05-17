window.onload = function () {
    //<editor-fold desc="Changeable Configuration Block">
    window.ui = SwaggerUIBundle({
        url: "https://petstore.swagger.io/v2/swagger.json",
        dom_id: '#swagger-ui',
        deepLinking: true,
        presets: [
            SwaggerUIBundle.presets.apis
        ]
    })

    //</editor-fold>
};