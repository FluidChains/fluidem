const path = require("path");

module.exports = {
  paths: function (paths) {
    paths.appIndexJs = path.resolve(__dirname, "src/example/index.js");
    return paths;
  },
};
