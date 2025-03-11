
import { createApp } from "vue";
import SuperTokens from "supertokens-web-js";
import Session from "supertokens-web-js/recipe/session";
import App from "./App.vue";
import router from "./router";

SuperTokens.init({
    appInfo: {
        appName: "UniPro",
        apiDomain: "http://localhost",
        apiBasePath: "/api/auth",
    },
    recipeList: [
        Session.init(),
    ],
});

const app = createApp(App);

app.use(router);
app.mount("#app");
