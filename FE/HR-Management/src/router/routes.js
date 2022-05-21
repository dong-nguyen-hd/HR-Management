const routes = [
  {
    name: "Login",
    path: "/login",
    component: () => import("pages/Login.vue"),
  },
  {
    path: "/",
    component: () => import("layouts/MainLayout.vue"),
    meta: { requireLogin: true },
    children: [
      {
        name: "List Employee",
        path: "/list-employee",
        component: () => import("src/pages/Employee/ListEmployee.vue"),
      },
      {
        name: "Update Personal Information",
        path: "/self-update",
        component: () => import("pages/Account/UpdateAccountInformation.vue"),
      },
      {
        name: "Change Password",
        path: "/change-password",
        component: () => import("pages/Account/ChangePassword.vue"),
      },
      {
        name: "Category",
        path: "/category",
        component: () => import("pages/Category/Category.vue"),
      },
      {
        name: "Office",
        path: "/office",
        component: () => import("pages/Office/Office.vue"),
      },
      {
        name: "List Project",
        path: "/project",
        component: () => import("pages/Project/ListProject.vue"),
      },
    ],
  },

  // Always leave this as last one,
  // but you can also remove it
  {
    path: "/:catchAll(.*)*",
    component: () => import("pages/Error404.vue"),
  },
];

export default routes;
