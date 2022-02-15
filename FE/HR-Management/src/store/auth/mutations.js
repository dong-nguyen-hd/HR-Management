export const setToken = (state, payload) => {
    state.token = payload;
    state.isAuthenticated = true;
}

export const removeToken = (state) => {
    state.token = null;
    state.isAuthenticated = false;
}

export const setInformation = (state, payload) => {
    state.information = payload;
}