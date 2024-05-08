module.exports = {
  "/api/eventplacement": {
    target:
      process.env["services__eventplacementapi__https__0"] ||
      process.env["services__eventplacementapi__http__0"],
    secure: process.env["NODE_ENV"] !== "development",
    pathRewrite: {
      "^/api": "",
    },
  },
  "/api/cart": {
    target:
      process.env["services__cartapi__https__0"] ||
      process.env["services__cartapi__http__0"],
    secure: process.env["NODE_ENV"] !== "development",
    pathRewrite: {
      "^/api": "",
    },
  },
};
