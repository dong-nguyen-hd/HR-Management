export function getInformation(state) {
  return state.information;
}

export function getRole(state) {
  return state.information.role;
}

export function getToken(state) {
  return state.token;
}

export function isAuthenticated(state) {
  return state.isAuthenticated;
}
