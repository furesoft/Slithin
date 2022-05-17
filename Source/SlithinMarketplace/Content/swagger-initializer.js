window.onload = function () {
    //<editor-fold desc="Changeable Configuration Block">
    window.ui = SwaggerUIBundle({
        url: "https://slithin-api.multiplayer.co.at/swagger_definition.json",
        dom_id: '#swagger-ui',
        deepLinking: true,
        presets: [
            SwaggerUIBundle.presets.apis
        ]
    })

    //</editor-fold>
};