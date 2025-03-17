import { createRouter, createWebHistory } from "vue-router";

// Authentication
import AuthView from "./views/Auth.vue";
import Session from "supertokens-web-js/recipe/session";

// Home
import HomeView from "./views/Home.vue"; 

// Users
import UEFView from "./views/Forms/UserEditForm.vue"; 
import CreateUserView from "@/views/Forms/CreateForms/CreateUserForm.vue"; 
import UserShowView from "@/views/UsersShow.vue"; 

// Universities
import UniveritiesShowView from "@/views/UniversitiesShow.vue"; 
import CreateUniView from "@/views/Forms/CreateForms/CreateUniversityForm.vue"; 
import UniveristyEditView from "@/views/Forms/UniversityEditForm.vue"; 

// Academics
import AcademicShowView from "@/views/AcademicShow.vue"; 
import AcademicCreateFormView from "@/views/Forms/CreateForms/CreateAcademicsForm.vue"; 
import AcademicEditView from "@/views/Forms/AcademicEditForm.vue"; 

// Departments
import DepartmentShowView from "@/views/DepartmentShow.vue"; 
import DepartmentCreateFormView from "@/views/Forms/CreateForms/CreateDepartmentsForm.vue"; 
import DepartmentEditVeiw from "@/views/Forms/DepartmentEditForm.vue"; 


// Student Groups
import StudentGroupsShowView from "@/views/StudentGroupsShow.vue"; 
import CreateStudentGroupFormView from "@/views/Forms/CreateForms/CreateStudentGroupForm.vue"; 
import StudentGroupEditView from "@/views/Forms/StudentGroupEdit.vue"; 




const routes = [
  {
    path: "/auth",
    name: "auth",
    component: AuthView,
  },
  {
    path: "/",
    name: "home",
    component: HomeView,
    meta: { requiresAuth: true }
  },
  {
    path: '/profile/edit',
    name: 'profile-edit',
    component: UEFView,
    meta: { requiresAuth: true }
  },
  {
    path: '/university/create',
    name: 'uni-create',
    component: CreateUniView,
    meta: { requiresAuth: true }
  },
  {
    path: '/universities/all',
    name: 'uni-show',
    component: UniveritiesShowView,
    meta: { requiresAuth: true }
  },
  {
    path: '/university/:id',
    name: 'uni-edit',
    component: UniveristyEditView,
    meta: { requiresAuth: true }
  },
  {
    path: '/academic/create',
    name: 'ac-create',
    component: AcademicCreateFormView,
    meta: { requiresAuth: true }
  },
  {
    path: '/academics/all',
    name: 'ac-show',
    component: AcademicShowView,
    meta: { requiresAuth: true }
  },
  {
    path: '/academic/:id',
    name: 'ac-edit',
    component: AcademicEditView,
    meta: { requiresAuth: true }
  },
  {
    path: '/department/create',
    name: 'dp-create',
    component: DepartmentCreateFormView,
    meta: { requiresAuth: true }
  },
  {
    path: '/departments/all',
    name: 'dp-show',
    component: DepartmentShowView,
    meta: { requiresAuth: true }
  },
  {
    path: '/department/:id',
    name: 'dp-edit',
    component: DepartmentEditVeiw,
    meta: { requiresAuth: true }
  },
  {
    path: '/student-group/create',
    name: 'st-group-create',
    component: CreateStudentGroupFormView,
    meta: { requiresAuth: true }
  },
  {
    path: '/student-groups/all',
    name: 'st-group-show',
    component: StudentGroupsShowView,
    meta: { requiresAuth: true }
  },
  {
    path: '/student-group/:id',
    name: 'st-group-edit',
    component: StudentGroupEditView,
    meta: { requiresAuth: true }
  },
  {
    path: '/user/create',
    name: 'user-create',
    component: CreateUserView,
    meta: { requiresAuth: true }
  },
  {
    path: '/users/all',
    name: 'user-show',
    component: UserShowView,
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
