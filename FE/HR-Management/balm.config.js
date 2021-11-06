const config = require('./config/balmrc');
const publish = require('./config/balm.publish');

const api = mix => {
  if (mix.env.isDev) {
    mix.copy('node_modules/balm-ui/fonts/*', 'app/fonts');
  }
};

module.exports = () => {
  return {
    config,
    api
  };
};
