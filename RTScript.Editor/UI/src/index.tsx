import { h, render } from "preact";
import "./index.css";
import App from "./App"

declare const module: any

const mountNode = document.getElementById('root');

render(<App />, mountNode, mountNode);

if (module.hot) {
    module.hot.accept();
}