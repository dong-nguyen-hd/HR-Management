module.exports = {
  presets: [['@babel/preset-env', { modules: false }]],
  plugins: ['@babel/plugin-transform-runtime'],
  env: {
    test: {
      presets: ['@babel/preset-env']
    }
  }
};
