import { h } from "preact";
import LangService from './services/RTLang.service';
import Editor from "./Editor";

export default _ => (
    <div>
        <Editor service={new LangService()} />
    </div>
)