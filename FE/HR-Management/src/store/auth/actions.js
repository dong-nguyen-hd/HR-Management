import { api } from "src/boot/axios";

export const login = async ({ commit, dispatch }, payload) => {
  let resource = payload.resource;
  localStorage.setItem("hrtoken", resource.accessToken);
  localStorage.setItem("test", JSON.stringify(resource));
  commit("setToken", resource.accessToken);
  commit("setInformation", resource);
  dispatch("setHeaderJWT");
};

export const logOut = async ({ commit, dispatch }) => {
  localStorage.removeItem('hrtoken');
  commit("removeToken");
  dispatch("setHeaderJWT");
};

export const getInformation = async ({ commit }, id) => {
  await api.get(`/api/v1/account/${id}`).then(response => {
    commit('setInformation', response.data.resource)
  })
}

export const init = async ({ commit, dispatch }) => {
  // let tokenStorage = localStorage.getItem("hrtoken");
  // if (tokenStorage) {
  //   let token = localResource.toString();
  //   commit("setToken", token);
  //   dispatch("setHeaderJWT");

  //   await dispatch()

  //   commit("setInformation", resource);
  // } else {
  //   commit("removeToken");
  // }
};

export const setHeaderJWT = async ({ getters }) => {
  let isAuthenticated = getters.isAuthenticated;

  if (isAuthenticated)
    api.defaults.headers.common.Authorization = "JWT " + getters.getToken;
  else api.defaults.headers.common.Authorization = "";
};
