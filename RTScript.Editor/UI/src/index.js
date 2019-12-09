"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var preact_1 = require("preact");
require("./index.css");
var App_1 = require("./App");
var mountNode = document.getElementById('root');
preact_1.render(preact_1.h(App_1.default, null), mountNode, mountNode);
if (module.hot) {
    module.hot.accept();
}
