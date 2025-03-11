
import { createRouter, createWebHistory } from "vue-router";
import HomeView from "./views/Home.vue"; 
import AuthView from "./views/Auth.vue";
import ProfileView from "./views/Profile.vue";

const router = createRouter({ 
  history: createWebHistory(import.meta.env.BASE_URL), 
  routes: [
    { path: "/", name: "home", component: HomeView, }, 
    { path: "/auth", name: "auth", component: AuthView, },
    { path: "/profile", name: "profile", component: ProfileView, },
  ], 
});

export default router;
