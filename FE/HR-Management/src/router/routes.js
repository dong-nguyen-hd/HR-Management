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
        name: "Index",
        path: "/index",
        component: () => import("pages/Index.vue"),
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
