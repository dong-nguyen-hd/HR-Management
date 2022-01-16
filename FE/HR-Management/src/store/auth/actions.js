import { api } from "src/boot/axios";

export const login = async ({ commit }, payload) => {
  await api.post("/api/v1/token/login", payload).then((response) => {
    const result = response.data.resource;
    commit("setToken", result.accessToken);
    commit("setInformation", result);
    api.defaults.headers.common.Authorization = "JWT " + result.accessToken;
  });
};

export const logOut = ({ commit }) => {
  api.defaults.headers.common.Authorization = "";
  commit("removeToken");
};

export const init = async ({ commit, dispatch }) => {
  const localResource = localStorage.getItem("hrtoken");
  if (localResource) {
    let resource = JSON.parse(localResource);

    commit("setToken", resource.accessToken);
    commit("setInformation", resource);
    api.defaults.headers.common.Authorization = "JWT " + resource.accessToken;
  } else {
    commit("removeToken");
  }
};
