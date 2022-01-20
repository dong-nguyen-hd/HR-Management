import { api } from "src/boot/axios";

// export const login = async ({ commit }, payload) => {
//   await api.post("/api/v1/token/login", payload).then((response) => {
//     return response.data;
//     const result = response.data.resource;
//     commit("setToken", result.accessToken);
//     commit("setInformation", result);
//     api.defaults.headers.common.Authorization = "JWT " + result.accessToken;
//   });
// };

export const login = async ({ commit }, payload) => {
  await api
    .post("/api/v1/token/login", payload)
    .then((response) => {
      let result = response.data;
      commit("setToken", result.resource.accessToken);
      commit("setInformation", result.resource);
      api.defaults.headers.common.Authorization =
        "JWT " + result.resource.accessToken;
      return result;
    })
    .catch(function (error) {
      if (error.response) {
        return error.response.data;
      } else {
        let temp = { success: false, message: "Server Error!" };
        return temp;
      }
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
