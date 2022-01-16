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
        path: "/index",
        component: () => import("pages/Index.vue"),
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
