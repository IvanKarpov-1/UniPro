import { createRouter, createWebHistory } from "vue-router";
import HomeView from "./views/Home.vue"; 
import AuthView from "./views/Auth.vue";
import ProfileView from "./views/Profile.vue";
import Session from "supertokens-web-js/recipe/session";
import { doesSessionExist } from "supertokens-web-js"; // функция проверки сессии

const routes = [
  {
    path: "/auth",
    name: "auth",
    component: AuthView,
  },
  {
    path: "/profile",
    name: "profile",
    component: ProfileView,
    meta: { requiresAuth: true }, 
  },
  {
    path: "/",
    name: "home",
    component: HomeView,
    meta: { requiresAuth: true }
  },
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

// global nav guard
router.beforeEach(async (to, from, next) => {
  if (to.meta.requiresAuth) {
    const sessionExists = await Session.doesSessionExist();
    if (!sessionExists) {
      return next("/auth");
    }
  }
  next();
});

export default router;
