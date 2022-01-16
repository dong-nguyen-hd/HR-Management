export const setToken = (state, payload) => {
    state.token = payload;
    state.isAuthenticated = true;
}

export const removeToken = (state, payload) => {
    state.token = '';
    state.isAuthenticated = false;
}

export const setInformation = (state, payload) => {
    state.information = payload;
}